using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SyncLoopLibrary.Converters
{
    /// <summary>
    /// Enumeration converter.
    /// </summary>
    public class EnumConverter : IValueConverter
    {
        /// <summary>
        /// Enumeration converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        /// <summary>
        /// Enumeration converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }

    /// <summary>
    /// Enumeration to string value converter.
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        /// <summary>
        /// Enumeration to string value converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string EnumString;
            try
            {
                EnumString = Enum.GetName((value.GetType()), value);
                return EnumString;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Enumeration to string value converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Document mode to color converter.
    /// </summary>
    [ValueConversion(typeof(DocumentMode), typeof(SolidColorBrush))]
    public class DocumentModeToColorConverter : IValueConverter
    {
        /// <summary>
        /// Document mode to color converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DocumentMode))
                throw new ArgumentException("Value not of type DocumentMode");

            DocumentMode mode = (DocumentMode)value;

            SolidColorBrush result = null;

            //sanity checks
            switch (mode)
            {
                case DocumentMode.Excel:
                    result = new SolidColorBrush(Color.FromRgb(65, 105, 225));
                    break;

                case DocumentMode.Subtitles:
                    result = new SolidColorBrush(Color.FromRgb(130, 160, 30));
                    break;

                case DocumentMode.RTF:
                    result = new SolidColorBrush(Color.FromRgb(85, 80, 160));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Document mode to color converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Document mode to visibility converter.
    /// </summary>
    [ValueConversion(typeof(DocumentMode), typeof(Visibility))]
    public class ModeToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Document mode to visibility converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DocumentMode))
                throw new ArgumentException("Value not of type DocumentMode");

            DocumentMode mode = (DocumentMode)value;

            string label = parameter as string;

            Visibility result = Visibility.Visible;

            //sanity checks
            switch (mode)
            {
                case DocumentMode.Excel:

                    if (label == "Center")
                    {
                        result = Visibility.Visible;
                    }
                    else
                    {
                        result = Visibility.Hidden;
                    }

                    break;

                case DocumentMode.Subtitles:

                    result = Visibility.Visible;

                    break;

                case DocumentMode.RTF:

                    if (label == "Side")
                    {
                        result = Visibility.Visible;
                    }
                    else
                    {
                        result = Visibility.Hidden;
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Document mode to visibility converter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
