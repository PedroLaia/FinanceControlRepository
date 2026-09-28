using FinanceControlCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlCore
{
    public class TransacaoValidator
    {
        public ResultadoValidacao Validar(Transacao transacao)
        {
            var resultado = new ResultadoValidacao();

            if (transacao.Valor <= 0)
            {
                resultado.Erros.Add("Valor deve ser maior que zero!");
            }
            if (string.IsNullOrWhiteSpace(transacao.Descricao))
            {
                resultado.Erros.Add("Descrição é obrigatória!");
            }

            if (transacao.Categoria == null)
            {
                resultado.Erros.Add("Categória é obrigatória");
            }

            if (resultado.Valido = resultado.Erros.Count == 0)
            {
                resultado.Valido = true;
            }
            return resultado;


        }
    }
}
