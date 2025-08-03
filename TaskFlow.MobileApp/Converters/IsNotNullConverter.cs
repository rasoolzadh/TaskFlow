// TaskFlow.MobileApp/Converters/IsNotNullConverter.cs

using System.Globalization;

namespace TaskFlow.MobileApp.Converters
{
    /// <summary>
    /// A value converter that returns true if the input object is not null, and false if it is.
    /// </summary>
    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
