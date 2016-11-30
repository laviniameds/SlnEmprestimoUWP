using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoUWP.Models
{
    class AppEmprestimo:DbContext
    {
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=AppEmprestimo.db");
        }
    }
}
