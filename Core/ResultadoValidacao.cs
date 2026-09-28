using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlCore
{
    public class ResultadoValidacao
    {
        public bool Valido { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
    }
}
