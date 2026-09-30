using FinanceControlCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlData
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly FinanceDbContext _dbcontext;
        public CategoriaRepository(FinanceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Adicionar(Categoria categoria)
        {
            _dbcontext.Categorias.Add(categoria);
            _dbcontext.SaveChanges();
        }

        public void Remover(int id)
        {
            Categoria categoria = _dbcontext.Categorias.Find(id);
            if(categoria == null)
            {
                return;
            }
            _dbcontext.Categorias.Remove(categoria);
            _dbcontext.SaveChanges();
        }

        public List<Categoria> ObterTodas()
        {
            List<Categoria> categorias = _dbcontext.Categorias.ToList();
            return categorias;
        }



    }

}
