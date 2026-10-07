using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlDesktop.ViewModels
{
    public class MainViewModel
    {
        public CategoriaViewModel CategoriaVM { get; }
        public TransacaoViewModel TransacaoVM { get; }
        public HistoricoViewModel HistoricoVM { get; }
        public ResumoViewModel ResumoVM { get; }

        public MainViewModel(CategoriaViewModel categoriaVM, TransacaoViewModel transacaoVM, HistoricoViewModel historicoVM, ResumoViewModel resumoVM)
        {
            CategoriaVM = categoriaVM;
            TransacaoVM = transacaoVM;
            HistoricoVM = historicoVM;
            ResumoVM = resumoVM;
        }


    }
}
