using System;
using System.Globalization;
using System.Windows.Data;
using FinanceControlCore.Enums;

namespace FinanceControlDesktop.Converters
{
    public class ValorComSinalConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is TipoTransacao tipo && values[1] is decimal valor)
            {
                string sinal = tipo == TipoTransacao.Entrada ? "+" : "\u2212"; // + ou −
                return $"{sinal} {valor.ToString("C", culture)}";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}