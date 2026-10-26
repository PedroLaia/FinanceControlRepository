using FinanceControlCore.Entities;
using FinanceControlData.Repositories;
using FinanceControlDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace FinanceControlDesktop.ViewModels
{
    public class CategoriaViewModel : ViewModelBase
    {

        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public ObservableCollection<Categoria> Categorias { get; }

        public ICommand AdicionarCommand { get; }
        public ICommand RemoverCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand SalvarCommand { get; }
        
        private string _nome = string.Empty;

        public string Nome { get => _nome; set { _nome = value; OnPropertyChanged(); } }

        private Categoria? _categoriaSelecionada;
        public Categoria? CategoriaSelecionada { get => _categoriaSelecionada; set { _categoriaSelecionada = value; MensagemExcluir = string.Empty;ModoEdicao = false; OnPropertyChanged(); } }

        private string _mensagemExcluir = string.Empty;
        public string MensagemExcluir { get => _mensagemExcluir; set { _mensagemExcluir = value; OnPropertyChanged(); } }

        private string _nomeEdicao = string.Empty;
        public string NomeEdicao { get => _nomeEdicao; set { _nomeEdicao = value; OnPropertyChanged(); } }

        private bool _modoEdicao = false;
        public bool ModoEdicao { get => _modoEdicao; set { _modoEdicao = value; OnPropertyChanged(); } }


        public CategoriaViewModel(ICategoriaRepository categoria, ITransacaoRepository transacao)
        {
            _categoriaRepository = categoria;
            _transacaoRepository = transacao;
            Categorias = new ObservableCollection<Categoria>(_categoriaRepository.ObterTodas());
            AdicionarCommand = new RelayCommand(Adicionar, PodeAdicionar);
            RemoverCommand = new RelayCommand(Remover, PodeRemover);
            EditarCommand = new RelayCommand(Editar);
            SalvarCommand = new RelayCommand(Salvar);

        }

        public void Adicionar()
        {
            Categoria nova = new Categoria { Nome = Nome };
            _categoriaRepository.Adicionar(nova);
            Categorias.Add(nova);
            Nome = string.Empty;

        }

        public void Remover()
        {
            if (CategoriaSelecionada == null)
            {
                return;
            }
            if (_transacaoRepository.ExisteCategoria(CategoriaSelecionada.Id))
            {
                MensagemExcluir = "A Categoria tem Transações, Transfira as Transações ou Remova antes de Excluir a Categoria.";
                return;
            }
            _categoriaRepository.Remover(CategoriaSelecionada.Id);
            Categorias.Remove(CategoriaSelecionada);
            MensagemExcluir = string.Empty;
            ModoEdicao = false;

        }

        public void Editar()
        {
            NomeEdicao = CategoriaSelecionada.Nome;
            ModoEdicao = true;
        }

        public bool PodeRemover()
        {
            return CategoriaSelecionada != null;
        }

        public bool PodeAdicionar()
        {
            return !string.IsNullOrWhiteSpace(Nome);
        }

        public void Salvar()
        {
            CategoriaSelecionada.Nome = NomeEdicao;
            _categoriaRepository.Atualizar(CategoriaSelecionada);
            Categorias.Clear();
            foreach (var categoria in _categoriaRepository.ObterTodas())
            {
                Categorias.Add(categoria);
            }
            ModoEdicao = false;
        }
    }
}
