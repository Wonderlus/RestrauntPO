using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Restraunt.Converters
{
    public class ImagePathToPackUriConverter : IValueConverter
    {
        private const string DefaultImage =
            "pack://application:,,,/Assets/Images/Dishes/default.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path || string.IsNullOrWhiteSpace(path))
                return new BitmapImage(new Uri(DefaultImage));

            try
            {
                // формируем pack URI из строки БД
                var uri = $"pack://application:,,,/{path}";
                return new BitmapImage(new Uri(uri, UriKind.Absolute));
            }
            catch
            {
                return new BitmapImage(new Uri(DefaultImage));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
