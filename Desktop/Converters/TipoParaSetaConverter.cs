using System;
using System.Globalization;
using System.Windows.Data;
using FinanceControlCore.Enums;

namespace FinanceControlDesktop.Converters
{
    public class TipoParaSetaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TipoTransacao tipo && tipo == TipoTransacao.Entrada)
                return "\u2191"; // ↑ (entrada = entra dinheiro)
            return "\u2193";     // ↓ (saída = sai dinheiro)
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}