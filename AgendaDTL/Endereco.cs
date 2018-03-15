using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Endereco
    {
        public string CepLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string NumeroLogradouro { get; set; }
        public string ComplementoLogradouro { get; set; }
        public string Bairro { get; set; }
        public long IdBairro { get; set; }
        public int IdCidade { get; set; }
        public string Cidade { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
    }

    public class Cidade
    {
        public Cidade()
        {

        }
        public Cidade(string nome, int id)
        {
            Id = id;
            Nome = nome;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class Estado
    {
        public Estado()
        {

        }
        public Estado(string nome, int id)
        {
            Id = id;
            Nome = nome;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
