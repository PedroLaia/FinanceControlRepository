using FinanceControlCore;
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
using System.Transactions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace FinanceControlDesktop.ViewModels
{
    public class TransacaoViewModel : ViewModelBase
    {

        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        private readonly NotificacaoViewModel _notificacao;

        public ObservableCollection<Transacao> Transacoes { get; }
        public ObservableCollection<Categoria> Categorias { get; }


        public ICommand AdicionarCommand { get; }

        public ICommand EditarCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand CancelamentoCommand { get; }
        public ICommand RemoverCommand { get; }



        private string _descricao = string.Empty;
        public string Descricao { get => _descricao; set { _descricao = value; OnPropertyChanged(); } }

        private decimal _valor;
        public decimal Valor { get => _valor; set { _valor = Math.Round(value, 2, MidpointRounding.AwayFromZero); OnPropertyChanged(); } }

        private DateTime _data = DateTime.Now;
        public DateTime Data { get => _data; set { _data = value; OnPropertyChanged(); } }

        private TipoTransacao _tipoSelecionado;
        public TipoTransacao TipoSelecionado { get => _tipoSelecionado; set { _tipoSelecionado = value; OnPropertyChanged(); } }

        private Categoria? _categoriaSelecionada;
        public Categoria? CategoriaSelecionada { get => _categoriaSelecionada; set { _categoriaSelecionada = value; MensagemErro = string.Empty; OnPropertyChanged(); } }

        private string _mensagemErro = string.Empty;
        public string MensagemErro { get => _mensagemErro; set { _mensagemErro = value; OnPropertyChanged(); } }

        public Array Tipos { get; } = Enum.GetValues(typeof(TipoTransacao));

        private Transacao? _transacaoSelecionada;
        public Transacao? TransacaoSelecionada { get => _transacaoSelecionada; set { _transacaoSelecionada = value; OnPropertyChanged(); } }

        private bool _modoEdicao = false;
        public bool ModoEdicao { get => _modoEdicao; set { _modoEdicao = value; OnPropertyChanged(); } }

        
        ////////////HISTORICO
        
        public ICommand FiltrarCommand { get; }
        public ICommand AlternarFiltrosCommand { get; }
        
        public ICommand LimparFiltrosCommand { get; }

        private DateTime? _dataInicio;
        public DateTime? DataInicio { get => _dataInicio; set { _dataInicio = value; OnPropertyChanged(); } }

        private DateTime? _dataFim;
        public DateTime? DataFim { get => _dataFim; set { _dataFim = value; OnPropertyChanged(); } }

        private TipoTransacao? _tipoFiltro;
        public TipoTransacao? TipoFiltro { get => _tipoFiltro; set { _tipoFiltro = value; OnPropertyChanged(); } }

        private Categoria? _categoriaFiltro;
        public Categoria? CategoriaFiltro { get => _categoriaFiltro; set { _categoriaFiltro = value; OnPropertyChanged(); } }

        private bool _filtrosVisiveis = false;
        public bool FiltrosVisiveis { get => _filtrosVisiveis; set { _filtrosVisiveis = value; OnPropertyChanged(); } }

        ////////////HISTORICO



        public TransacaoViewModel(ITransacaoRepository transacao, ICategoriaRepository categoria, NotificacaoViewModel notificacao)
        {
            _transacaoRepository = transacao;
            _categoriaRepository = categoria;
            _notificacao = notificacao;
            Transacoes = new ObservableCollection<Transacao>(_transacaoRepository.ObterTodas());
            Categorias = new ObservableCollection<Categoria>(_categoriaRepository.ObterTodas());
            AdicionarCommand = new RelayCommand(Adicionar);
            EditarCommand = new RelayCommand(Editar);
            SalvarCommand = new RelayCommand(Salvar);
            CancelamentoCommand = new RelayCommand(Cancelamento);
            RemoverCommand = new RelayCommand(Remover);
            FiltrarCommand = new RelayCommand(Filtrar);
            AlternarFiltrosCommand = new RelayCommand(AlternarFiltros);
            LimparFiltrosCommand = new RelayCommand(LimparFiltros);

        }

        public void Adicionar()
        {
            Transacao nova = new Transacao { Descricao = Descricao, Valor = Valor, Data = Data, Tipo = TipoSelecionado, Categoria = CategoriaSelecionada };

            TransacaoValidator validator = new TransacaoValidator();
            ResultadoValidacao resultado = validator.Validar(nova);
            if (!resultado.Valido)
            {
                _notificacao.Mostrar("Erro de Validação", string.Join("\n", resultado.Erros));
                return;
            }

            MensagemErro = String.Empty;

            _transacaoRepository.Adicionar(nova);
            Transacoes.Add(nova);
            Descricao = string.Empty;
            Valor = 0;
            Data = DateTime.Now;
            CategoriaSelecionada = null;
        }

        public void CarregarCategorias()
        {
            Categorias.Clear();
            foreach (var categoria in _categoriaRepository.ObterTodas())
            {
                Categorias.Add(categoria);
            }
        }

        public void Editar()
        {
            Descricao = TransacaoSelecionada.Descricao;
            Valor = TransacaoSelecionada.Valor;
            Data = TransacaoSelecionada.Data;
            TipoSelecionado = TransacaoSelecionada.Tipo;
            CategoriaSelecionada = TransacaoSelecionada.Categoria;
            ModoEdicao = true;
        }

        public void Salvar()
        {
            TransacaoSelecionada.Descricao = Descricao;
            TransacaoSelecionada.Valor = Valor;
            TransacaoSelecionada.Data = Data;
            TransacaoSelecionada.Tipo = TipoSelecionado;
            TransacaoSelecionada.Categoria = CategoriaSelecionada;

            TransacaoValidator validator = new TransacaoValidator();
            ResultadoValidacao resultado = validator.Validar(TransacaoSelecionada);
            if (!resultado.Valido)
            {
                _notificacao.Mostrar("Erro de Validação", string.Join("\n", resultado.Erros));
                return;
            }

            MensagemErro = String.Empty;

            _transacaoRepository.Atualizar(TransacaoSelecionada);


            Transacoes.Clear();
            foreach (var transacao in _transacaoRepository.ObterTodas())
            {
                Transacoes.Add(transacao);
            }

            Descricao = string.Empty;
            Valor = 0;
            Data = DateTime.Now;
            TipoSelecionado = TipoTransacao.Entrada;
            CategoriaSelecionada = null;
            ModoEdicao = false;
        }

        public void Cancelamento()
        {
            Descricao = string.Empty;
            Valor = 0;
            Data = DateTime.Now;
            TipoSelecionado = TipoTransacao.Entrada;
            CategoriaSelecionada = null;
            ModoEdicao = false;
        }


        public void Remover()
        {
            if (TransacaoSelecionada == null)
                return;

            var resposta = MessageBox.Show(
                "Deseja excluir esta transação?",
                "Confirmar exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resposta != MessageBoxResult.Yes)
                return;

            _transacaoRepository.Remover(TransacaoSelecionada.Id);
            Transacoes.Remove(TransacaoSelecionada);
        }


        public void Filtrar()
        {
            int? categoriaId = CategoriaFiltro?.Id;
            List<Transacao> resultado = _transacaoRepository.ObterPorFiltro(DataInicio, DataFim, TipoFiltro, categoriaId);
            Transacoes.Clear();
            foreach (var transacao in resultado)
            {
                Transacoes.Add(transacao);
            }
        }

        private void AlternarFiltros()
        {
            FiltrosVisiveis = !FiltrosVisiveis;
        }

        public void LimparFiltros()
        {
            DataInicio = null;
            DataFim = null;
            TipoFiltro = null;
            CategoriaFiltro = null;
            Filtrar();
        }
    }
}
