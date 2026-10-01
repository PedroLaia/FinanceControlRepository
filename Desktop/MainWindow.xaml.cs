using FinanceControlData;
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

            var repositorio = new CategoriaRepository(context);
            var viewModel = new CategoriaViewModel(repositorio);
            DataContext = viewModel;
        }
    }
}