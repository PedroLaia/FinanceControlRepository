using System;
using System.Globalization;
using System.Windows.Data;
using FinanceControlCore.Enums;

namespace FinanceControlDesktop.Converters
{
    public class EnumParaTextoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                PeriodoResumo.MesAtual => "Mês Atual",
                PeriodoResumo.MesAnterior => "Mês Anterior",
                PeriodoResumo.Geral => "Geral",

                TipoFiltroResumo.Todos => "Todos",
                TipoFiltroResumo.Entradas => "Entradas",
                TipoFiltroResumo.Saidas => "Saídas",

                TipoTransacao.Entrada => "Entrada",
                TipoTransacao.Saida => "Saída",

                _ => value?.ToString() ?? string.Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}