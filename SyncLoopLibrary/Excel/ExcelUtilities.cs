using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Library for generating Excel documents.
    /// </summary>
    public static class ExcelUtilities
    {

        #region FIELDS

        // Number of columns in the report.
        static int numberOfColumns = 10;

        #endregion



        #region PROPERTIES

        /// <summary>
        /// 1st level indent.
        /// </summary>
        public static string Indent1 { get; set; } = "  ";

        /// <summary>
        /// 2nd level indent.
        /// </summary>
        public static string Indent2 { get; set; } = "    ";

        /// <summary>
        /// 3rd level indent.
        /// </summary>
        public static string Indent3 { get; set; } = "      ";

        /// <summary>
        ///  4th level indent.
        /// </summary>
        public static string Indent4 { get; set; } = "        ";

        /// <summary>
        /// 5th level indent.
        /// </summary>
        public static string Indent5 { get; set; } = "          ";

        /// <summary>
        /// 6th level indent.
        /// </summary>
        public static string Quote { get; set; } = "\"";

        /// <summary>
        /// New line character.
        /// </summary>
        public static string NewLine { get; set; } = @"&#10;";

        #endregion



        #region METHODS

        /// <summary>
        /// Creates financial report in Excel format.
        /// </summary>
        /// <param name="listOfCharacters">List of character objects.</param>
        /// <param name="data">Program info.</param>
        /// <param name="listOfLoops">List of loops.</param>
        /// <returns></returns>
        public static string CreateExcelDocument(List<Character> listOfCharacters, ProgramInfo data, List<Loop> listOfLoops)
        {

            // This variable will count the number of characters without lines.
            // Those characters will be added as empty lines at the end
            // of the info sheet.
            // int charactersWithoutLines = 0;

            #region DOCUMENT HEADER

            ExcelDocumentHeader documentHeader = new ExcelDocumentHeader();

            #endregion



            #region DOCUMENT PROPERTIES

            // Create document properties.
            DocumentProperties documentProperties = new DocumentProperties();
            // Add title.
            documentProperties.DocumentTitle = "Glyphos, Servicios de Comunicación";
            // Add date.
            documentProperties.DocumentDateCreated = DateTime.Now.ToString("dd-MM-yyyy");

            #endregion



            #region EXCEL WORKBOOK

            // Create Excel document properties.
            ExcelWorkbook excelWorkbook = new ExcelWorkbook();

            #endregion



            #region DOCUMENT STYLES

            // Create list of styles.
            List<Style> stylesList = new List<Style>();
            // Default.
            stylesList.Add(new Style()
            {
                ID = "Default",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Bottom),
                Font = new StyleFont("Calibri", "11", "#000000")
            });
            // Borders.
            stylesList.Add(new Style()
            {
                ID = "Borders",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Calibri", "12", "#000000")
            });
            // Text wrap, no borders.
            stylesList.Add(new Style()
            {
                ID = "TextWrapNoBorders",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Font = new StyleFont("Calibri", "12", "#000000")
            });
            // Grey background with borders.
            stylesList.Add(new Style()
            {
                ID = "GreyBackgroundWithBorders",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior("#969696", StyleInterior.InteriorPattern.Solid),
            });
            // Courier New with borders, left aligned.
            stylesList.Add(new Style()
            {
                ID = "CourierNewWithBordersLeftAligned",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Courier New", "14", "#000000")
            });
            // Calibri with borders, left aligned.
            stylesList.Add(new Style()
            {
                ID = "CalibriWithBordersLeftAligned",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Calibri", "12", "#000000")
            });
            // Calibri 14.
            stylesList.Add(new Style()
            {
                ID = "Calibri12",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000")
            });
            // Borders, wrap, bold.
            stylesList.Add(new Style()
            {
                ID = "BordersWrapBold",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold)
            });
            // Borders, wrap, bold, left aligned.
            stylesList.Add(new Style()
            {
                ID = "BordersWrapBoldLeftAligned",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Center, StyleAlignment.TextWrapping.Yes),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Left, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Right, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                    new StyleBorder(StyleBorder.BorderType.Top, StyleBorder.LineStyle.Continuous, "1", "#000000"),
                },
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold)
            });

            // Create styles collection.
            DocumentStyles documentStyles = new DocumentStyles(stylesList.ToArray());

            #endregion



            #region INFO WORKSHEET COLUMNS
            // TODO: CONTINUE HERE.
            // Create columns list for info sheet.
            List<Column> infoSheetColumns = new List<Column>();
            // Program.
            infoSheetColumns.Add(new Column("187.5", "TextWrapNoBorders", Column.AutoFit.False));
            // ID.
            infoSheetColumns.Add(new Column("82.5", "TextWrapNoBorders", Column.AutoFit.False));
            // Characters.
            infoSheetColumns.Add(new Column("108.75", "TextWrapNoBorders", Column.AutoFit.False));
            // Gender.
            infoSheetColumns.Add(new Column("66.75", "TextWrapNoBorders", Column.AutoFit.False));
            // Lines.
            infoSheetColumns.Add(new Column("66.75", "TextWrapNoBorders", Column.AutoFit.False));

            #endregion



            #region INFO WORKSHEET HEADER CELLS

            // Create cell list.
            List<Cell> infoHeaderCells = new List<Cell>();
            // Program.
            infoHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Programa"));
            // ID.
            infoHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "ID"));
            // Characters.
            infoHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Personajes"));
            // Gender.
            infoHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Genero"));
            // Lines.
            infoHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Lineas"));

            #endregion



            #region INFO WORKSHEET HEADER ROW

            // Create row list.
            List<Row> infoSheetRows = new List<Row>();
            // Add headers.
            infoSheetRows.Add(new Row(infoHeaderCells));

            #endregion



            #region INFO WORKSHEET CELLS

            // Get number of characters in program.
            // We have to iterate through the character list and discard
            // any character with 0 lines.
            // Let's create a new clean character list.
            List<Character> characters = new List<Character>();
            // And save the number of characters.
            int numberOfCharacters = 0;

            foreach (Character character in listOfCharacters)
            {
                if (character.Lines > 0)
                {
                    characters.Add(character);
                    numberOfCharacters++;
                }
            }

            // Check if there are characters.
            if (numberOfCharacters > 0)
            {
                // Number of columns to merge is one more than the number of characters to account for header row.
                // Minimum of 10 to allow space for program information on first row.
                int cellsToMergeDown = (numberOfCharacters >= 10) ? numberOfCharacters - 1 : 9;
                // Cell data.
                string cellData = String.Empty;
                // Create a variable to hold the gender.
                // We must check for gender = "NONE".
                // If so, the final gender should be an empty string,
                // so it doesn't appear the word "NONE" in the Excel document.
                string gender = (characters[0].Gender.ToString().ToUpper() == "NONE") ? String.Empty : characters[0].Gender.ToString();

                // First row.
                infoSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                            new Cell(
                                "Borders",
                                Cell.DataType.String,
                                data.EpisodeSeries.NameEnglish + NewLine +
                                data.EpisodeNameEnglish + NewLine + NewLine +
                                data.EpisodeNumber + NewLine + NewLine +
                                data.EpisodeSeries.NameSpanish + NewLine +
                                data.EpisodeNameSpanish,
                                new CellMerge(0, cellsToMergeDown),
                                ""),
                            new Cell(
                                "Borders",
                                Cell.DataType.String,
                                data.EpisodeCode,
                                new CellMerge(0, cellsToMergeDown),
                                ""),
                            new Cell(
                                "Borders",
                                Cell.DataType.String,
                                characters[0].Name),
                            new Cell(
                                "Borders",
                                Cell.DataType.String,
                                gender),
                            new Cell(
                                "Borders",
                                Cell.DataType.Number,
                                characters[0].Lines.ToString())
                        }));
                // Characters.
                for (int i = 1; i < numberOfCharacters; i++)
                {
                    // Create a variable to hold the gender.
                    // We must check for gender = "NONE".
                    // If so, the final gender should be an empty string,
                    // so it doesn't appear the word "NONE" in the Excel document.
                    gender = (characters[i].Gender.ToString().ToUpper() == "NONE") ? String.Empty : characters[i].Gender.ToString();
                    // Now we create the cell.
                    infoSheetRows.Add(
                        new Row(
                            new List<Cell>()
                            {
                                    new Cell("Borders", Cell.DataType.String, characters[i].Name,"3"),
                                    new Cell("Borders", Cell.DataType.String, gender),
                                    new Cell("Borders", Cell.DataType.Number, characters[i].Lines.ToString())
                            }));
                }

                // Add aditional rows if needed for space for first row.
                if (characters.Count < 10)
                {
                    for (int i = 1; i < (11 - characters.Count); i++)
                    {
                        infoSheetRows.Add(
                            new Row(
                                new List<Cell>()
                                {
                                    new Cell("Borders", Cell.DataType.String, "", "3"),
                                    new Cell("Borders", Cell.DataType.String, ""),
                                    new Cell("Borders", Cell.DataType.String, "")
                                }));
                    }
                }

                if (Settings.ApplicationSettings.WriteExcelTotalsRow)
                {
                    // Total lines row.
                    infoSheetRows.Add(
                            new Row(
                                new List<Cell>()
                                {
                                    new Cell("Borders", Cell.DataType.String, "TOTAL DE LINEAS",new CellMerge(1, 0), "3"),
                                    new Cell()
                                    {
                                        CellStyleID = "Borders",
                                        CellDataType = Cell.DataType.Number,
                                        CellFormula = @"=SUM(R[-" + (cellsToMergeDown + 1).ToString() + "]C:R[-1]C)"
                                    }
                                }));
                }
            }

            #endregion



            #region INFO SHEET TABLE

            // Create table.
            Table infoTable = new Table(infoSheetRows, infoSheetColumns);

            #endregion



            #region INFO WORKSHEET

            // Create worksheets list.
            List<Worksheet> bookWorksheets = new List<Worksheet>();
            // Add info worksheet.
            bookWorksheets.Add(new Worksheet("spa-dub-card", infoTable, new WorksheetOptions(
                new PageSetup(
                    new Layout(true),
                    new Header(0.19685039370078741,
                                "&amp;ZProgram&#10;Episode&amp;C&#10;Ep. XX&amp;DPrograma&#10;Episodio"),
                    new Footer(0.19685039370078741,
                                "&amp;ZTraductor&amp;DPágina &amp;P"),
                    new PageMargins(0.59055118110236227, 0.39370078740157483, 0.39370078740157483, 0.78740157480314965)))));

            #endregion



            #region PROGRAM WORKSHEET COLUMNS

            // Create columns list for program sheet.
            // IMPORTANT! The width reported in Excel should be multiplied by 6 to get the number to use here.
            List<Column> programSheetColumns = new List<Column>();
            // Loop.
            if (Settings.ApplicationSettings.HideLoopAndEnglishColumns)
            {
                programSheetColumns.Add(new Column("31.74", "TextWrapNoBorders", Column.AutoFit.False, Column.ColumnHidden.True));
            }
            else
            {
                programSheetColumns.Add(new Column("31.74", "TextWrapNoBorders"));
            }
            // Timecode.
            programSheetColumns.Add(new Column("82.5", "TextWrapNoBorders"));
            // Character.
            programSheetColumns.Add(new Column("108.75", "TextWrapNoBorders"));
            // English.
            if (Settings.ApplicationSettings.HideLoopAndEnglishColumns)
            {
                programSheetColumns.Add(new Column("42", "TextWrapNoBorders", Column.AutoFit.False, Column.ColumnHidden.True));
            }
            else
            {
                programSheetColumns.Add(new Column("42", "TextWrapNoBorders"));
            }
            // Spahish.
            programSheetColumns.Add(new Column("318.75", "TextWrapNoBorders"));
            // Lines.
            programSheetColumns.Add(new Column("30", "TextWrapNoBorders"));

            #endregion



            #region PROGRAM WORKSHEET HEADER CELLS

            // Create cell list.
            List<Cell> programHeaderCells = new List<Cell>();
            // Loop.
            programHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Loop"));
            // Timecode.
            programHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Timecode"));
            // Character.
            programHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Character"));
            // English.
            programHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "English"));
            // Spanish.
            programHeaderCells.Add(new Cell("GreyBackgroundWithBorders", Cell.DataType.String, "Spanish"));

            #endregion



            #region PROGRAM WORKSHEET HEADER ROW

            // Create row list.
            List<Row> programSheetRows = new List<Row>();
            // Add headers.
            programSheetRows.Add(new Row(programHeaderCells));

            #endregion



            #region PROGRAM ROWS

            // Check if there are characters.
            if (listOfLoops.Count > 0)
            {
                int loopCounter = 1;
                foreach (Loop loop in listOfLoops)
                {
                    if (loop.Mode == DocumentMode.Excel && !String.IsNullOrEmpty(loop.CharacterDialog))
                    {
                        if (Settings.ApplicationSettings.UseCourierNewForContentInExcel)
                        {
                            if (Settings.ApplicationSettings.UseInternalLineCount)
                            {
                                programSheetRows.Add(
                                   new Row(
                                       new List<Cell>()
                                       {
                                    new Cell("Borders", Cell.DataType.Number, loopCounter.ToString()),
                                    new Cell("Borders", Cell.DataType.String, loop.Timecode),
                                    new Cell("Borders", Cell.DataType.String, loop.Character.Name),
                                    new Cell("Borders", Cell.DataType.String, string.Empty,"4"),
                                    new Cell("CourierNewWithBordersLeftAligned", Cell.DataType.String, loop.CharacterDialog,"5"),
                                    new Cell("Borders", Cell.DataType.Number, loop.LoopLines.ToString()),
                               }));
                            }
                            else
                            {
                                programSheetRows.Add(
                                   new Row(
                                       new List<Cell>()
                                       {
                                    new Cell("Borders", Cell.DataType.Number, loopCounter.ToString()),
                                    new Cell("Borders", Cell.DataType.String, loop.Timecode),
                                    new Cell("Borders", Cell.DataType.String, loop.Character.Name),
                                    new Cell("Borders", Cell.DataType.String, string.Empty,"4"),
                                    new Cell("CourierNewWithBordersLeftAligned", Cell.DataType.String, loop.CharacterDialog,"5"),
                                    new Cell()
                                    {
                                        CellStyleID = "Calibri12",
                                        CellDataType = Cell.DataType.Number,
                                        CellFormula = @"=IF((LEN(RC[-1]))<57,1,ROUNDUP(((LEN(RC[-1]))/57),0))"
                                    }
                               }));
                            }
                        }
                        else
                        {
                            if (Settings.ApplicationSettings.UseInternalLineCount)
                            {
                                programSheetRows.Add(
                                   new Row(
                                       new List<Cell>()
                                       {
                                    new Cell("Borders", Cell.DataType.Number, loopCounter.ToString()),
                                    new Cell("Borders", Cell.DataType.String, loop.Timecode),
                                    new Cell("Borders", Cell.DataType.String, loop.Character.Name),
                                    new Cell("Borders", Cell.DataType.String, string.Empty,"4"),
                                    new Cell("CalibriWithBordersLeftAligned", Cell.DataType.String, loop.CharacterDialog,"5"),
                                    new Cell("Calibri12", Cell.DataType.Number, loop.LoopLines.ToString()),
                               }));
                            }
                            else
                            {
                                programSheetRows.Add(
                                   new Row(
                                       new List<Cell>()
                                       {
                                    new Cell("Borders", Cell.DataType.Number, loopCounter.ToString()),
                                    new Cell("Borders", Cell.DataType.String, loop.Timecode),
                                    new Cell("Borders", Cell.DataType.String, loop.Character.Name),
                                    new Cell("Borders", Cell.DataType.String, string.Empty,"4"),
                                    new Cell("CalibriWithBordersLeftAligned", Cell.DataType.String, loop.CharacterDialog,"5"),
                                    new Cell()
                                    {
                                        CellStyleID = "Calibri12",
                                        CellDataType = Cell.DataType.Number,
                                        CellFormula = @"=IF((LEN(RC[-1]))<57,1,ROUNDUP(((LEN(RC[-1]))/57),0))"
                                    }
                               }));
                            }
                        }

                        loopCounter++;
                    }
                }
            }


            #endregion



            #region PROGRAM SHEET TABLE

            // Create table.
            Table programTable = new Table(programSheetRows, programSheetColumns);

            #endregion



            #region PROGRAM WORKSHEET

            // Add info worksheet.
            bookWorksheets.Add(new Worksheet("spa-dub-script", programTable, new WorksheetOptions(
                new PageSetup(
                    new Layout(true),
                    new Header(0.19685039370078741,
                                "&amp;ZProgram&#10;Episode&amp;C&#10;Ep. XX&amp;DPrograma&#10;Episodio"),
                    new Footer(0.19685039370078741,
                                "&amp;ZTraductor&amp;DPágina &amp;P"),
                    new PageMargins(0.59055118110236227, 0.39370078740157483, 0.39370078740157483, 0.78740157480314965)))));

            #endregion

            // Create document.
            Workbook document = new Workbook(documentStyles, documentProperties, excelWorkbook, bookWorksheets);

            // Debug.Write(document.WriteBook());

            // Open in Excel.
            //Process.Start(path);

            return document.WriteBook();
        }


        /// <summary>
        /// Creates financial report in Excel format.
        /// </summary>
        /// <param name="reportData">Report object.</param>
        /// <returns>Report in Excel XML format.</returns>
        public static string CreateExcelReport(Report reportData)
        {

            #region VARIABLES

            // Background color for totals.
            string titleAndTotalsColor = "#f0f4c3";
            // Background color for headers.
            string headersColor = "#bdc192";
            // Adittional color.
            string additionalColor = "#fffff6";

            #endregion



            #region DOCUMENT HEADER

            ExcelDocumentHeader documentHeader = new ExcelDocumentHeader();

            #endregion



            #region DOCUMENT PROPERTIES

            // Create document properties.
            DocumentProperties documentProperties = new DocumentProperties();
            // Add title.
            documentProperties.DocumentTitle = reportData.Translator;
            // Add date.
            documentProperties.DocumentDateCreated = DateTime.Now.ToString("dd-MM-yyyy");

            #endregion



            #region OFFICE DOCUMENT PROPERTIES

            OfficeDocumentSettings officeSettings = new OfficeDocumentSettings(
                                                        new Colors(
                                                            new List<CustomColor>()
                                                            {
                                                                new CustomColor("#FFFFF0", 35),
                                                                new CustomColor("#F0F4C3", 37),
                                                                new CustomColor("#BDC192", 39)
                                                            }));

            #endregion



            #region EXCEL WORKBOOK

            // Create Excel document properties.
            ExcelWorkbook excelWorkbook = new ExcelWorkbook();

            #endregion



            #region DOCUMENT STYLES

            // Create list of styles.
            List<Style> stylesList = new List<Style>();
            // Default.
            stylesList.Add(new Style()
            {
                ID = "Default",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Bottom),
                Font = new StyleFont("Calibri", "12", "#000000"),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Centered.
            stylesList.Add(new Style()
            {
                ID = "Centered",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Company name.
            stylesList.Add(new Style()
            {
                ID = "Company",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Titles and totals.
            stylesList.Add(new Style()
            {
                ID = "TitleAndTotals",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Titles and totals bold.
            stylesList.Add(new Style()
            {
                ID = "TitleAndTotalsBold",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Right, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }

            });
            // Headers.
            stylesList.Add(new Style()
            {
                ID = "Headers",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(headersColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "NumberCell",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Right, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "NumberAsText",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.General),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "TextCell",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });

            // Create styles collection.
            DocumentStyles documentStyles = new DocumentStyles(stylesList.ToArray());

            #endregion



            #region WORKSHEET COLUMNS

            // Create columns list for report sheet.
            List<Column> reportSheetColumns = new List<Column>();
            // Channel.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Series.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Episode.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Number.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Code.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Duration.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Rate.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Rate amount.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Date.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Subtotal.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });

            #endregion



            #region WORKSHEET HEADER ROWS

            // Create row list.
            List<Row> reportSheetRows = new List<Row>();
            // Add headers.
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "Company",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String,
                            CellData = reportData.Translator
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String,
                            CellData = "Reporte de trabajo - " + reportData.Month + @"/" + reportData.Year
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));

            #endregion



            #region WORKSHEET ROWS

            // Loop throug channels.
            foreach (ChannelReport report in reportData.Reports)
            {
                // Add header.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                            new Cell("Headers", Cell.DataType.String, "Canal"),
                            new Cell("Headers", Cell.DataType.String, "Serie"),
                            new Cell("Headers", Cell.DataType.String, "Episodio"),
                            new Cell("Headers", Cell.DataType.String, "Número"),
                            new Cell("Headers", Cell.DataType.String, "Código"),
                            new Cell("Headers", Cell.DataType.String, "Fecha"),
                            new Cell("Headers", Cell.DataType.String, "Duración"),
                            new Cell("Headers", Cell.DataType.String, "Tipo"),
                            new Cell("Headers", Cell.DataType.String, "Tarifa"),
                            new Cell("Headers", Cell.DataType.String, "Subtotal"),
                        }));

                // Get a sorted list.
                List<ProgramInfo> sortedPrograms = new List<ProgramInfo>((ObservableCollection<ProgramInfo>)report.DataContext);

                sortedPrograms.Sort((x, y) => DateTime.Compare(x.DateDelivered, y.DateDelivered));

                // Loop through programs.
                foreach (ProgramInfo program in sortedPrograms)
                {
                    // Create row.
                    List<Cell> rowCells = new List<Cell>();
                    // Add cells to list.

                    // CHANNEL.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = report.Channel.Name.ToUpper(),
                        CellFormula = ""
                    });

                    // SERIES.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeSeries.NameEnglish,
                        CellFormula = ""
                    });

                    // EPISODE NAME.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeNameEnglish,
                        CellFormula = ""
                    });

                    // EPISODE NUMBER.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeNumber,
                        CellFormula = ""
                    });

                    // EPISODE CODE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeCode,
                        CellFormula = ""
                    });

                    // EPISODE DATE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.DateDelivered.ToString("dd/MM/yyy"),
                        CellFormula = ""
                    });

                    // EPISODE DURATION.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberAsText",
                        CellDataType = Cell.DataType.Number,
                        CellData = program.Duration.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE RATE TYPE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.Rate.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE RATE AMOUNT.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberCell",
                        CellDataType = Cell.DataType.Number,
                        CellData = program.RateAmount.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE SUBTOTAL.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberCell",
                        CellDataType = Cell.DataType.Number,
                        CellData = "0", // program.Amount.ToString(),
                        CellFormula = "=RC[-1]*RC[-3]"
                    });

                    // Add row.
                    reportSheetRows.Add(new Row(rowCells));
                }

                // SUBTOTAL ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                            new Cell()
                            {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "SUBTOTAL:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula = "=SUM(R[-" + (((ObservableCollection<ProgramInfo>)report.DataContext).Count).ToString() + "]C:R[-1]C)"
                                }
                }));

                // IVA ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "I.V.A.:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula = "=R[-1]C*" + (report.IVA/100).ToString()
                                }
                }));

                // TOTAL ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "TOTAL:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula ="=R[-2]C+R[-1]C"
                                }
                }));

                // Blank row.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String }
                }));
            }

            #endregion



            #region WORKSHEET TABLE

            // Create table.
            Table reportTable = new Table(reportSheetRows, reportSheetColumns);

            #endregion



            #region REPORT WORKSHEET

            // Create worksheets list.
            List<Worksheet> bookWorksheets = new List<Worksheet>();
            // Add info worksheet.
            bookWorksheets.Add(new Worksheet(
                    "REPORTE",
                reportTable,
                    new WorksheetOptions(
                        new PageSetup(
                        new Layout(true),
                        new Header(0.19685039370078741,
                                    "Glyphos, Servicios de Comunicación, C. A. - Reporte de Trabajo - " + reportData.Month + @"/" + reportData.Year),
                                    null,
                                    new PageMargins(0.59055118110236227, 0.39370078740157483, 0.39370078740157483, 0.78740157480314965)))));

            #endregion


            // Create document.
            Workbook document = new Workbook(documentStyles, documentProperties, excelWorkbook, bookWorksheets, officeSettings);

            // Debug.Write(document.WriteBook());

            return document.WriteBook();


        }


        /// <summary>
        /// Creates financial report in Excel format.
        /// </summary>
        /// <param name="reportData">Report object.</param>
        /// <returns>Report in Excel XML format.</returns>
        [Obsolete]
        public static string CreateExcelReportOld(Report reportData)
        {

            #region VARIABLES

            // Background color for totals.
            string titleAndTotalsColor = "#f0f4c3";
            // Background color for headers.
            string headersColor = "#bdc192";
            // Adittional color.
            string additionalColor = "#fffff6";

            #endregion



            #region DOCUMENT HEADER

            ExcelDocumentHeader documentHeader = new ExcelDocumentHeader();

            #endregion



            #region DOCUMENT PROPERTIES

            // Create document properties.
            DocumentProperties documentProperties = new DocumentProperties();
            // Add title.
            documentProperties.DocumentTitle = reportData.Translator;
            // Add date.
            documentProperties.DocumentDateCreated = DateTime.Now.ToString("dd-MM-yyyy");

            #endregion



            #region OFFICE DOCUMENT PROPERTIES

            OfficeDocumentSettings officeSettings = new OfficeDocumentSettings(
                                                        new Colors(
                                                            new List<CustomColor>()
                                                            {
                                                                new CustomColor("#FFFFF0", 35),
                                                                new CustomColor("#F0F4C3", 37),
                                                                new CustomColor("#BDC192", 39)
                                                            }));

            #endregion



            #region EXCEL WORKBOOK

            // Create Excel document properties.
            ExcelWorkbook excelWorkbook = new ExcelWorkbook();

            #endregion



            #region DOCUMENT STYLES

            // Create list of styles.
            List<Style> stylesList = new List<Style>();
            // Default.
            stylesList.Add(new Style()
            {
                ID = "Default",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Left, StyleAlignment.VerticalAlignment.Bottom),
                Font = new StyleFont("Calibri", "12", "#000000"),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Centered.
            stylesList.Add(new Style()
            {
                ID = "Centered",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Company name.
            stylesList.Add(new Style()
            {
                ID = "Company",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Titles and totals.
            stylesList.Add(new Style()
            {
                ID = "TitleAndTotals",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Titles and totals bold.
            stylesList.Add(new Style()
            {
                ID = "TitleAndTotalsBold",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Right, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(titleAndTotalsColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }

            });
            // Headers.
            stylesList.Add(new Style()
            {
                ID = "Headers",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000", StyleFont.FontWeight.Bold),
                Background = new StyleInterior(headersColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard)
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "NumberCell",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Right, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "NumberAsText",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.General),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });
            // Normal number cell.
            stylesList.Add(new Style()
            {
                ID = "TextCell",
                Alignment = new StyleAlignment(StyleAlignment.HorizontalAlignment.Center, StyleAlignment.VerticalAlignment.Center),
                Font = new StyleFont("Calibri", "12", "#000000"),
                Background = new StyleInterior(additionalColor, StyleInterior.InteriorPattern.Solid),
                CellNumberFormat = new NumberFormat(NumberFormat.Format.Standard),
                Borders = new List<StyleBorder>()
                {
                    new StyleBorder(StyleBorder.BorderType.Bottom, StyleBorder.LineStyle.Continuous, "1", "#777777")
                }
            });

            // Create styles collection.
            DocumentStyles documentStyles = new DocumentStyles(stylesList.ToArray());

            #endregion



            #region WORKSHEET COLUMNS

            // Create columns list for report sheet.
            List<Column> reportSheetColumns = new List<Column>();
            // Channel.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Series.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Episode.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Number.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Code.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Duration.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Rate.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Rate amount.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Date.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });
            // Subtotal.
            reportSheetColumns.Add(new Column() { AutoFitWidth = Column.AutoFit.True, ColumnStyleID = "Centered" });

            #endregion



            #region WORKSHEET HEADER ROWS

            // Create row list.
            List<Row> reportSheetRows = new List<Row>();
            // Add headers.
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "Company",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String,
                            CellData = reportData.Translator
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String,
                            CellData = "Reporte de trabajo - " + reportData.Month + @"/" + reportData.Year
                        }
                    }));
            reportSheetRows.Add(
                new Row(
                    new List<Cell>()
                    {
                        new Cell()
                        {
                            CellStyleID = "TitleAndTotals",
                            Merge = new CellMerge(numberOfColumns-1, 0),
                            CellDataType = Cell.DataType.String
                        }
                    }));

            #endregion



            #region WORKSHEET ROWS

            // Loop throug channels.
            foreach (ChannelReport report in reportData.Reports)
            {
                // Add header.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                            new Cell("Headers", Cell.DataType.String, "Canal"),
                            new Cell("Headers", Cell.DataType.String, "Serie"),
                            new Cell("Headers", Cell.DataType.String, "Episodio"),
                            new Cell("Headers", Cell.DataType.String, "Número"),
                            new Cell("Headers", Cell.DataType.String, "Código"),
                            new Cell("Headers", Cell.DataType.String, "Fecha"),
                            new Cell("Headers", Cell.DataType.String, "Duración"),
                            new Cell("Headers", Cell.DataType.String, "Tipo"),
                            new Cell("Headers", Cell.DataType.String, "Tarifa"),
                            new Cell("Headers", Cell.DataType.String, "Subtotal"),
                        }));
                // Loop through programs.
                foreach (ProgramInfo program in (ObservableCollection<ProgramInfo>)report.DataContext)
                {
                    // Create row.
                    List<Cell> rowCells = new List<Cell>();
                    // Add cells to list.

                    // CHANNEL.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = report.Channel.Name.ToUpper(),
                        CellFormula = ""
                    });

                    // SERIES.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeSeries.NameEnglish,
                        CellFormula = ""
                    });

                    // EPISODE NAME.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeNameEnglish,
                        CellFormula = ""
                    });

                    // EPISODE NUMBER.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeNumber,
                        CellFormula = ""
                    });

                    // EPISODE CODE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.EpisodeCode,
                        CellFormula = ""
                    });

                    // EPISODE DATE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.DateDelivered.ToString("dd/MM/yyy"),
                        CellFormula = ""
                    });

                    // EPISODE DURATION.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberAsText",
                        CellDataType = Cell.DataType.Number,
                        CellData = program.Duration.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE RATE TYPE.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "TextCell",
                        CellDataType = Cell.DataType.String,
                        CellData = program.Rate.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE RATE AMOUNT.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberCell",
                        CellDataType = Cell.DataType.Number,
                        CellData = program.RateAmount.ToString(),
                        CellFormula = ""
                    });

                    // EPISODE SUBTOTAL.
                    rowCells.Add(new Cell()
                    {
                        CellStyleID = "NumberCell",
                        CellDataType = Cell.DataType.Number,
                        CellData = "0", // program.Amount.ToString(),
                        CellFormula = "=RC[-1]*RC[-3]"
                    });

                    // Add row.
                    reportSheetRows.Add(new Row(rowCells));
                }

                // SUBTOTAL ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                            new Cell()
                            {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "SUBTOTAL:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula = "=SUM(R[-" + (((ObservableCollection<ProgramInfo>)report.DataContext).Count).ToString() + "]C:R[-1]C)"
                                }
                }));

                // IVA ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "I.V.A.:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula = "=R[-1]C*" + (report.IVA/100).ToString()
                                }
                }));

                // TOTAL ROW.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.String,
                                    CellData = "TOTAL:",
                                    CellStyleID = "Headers",
                                    CellIndex = (numberOfColumns-1).ToString()
                                },
                                new Cell()
                                {
                                    CellDataType = Cell.DataType.Number,
                                    CellData = "0",
                                    CellStyleID = "TitleAndTotalsBold",
                                    CellFormula ="=R[-2]C+R[-1]C"
                                }
                }));

                // Blank row.
                reportSheetRows.Add(
                    new Row(
                        new List<Cell>()
                        {
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String },
                                new Cell() { CellDataType = Cell.DataType.String }
                }));
            }

            #endregion



            #region WORKSHEET TABLE

            // Create table.
            Table reportTable = new Table(reportSheetRows, reportSheetColumns);

            #endregion



            #region REPORT WORKSHEET

            // Create worksheets list.
            List<Worksheet> bookWorksheets = new List<Worksheet>();
            // Add info worksheet.
            bookWorksheets.Add(new Worksheet(
                    "REPORTE",
                reportTable,
                    new WorksheetOptions(
                        new PageSetup(
                        new Layout(true),
                        new Header(0.19685039370078741,
                                    "Glyphos, Servicios de Comunicación, C. A. - Reporte de Trabajo - " + reportData.Month + @"/" + reportData.Year),
                                    null,
                                    new PageMargins(0.59055118110236227, 0.39370078740157483, 0.39370078740157483, 0.78740157480314965)))));

            #endregion


            // Create document.
            Workbook document = new Workbook(documentStyles, documentProperties, excelWorkbook, bookWorksheets, officeSettings);

            // Debug.Write(document.WriteBook());

            return document.WriteBook();


        }


        #endregion
    }
}
