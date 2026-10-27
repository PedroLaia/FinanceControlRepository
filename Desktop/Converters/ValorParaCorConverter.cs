using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FinanceControlDesktop.Converters
{
    public class ValorParaCorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal valor && valor < 0)
                return new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26)); // vermelho (negativo)
            return new SolidColorBrush(Color.FromRgb(0x16, 0xA3, 0x4A));     // verde (>= 0)
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}