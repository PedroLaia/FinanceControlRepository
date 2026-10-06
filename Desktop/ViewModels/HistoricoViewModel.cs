using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using FinanceControlData.Repositories;
using FinanceControlDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinanceControlDesktop.ViewModels
{
    public class HistoricoViewModel : ViewModelBase
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ObservableCollection<Transacao> Transacoes { get; }
        public ObservableCollection<Categoria> Categorias { get; }

        public ICommand FiltrarCommand { get; }

        private DateTime? _dataInicio;
        public DateTime? DataInicio { get => _dataInicio; set { _dataInicio = value; OnPropertyChanged(); } }

        private DateTime? _dataFim;
        public DateTime? DataFim { get => _dataFim; set { _dataFim = value; OnPropertyChanged(); } }

        private TipoTransacao? _tipoFiltro;
        public TipoTransacao? TipoFiltro { get => _tipoFiltro; set { _tipoFiltro = value; OnPropertyChanged(); } }

        private Categoria? _categoriaFiltro;
        public Categoria? CategoriaFiltro { get => _categoriaFiltro; set { _categoriaFiltro = value; OnPropertyChanged(); } }

        public Array Tipos { get; } = Enum.GetValues(typeof(TipoTransacao));

        public HistoricoViewModel(ITransacaoRepository transacao, ICategoriaRepository categoria)
        {
            _transacaoRepository = transacao;
            _categoriaRepository = categoria;
            Transacoes = new ObservableCollection<Transacao>(_transacaoRepository.ObterTodas());
            Categorias = new ObservableCollection<Categoria>(_categoriaRepository.ObterTodas());
            FiltrarCommand = new RelayCommand(Filtrar);

        }

        public void Filtrar()
        {
            int? categoriaId = CategoriaFiltro?.Id;
            List<Transacao> resultado = _transacaoRepository.ObterPorFiltro(DataInicio, DataFim, TipoFiltro, categoriaId);
            Transacoes.Clear();
            foreach(var transacao in resultado)
            {
                Transacoes.Add(transacao);
            }
        }
    }
}
