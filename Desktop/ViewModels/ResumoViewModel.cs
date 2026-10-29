using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using FinanceControlCore.Services;
using FinanceControlData.Repositories;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FinanceControlDesktop.ViewModels
{
    public class ResumoViewModel : ViewModelBase
    {

        private readonly ITransacaoRepository _transacaoRepository;

        private ResumoService _resumoService = new ResumoService();

        private decimal _totalEntradas;
        public decimal TotalEntradas { get => _totalEntradas; set { _totalEntradas = value; OnPropertyChanged(); } }

        private decimal _totalSaidas;
        public decimal TotalSaidas { get => _totalSaidas; set { _totalSaidas = value; OnPropertyChanged(); } }

        private decimal _saldo;
        public decimal Saldo { get => _saldo; set { _saldo = value; OnPropertyChanged(); } }


        public ObservableCollection<KeyValuePair<string, decimal>> ExtratoPorCategoria { get; }


        private TipoFiltroResumo _tipoFiltroSelecionado = TipoFiltroResumo.Todos;
        public TipoFiltroResumo TipoFiltroSelecionado { get => _tipoFiltroSelecionado; set { _tipoFiltroSelecionado = value; OnPropertyChanged(); CalcularResumo(); } }

        private PeriodoResumo _periodoSelecionado = PeriodoResumo.Geral;
        public PeriodoResumo PeriodoSelecionado { get => _periodoSelecionado; set { _periodoSelecionado = value; OnPropertyChanged(); CalcularResumo(); } }

        public Array TiposFiltro { get; } = Enum.GetValues(typeof(TipoFiltroResumo));
        public Array Periodos { get; } = Enum.GetValues(typeof(PeriodoResumo));

        private ISeries[] _entradasSaidasSeries;
        public ISeries[] EntradasSaidasSeries { get => _entradasSaidasSeries; set { _entradasSaidasSeries = value; OnPropertyChanged(); } }

        private ISeries[] _gastosPorCategoriaSeries;
        public ISeries[] GastosPorCategoriaSeries { get => _gastosPorCategoriaSeries; set { _gastosPorCategoriaSeries = value; OnPropertyChanged(); } }

        public Axis[] EixoX { get; } = { new Axis { IsVisible = false } };

        public ResumoViewModel(ITransacaoRepository transacao)
        {
            _transacaoRepository = transacao;
            ExtratoPorCategoria = new ObservableCollection<KeyValuePair<string, decimal>>();
            CalcularResumo();


        }

        public void CalcularResumo()
        {
            IEnumerable<Transacao> transacoes = _transacaoRepository.ObterTodas();
            switch (PeriodoSelecionado)
            {
                case PeriodoResumo.MesAtual:
                    transacoes = transacoes.Where(t => t.Data.Month == DateTime.Now.Month && t.Data.Year == DateTime.Now.Year);
                    break;

                case PeriodoResumo.MesAnterior:
                    var mesAnterior = DateTime.Now.AddMonths(-1);
                    transacoes = transacoes.Where(t => t.Data.Month == mesAnterior.Month && t.Data.Year == mesAnterior.Year);
                    break;

                default:

                    break;
            }

            TotalEntradas = _resumoService.TotalEntradas(transacoes);
            TotalSaidas = _resumoService.TotalSaidas(transacoes);
            Saldo = _resumoService.CalcularSaldo(transacoes);



            Dictionary<string, decimal> extrato;

            switch (TipoFiltroSelecionado)
            {
                case TipoFiltroResumo.Entradas:
                    extrato = _resumoService.EntradasPorCategoria(transacoes);
                    break;
                case TipoFiltroResumo.Saidas:
                    extrato = _resumoService.SaidasPorCategoria(transacoes);
                    break;

                default:
                    extrato = _resumoService.SaldoPorCategoria(transacoes);
                    break;
            }


            ExtratoPorCategoria.Clear();




            foreach (var item in extrato)
            {
                ExtratoPorCategoria.Add(item);
            }


            // Barras: duas séries coloridas (verde Entradas / vermelho Saídas)
            EntradasSaidasSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Entradas",
                    Values = new double[] { (double)TotalEntradas },
                    Fill = new SolidColorPaint(SKColor.Parse("#16A34A"))
                },
                new ColumnSeries<double>
                {
                    Name = "Saídas",
                    Values = new double[] { (double)TotalSaidas },
                    Fill = new SolidColorPaint(SKColor.Parse("#DC2626"))
                }
            };

            /// só categorias com saída (> 0)
            var saidas = _resumoService.SaidasPorCategoria(transacoes)
                .Where(x => x.Value > 0)
                .ToList();

            List<ISeries> fatias = new List<ISeries>();
            for (int i = 0; i < saidas.Count; i++)
            {
                var item = saidas[i];

                // tom espalhado pela roda de cores -> cada fatia uma cor única
                float hue = (float)(i * 360.0 / saidas.Count);

                fatias.Add(new PieSeries<double>
                {
                    Values = new double[] { (double)item.Value },
                    Name = item.Key,
                    Fill = new SolidColorPaint(SKColor.FromHsl(hue, 60, 55)),
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsFormatter = point => point.StackedValue.Share.ToString("P0")
                });
            }
            GastosPorCategoriaSeries = fatias.ToArray();


        }




    }
}
