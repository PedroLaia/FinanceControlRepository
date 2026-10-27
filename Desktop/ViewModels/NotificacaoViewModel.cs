using FinanceControlDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace FinanceControlDesktop.ViewModels
{
    public class NotificacaoViewModel : ViewModelBase
    {
        private string _titulo = string.Empty;
        public string Titulo { get => _titulo; set { _titulo = value; OnPropertyChanged(); } }

        private string _mensagem = string.Empty;
        public string Mensagem { get => _mensagem; set { _mensagem = value; OnPropertyChanged(); } }

        private bool _visivel = false;
        public bool Visivel { get => _visivel; set { _visivel = value; OnPropertyChanged(); } }

        private readonly DispatcherTimer _timer;
        public ICommand EsconderCommand { get; }

        public NotificacaoViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += AoFinalizar;
            EsconderCommand = new RelayCommand(Esconder);
        }



        private void AoFinalizar(object? sender, EventArgs e)
        {
            Visivel = false;
            _timer.Stop();
        }

        public void Mostrar(string titulo, string mensagem)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            Visivel = true;
            _timer.Stop();
            _timer.Start();
        }

        private void Esconder()
        {
            Visivel = false;
            _timer.Stop();
        }
    }
}
