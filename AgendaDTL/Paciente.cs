using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Paciente : Pessoa
    {
        public int Id { get; set; }
        public int IdClinicaDeCadastro { get; set; }
        public List<Pessoa> Dependentes { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class DependentePaciente : Pessoa
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
    }

    public class Pessoa
    {
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}
