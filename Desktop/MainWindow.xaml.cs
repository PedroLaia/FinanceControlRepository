using FinanceControlData;
using FinanceControlData.Repositories;
using FinanceControlDesktop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceControlDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var context = new FinanceDbContext();
            context.Database.Migrate();

            var categoriaRepository = new CategoriaRepository(context);
            var transacaoRepository = new TransacaoRepository(context);

            var categoriaVM = new CategoriaViewModel(categoriaRepository);
            var transacaoVM = new TransacaoViewModel(transacaoRepository, categoriaRepository);
            var historicoVM = new HistoricoViewModel(transacaoRepository, categoriaRepository);
            var resumoVM = new ResumoViewModel(transacaoRepository);
            var mainVM = new MainViewModel(categoriaVM, transacaoVM, historicoVM, resumoVM);

            DataContext = mainVM;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (DataContext is MainViewModel main)
                {
                    main.TransacaoVM.CarregarCategorias();
                    main.ResumoVM.CalcularResumo();
                }
            }
        }
    }
}