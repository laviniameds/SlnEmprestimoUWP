using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmprestimoUWP.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int IdContato { get; set; }
        public string Descricao { get; set; }
        public Contato Contato { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevDev { get; set; }
        public bool Devolvido { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}
