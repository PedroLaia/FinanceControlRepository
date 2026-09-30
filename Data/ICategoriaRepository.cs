using FinanceControlCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlData
{
    public interface ICategoriaRepository
    {
        void Adicionar(Categoria categoria);
        void Remover(int id);
        List<Categoria> ObterTodas();

    }
}
