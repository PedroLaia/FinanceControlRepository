using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FinanceControlCore.Enums;

namespace FinanceControlDesktop.Converters
{
    public class TipoParaCorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool entrada = value is TipoTransacao t && t == TipoTransacao.Entrada;
            string tom = parameter as string;

            if (tom == "Fundo")
                return entrada
                    ? new SolidColorBrush(Color.FromRgb(0xEA, 0xF3, 0xDE))   // verde claro
                    : new SolidColorBrush(Color.FromRgb(0xFC, 0xEB, 0xEB));  // vermelho claro

            if (tom == "Texto")
                return entrada
                    ? new SolidColorBrush(Color.FromRgb(0x27, 0x50, 0x0A))   // verde escuro
                    : new SolidColorBrush(Color.FromRgb(0x79, 0x1F, 0x1F));  // vermelho escuro

            // padrão (sem parâmetro) = tom médio, usado no valor
            return entrada
                ? new SolidColorBrush(Color.FromRgb(0x16, 0xA3, 0x4A))
                : new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}