using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceControlDesktop.Views
{
    /// <summary>
    /// Interaction logic for TransacaoView.xaml
    /// </summary>
    public partial class TransacaoView : UserControl
    {
        public TransacaoView()
        {
            InitializeComponent();
        }
        private void Valor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string textoFinal = textBox.Text
                .Remove(textBox.SelectionStart, textBox.SelectionLength)
                .Insert(textBox.SelectionStart, e.Text);

            e.Handled = !Regex.IsMatch(textoFinal, @"^\d*([.,]\d{0,2})?$");
        }
    }
}
