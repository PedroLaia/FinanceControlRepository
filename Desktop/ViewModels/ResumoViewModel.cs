using FinanceControlCore.Services;
using FinanceControlData.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ResumoViewModel(ITransacaoRepository transacao)
        {
            _transacaoRepository = transacao;
            ExtratoPorCategoria = new ObservableCollection<KeyValuePair<string, decimal>>();
            CalcularResumo();

        }

        public void CalcularResumo()
        {
            var transacoes = _transacaoRepository.ObterTodas();
            TotalEntradas = _resumoService.TotalEntradas(transacoes);
            TotalSaidas = _resumoService.TotalSaidas(transacoes);
            Saldo = _resumoService.CalcularSaldo(transacoes);
            ExtratoPorCategoria.Clear();
            foreach(var item in _resumoService.SaldoPorCategoria(transacoes))
            {
                ExtratoPorCategoria.Add(item);
            }
        }
    }
}
