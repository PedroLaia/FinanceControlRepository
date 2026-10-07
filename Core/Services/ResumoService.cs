using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlCore.Services
{
    public class ResumoService
    {

        public decimal TotalEntradas(IEnumerable<Transacao> transacoes)
        {
            return transacoes
                .Where(t => t.Tipo == TipoTransacao.Entrada)
                .Sum(t => t.Valor);
        }
        public decimal TotalSaidas(IEnumerable<Transacao> transacoes)
        {
            return transacoes
                .Where(t => t.Tipo == TipoTransacao.Saida)
                .Sum(t => t.Valor);
        }
        public decimal CalcularSaldo(IEnumerable<Transacao> transacoes)
        {
            var totalEntradas = TotalEntradas (transacoes);
            var totalSaidas = TotalSaidas(transacoes);
            return totalEntradas - totalSaidas;
        }

        public Dictionary<string, decimal> TotalPorCategoria(IEnumerable<Transacao> transacoes)
        {
            return transacoes
                .GroupBy(c => c.Categoria.Nome)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(c=> c.Valor));
        }

        public Dictionary<string, decimal> SaldoPorCategoria(IEnumerable<Transacao> transacoes)
        {
            return transacoes
                .GroupBy(c => c.Categoria.Nome)
                .ToDictionary(
                    g => g.Key,
                    g => g.Where(c => c.Tipo == TipoTransacao.Entrada).Sum(v => v.Valor) - g.Where(a => a.Tipo == TipoTransacao.Saida).Sum(v => v.Valor));
                
        }

    }
}
