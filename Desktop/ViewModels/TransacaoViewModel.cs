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
using System.Windows.Input;

namespace FinanceControlDesktop.ViewModels
{
    public class TransacaoViewModel : ViewModelBase
    {

        private readonly ITransacaoRepository _transacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ObservableCollection<Transacao> Transacoes { get; }
        public ObservableCollection<Categoria> Categorias { get; }


        public ICommand AdicionarCommand { get; }

        public ICommand EditarCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand CancelamentoCommand { get; }


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



        public TransacaoViewModel(ITransacaoRepository transacao, ICategoriaRepository categoria)
        {
            _transacaoRepository = transacao;
            _categoriaRepository = categoria;
            Transacoes = new ObservableCollection<Transacao>(_transacaoRepository.ObterTodas());
            Categorias = new ObservableCollection<Categoria>(_categoriaRepository.ObterTodas());
            AdicionarCommand = new RelayCommand(Adicionar);
            EditarCommand = new RelayCommand(Editar);
            SalvarCommand = new RelayCommand(Salvar);
                        
            CancelamentoCommand = new RelayCommand(Cancelamento);


        }

        public void Adicionar()
        {
            Transacao nova = new Transacao { Descricao = Descricao, Valor = Valor, Data = Data, Tipo = TipoSelecionado, Categoria = CategoriaSelecionada };

            TransacaoValidator validator = new TransacaoValidator();
            ResultadoValidacao resultado = validator.Validar(nova);
            if (!resultado.Valido)
            {
                MensagemErro = string.Join("\n", resultado.Erros);
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
                MensagemErro = string.Join("\n", resultado.Erros);
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
    }
}
