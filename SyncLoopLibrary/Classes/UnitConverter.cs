using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Converts all the meassurement units present in the document from the English system to the metric system.
    /// </summary>
    public class UnitConverter
    {
        #region CONSTANTS

        //---------------------------------------------------------- Length
        /// <summary>
        /// Miles to kilometers.
        /// </summary>
        private const double MILES2KM = 1.60934;

        /// <summary>
        /// Inches to centimeters.
        /// </summary>
        private const double INCHES2CM = 2.54;

        /// <summary>
        /// Yards to meters.
        /// </summary>
        private const double YARDS2METERS = 0.9144;

        /// <summary>
        /// Feet to meters.
        /// </summary>
        private const double FEET2METERS = 0.3048;

        /// <summary>
        /// Feet to centimeters.
        /// </summary>
        private const double FEET2CM = 30.48;

        //---------------------------------------------------------- Weight
        /// <summary>
        /// Pounds to kilograms.
        /// </summary>
        private const double LB2KG = 0.453592;

        /// <summary>
        /// Short ton to metric ton.
        /// </summary>
        private const double SHORTTON2TON = 0.907185;

        /// <summary>
        /// Ounces to grams.
        /// </summary>
        private const double OUNCES2GR = 28.3495;

        //---------------------------------------------------------- Volume
        /// <summary>
        /// Ounces to mililiters.
        /// </summary>
        private const double OUNCES2ML = 29.5735;

        /// <summary>
        /// Quarts to liters.
        /// </summary>
        private const double QUART2LITRE = 0.946353;

        /// <summary>
        /// Gallons to liters.
        /// </summary>
        private const double GALLONS2LITRE = 3.78541;

        //---------------------------------------------------------- Area
        /// <summary>
        /// Acres to hectares.
        /// </summary>
        private const double ACRES2HECTARES = 0.404686;

        /// <summary>
        /// Square feet to square meters.
        /// </summary>
        private const double SQFT2SQMETERS = 0.092903;

        /// <summary>
        /// Square miles to square kilometers.
        /// </summary>
        private const double SQM2SQK = 2.58999;

        //---------------------------------------------------------- Torque
        /// <summary>
        /// Pounds per feet to kilograms per meter.
        /// </summary>
        private const double LBFT2KGM = 1.48816;

        #endregion



        #region VARIABLES

        /// <summary>
        /// Shorcuts to look for in string.
        /// </summary>
        static string[] units = new string[] { "-MI", "-IN", "-YA", "-FT", "-LB", "-ST", "-OUG", "-OUM", "-QU", "-GA", "-AC", "-SF", "-SM", "-GF", "-LF" };

        /// <summary>
        /// Flag to deal with temperatures, since it is not a single factor.
        /// </summary>
        static bool isTemperature = false;

        #endregion



        #region METHODS

        /// <summary>
        /// Convert meassurement units.
        /// </summary>
        /// <param name="content">Unit string</param>
        /// <returns>Converted strnig.</returns>
        public static string Convert(string content)
        {
            // The string will be divided by these characters.
            string[] separators = new string[] { ", ", ";", ". ", ":", " ", "?", "¿", "¡", "!", "\n", "\r", ".\n", ".\r", ",\r", ",\n" };
            // The final string.
            string converted = String.Empty;
            // The number parsed from string.
            double numberToConvert = 0.0;
            // The converte number.
            double convertedNumber = 0.0;
            // The final conversion factor.
            double factor = 0.0;
            // Spanish units.
            string spanishUnit = String.Empty;
            // The final string with units in words.
            string convertedString = String.Empty;
            // The string separated into words.
            string[] words = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string w in words)
            {
                // Make it upper case.
                string upperCaseWord = w.ToUpper();

                foreach (string u in units)
                {
                    if (upperCaseWord.EndsWith(u))
                    {
                        try
                        {
                            // Change any existing decimal comma to period.
                            string fixedNumber = upperCaseWord.Replace(',', '.');
                            numberToConvert = Double.Parse(fixedNumber.Substring(0, w.Length - u.Length));
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("The number \"" + w + "\" could not be converted." + Environment.NewLine + e.Message, "Alert", MessageBoxButton.OK , MessageBoxImage.Information );
                        }

                        switch (u)
                        {
                            case "-MI":

                                factor = MILES2KM;
                                spanishUnit = "kilómetros";
                                break;

                            case "-IN":

                                factor = INCHES2CM;
                                spanishUnit = "centímetros";
                                break;

                            case "-YA":

                                factor = YARDS2METERS;
                                spanishUnit = "metros";
                                break;

                            case "-FT":

                                if (numberToConvert < 4.0)
                                {
                                    factor = FEET2CM;
                                    spanishUnit = "centímetros";
                                }
                                else
                                {
                                    factor = FEET2METERS;
                                    spanishUnit = "metros";
                                }
                                break;

                            case "-LB":

                                factor = LB2KG;
                                spanishUnit = "kilos";
                                break;

                            case "-ST":

                                factor = SHORTTON2TON;
                                spanishUnit = "toneladas";
                                break;

                            case "-OUG":

                                factor = OUNCES2GR;
                                spanishUnit = "gramos";
                                break;

                            case "-OUM":

                                factor = OUNCES2ML;
                                spanishUnit = "mililitros";
                                break;

                            case "-QU":

                                factor = QUART2LITRE;
                                spanishUnit = "litros";
                                break;

                            case "-GA":

                                factor = GALLONS2LITRE;
                                spanishUnit = "litros";
                                break;

                            case "-AC":

                                factor = ACRES2HECTARES;
                                spanishUnit = "hectáreas";
                                break;

                            case "-SF":

                                factor = SQFT2SQMETERS;
                                spanishUnit = "metros cuadrados";
                                break;

                            case "-SM":

                                factor = SQM2SQK;
                                spanishUnit = "kilómetros cuadrados";
                                break;

                            case "-LF":

                                factor = LBFT2KGM;
                                spanishUnit = "kilos por metro";
                                break;

                            case "-GF":

                                isTemperature = true;
                                spanishUnit = "grados";
                                break;

                            default:

                                factor = 1;
                                spanishUnit = String.Empty;
                                break;
                        }

                        // Number of decimals from settings
                        int decimalPlaces = (Settings.ApplicationSettings.ConverterDecimalPlaces >= 0) ? Settings.ApplicationSettings.ConverterDecimalPlaces : 0;

                        // Do the actual conversion.
                        if (!isTemperature)
                        {
                            convertedNumber = Math.Round(numberToConvert * factor, decimalPlaces);
                        }
                        else
                        {
                            convertedNumber = Math.Round(ConvertTemperature(numberToConvert), decimalPlaces);
                            isTemperature = false;
                        }

                        convertedString = convertedNumber + " " + spanishUnit;

                        /*********************************************************************************************************************
                        /* PATTERN EXPLANATION
                        
                        @"(?<=^|\s)" + w + @"(?=\s|$)"

                        @"
                        (?<=        Assert that the regex below can be matched, with the match ending at this position (positive lookbehind)
                                    Match either the regular expression below (attempting the next alternative only if this one fails)
                        ^           Assert position at the beginning of the string
                        |           Or match regular expression number 2 below (the entire group fails if this one fails to match)
                        \s          Match a single character that is a “whitespace character” (spaces, tabs, and line breaks)
                        )
                        word        Match the characters in the word literally
                        (?=         Assert that the regex below can be matched, starting at this position (positive lookahead)
                                    Match either the regular expression below (attempting the next alternative only if this one fails)
                        \s          Match a single character that is a “whitespace character” (spaces, tabs, and line breaks)
                        |           Or match regular expression number 2 below (the entire group fails if this one fails to match)
                        $           Assert position at the end of the string (or before the line break at the end of the string, if any)
                        )"

                        I added the |,|. to allow for a match on commas and periods at the end too.
                    
                        *********************************************************************************************************************/

                        string pattern = @"(?<=^|\s|\?|¿)" + w + @"(?=\s|$|,|.)";
                        // Replace dot for commas.
                        convertedString = SwapDotAndCommas(convertedString);
                        // Replace the original value with the converted value.
                        content = Regex.Replace(content, pattern, convertedString);
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// Converts temperatures to Celsius.
        /// </summary>
        /// <param name="degreesF"> Degrees F.</param>
        /// <returns>Degrees C.</returns>
        private static double ConvertTemperature(double degreesF)
        {
            double degreesC = (degreesF - 32) / 1.8;
            return degreesC;
        }

        /// <summary>
        /// Swaps dot and commas from English to Spanish format.
        /// Unit tested.
        /// </summary>
        /// <param name="wordToSwap">Number to swap dots and commas.</param>
        /// <returns>Converted number.</returns>
        private static string SwapDotAndCommas(string wordToSwap)
        {
            // Convert word to char array.
            char[] letters = wordToSwap.ToCharArray();
            // Loop and change them.
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == ',')
                {
                    letters[i] = '.';
                }
                else if (letters[i] == '.')
                {
                    letters[i] = ',';
                }
            }
            return new string(letters);
        }

        #endregion

    }

}
