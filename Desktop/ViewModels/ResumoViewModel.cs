using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using FinanceControlCore.Services;
using FinanceControlData.Repositories;
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
        }
    }
}
