using FinanceControlCore.Entities;
using FinanceControlCore.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlData.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly FinanceDbContext _dbcontext;

        public TransacaoRepository(FinanceDbContext dbcontext)
        {

            _dbcontext = dbcontext;
        }

        public void Adicionar(Transacao transacao)
        {
            _dbcontext.Transacoes.Add(transacao);
            _dbcontext.SaveChanges();
        }

        public void Atualizar(Transacao transacao)
        {
            _dbcontext.Transacoes.Update(transacao);
            _dbcontext.SaveChanges();
        }

        public List<Transacao> ObterPorFiltro(DateTime? inicio, DateTime? fim, TipoTransacao? tipo, int? categoriaId)
        {
            List<Transacao> transacoes;
            IQueryable<Transacao> consulta = _dbcontext.Transacoes.Include(t=> t.Categoria);
            if (categoriaId != null)
            {
                consulta = consulta.Where(t => t.CategoriaId == categoriaId);
            }
            if (tipo != null)
            {
                consulta = consulta.Where(t => t.Tipo == tipo);
            }
            if (inicio != null)
            {
                consulta = consulta.Where(t => t.Data >= inicio);
                
            }
            if (fim != null)
            {
                consulta = consulta.Where(t => t.Data <= fim);

            }


            return transacoes = consulta.ToList();

        }

        public Transacao ObterPorId(int id)
        {
            Transacao transacao = _dbcontext.Transacoes.Find(id);
            if (transacao == null)
            {
                return null;
            }
            return transacao;
        }

        public List<Transacao> ObterTodas()
        {
            List<Transacao> transacoes = _dbcontext.Transacoes.Include(t=> t.Categoria).ToList();
            return transacoes;

        }

        public void Remover(int id)
        {
            Transacao transacao = _dbcontext.Transacoes.Find(id);
            if (transacao == null)
            {
                return;
            }

            _dbcontext.Transacoes.Remove(transacao);
            _dbcontext.SaveChanges();

        }
    }
}
