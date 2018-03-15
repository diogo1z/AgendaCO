using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Odontologista
    {
        public int Id { get; set; }

        public int IdClinica { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Cro { get; set; }

        public int CroEstado { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Email { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Cep { get; set; }
    }
}
