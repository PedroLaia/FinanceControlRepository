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
                string pastaAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string pastaApp = Path.Combine(pastaAppData, "FinanceControl");

                Directory.CreateDirectory(pastaApp);

                string caminhoDB = Path.Combine(pastaApp, "FinanceControlDB.db");

                optionsBuilder.UseSqlite($"Data Source={caminhoDB}");
            }
        }
    }
}