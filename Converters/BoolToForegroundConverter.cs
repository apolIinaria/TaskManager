using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskManager.Converters
{
    // конвертер для перетворення булевого значення в колір тексту
    public class BoolToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
            {
                return new SolidColorBrush(Color.FromRgb(153, 153, 153));
            }
            return new SolidColorBrush(Color.FromRgb(51, 51, 51));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
