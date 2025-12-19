using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Restraunt.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            
            return status switch
            {
                "принят" => new SolidColorBrush(Color.FromRgb(59, 130, 246)),      // Синий
                "готовится" => new SolidColorBrush(Color.FromRgb(245, 158, 11)),   // Оранжевый/Янтарный
                "готов" => new SolidColorBrush(Color.FromRgb(34, 197, 94)),        // Зелёный
                "доставляется" => new SolidColorBrush(Color.FromRgb(99, 102, 241)), // Индиго
                "выполнен" => new SolidColorBrush(Color.FromRgb(22, 163, 74)),     // Тёмно-зелёный
                "отменен" => new SolidColorBrush(Color.FromRgb(239, 68, 68)),      // Красный
                _ => new SolidColorBrush(Color.FromRgb(155, 74, 30))               // Оранжевый текст по умолчанию
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
