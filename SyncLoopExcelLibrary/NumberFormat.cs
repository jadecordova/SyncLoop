using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopExcelLibrary
{
    /// <summary>
    /// Number format for cells. Child of Styles.
    /// </summary>
    public class NumberFormat
    {

        #region ----------------------------------------------------------------------ENUMERATIONS

        public enum Format
        {
            General,
            GeneralNumber,
            GeneralDate,
            LongDate,
            MediumDate,
            ShortDate,
            LongTime,
            MediumTime,
            ShortTime,
            Currency,
            EuroCurrency,
            Fixed,
            Standard,
            Percent,
            Scientific,
            YesNo,
            TrueFalse,
            OnOff
        }

        #endregion

        #region----------------------------------------------------------------------PROPERTIES

        public Format CellNumberFormat { get; set; }

        #endregion

        #region ----------------------------------------------------------------------CONSTRUCTORS

        public NumberFormat(Format cellNumberFormat)
        {
            CellNumberFormat = cellNumberFormat;
        }

        #endregion

        #region ----------------------------------------------------------------------METHODS

        public string WriteNumberFormat()
        {
            // Result constructor.
            StringBuilder format = new StringBuilder();
            // Header.
            format.Append(ExcelUtilities.Indent3 + @"<NumberFormat ss:Format=");

            string chosenFormat = String.Empty;

            switch (CellNumberFormat)
            {
                case Format.General:
                    chosenFormat = "General";
                    break;
                case Format.GeneralNumber:
                    chosenFormat = "General Number";
                    break;
                case Format.GeneralDate:
                    chosenFormat = "General Date";
                    break;
                case Format.LongDate:
                    chosenFormat = "Long Date";
                    break;
                case Format.MediumDate:
                    chosenFormat = "Medium Date";
                    break;
                case Format.ShortDate:
                    chosenFormat = "Short Date";
                    break;
                case Format.LongTime:
                    chosenFormat = "Long Time";
                    break;
                case Format.MediumTime:
                    chosenFormat = "Medium Time";
                    break;
                case Format.ShortTime:
                    chosenFormat = "Short Time";
                    break;
                case Format.Currency:
                    chosenFormat = "Currency";
                    break;
                case Format.EuroCurrency:
                    chosenFormat = "Euro Currency";
                    break;
                case Format.Fixed:
                    chosenFormat = "Fixed";
                    break;
                case Format.Standard:
                    chosenFormat = "Standard";
                    break;
                case Format.Percent:
                    chosenFormat = "Percent";
                    break;
                case Format.Scientific:
                    chosenFormat = "Scientific";
                    break;
                case Format.YesNo:
                    chosenFormat = "Yes/No";
                    break;
                case Format.TrueFalse:
                    chosenFormat = "True/False";
                    break;
                case Format.OnOff:
                    chosenFormat = "On/Off";
                    break;
                default:
                    break;
            }

            format.AppendLine(ExcelUtilities.Quote + chosenFormat + ExcelUtilities.Quote + @" />");

            return format.ToString();
        }

        #endregion
    }
}
