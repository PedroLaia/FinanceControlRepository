using FinanceControlCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlData
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet <Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=FinanceControlDB.db");
            }
        }
    }
}