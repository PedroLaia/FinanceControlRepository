using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlData
{
    public interface ITransacaoRepository
    {
        void Adicionar(Transacao transacao);

        void Atualizar(Transacao transacao);

        void Remover(int id);

        List<Transacao> ObterTodas();

        Transacao ObterPorId(int id);

        List<Transacao> ObterPorFiltro(DateTime? inicio, DateTime? fim, TipoTransacao? tipo, int? categoriaId);



    }
}
