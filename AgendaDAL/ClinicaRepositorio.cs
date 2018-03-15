using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaDTL;
using AgendaDb.Interfaces;

namespace AgendaDAL
{
    public class ClinicaRepositorio : Interfaces.IRepositorio<AgendaDTL.Clinica>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public ClinicaRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }
        public void Atualizar(Clinica entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@nomeFantasia", entity.NomeFantasia);
            _inners.Add("@razaoSocial", entity.RazaoSocial);
            _inners.Add("@cnpj", entity.Cnpj);
            _inners.Add("@email", entity.Email);
            _inners.Add("@idBairro", entity.Endereco.IdBairro == 0 ? null : (object)entity.Endereco.IdBairro);
            _inners.Add("@logradouro", entity.Endereco.Logradouro);
            _inners.Add("@numeroLogradouro", entity.Endereco.NumeroLogradouro);
            _inners.Add("@complementoLogradouro", entity.Endereco.ComplementoLogradouro);
            _inners.Add("@cepLogradouro", entity.Endereco.CepLogradouro);
            _context.Save("s_AlterarClinica", _inners);

        }

        public IEnumerable<Clinica> Buscar(Clinica entity)
        {
            _inners.Clear();

            _inners.Add("@idCidade", entity.Endereco.IdCidade);

            var result = _context.Get<DataTransferencia>("s_ListarClinica", _inners);

            if (result != null)
            {
                var listaClinica = new List<Clinica>();
                foreach (var r in result)
                {
                    listaClinica.Add(new Clinica()
                    {
                        Id = r.Id,
                        NomeFantasia = r.NomeFantasia,
                        RazaoSocial = r.RazaoSocial,
                        Cnpj = r.Cnpj,
                        Email = r.Email,
                        Endereco = new Endereco()
                        {
                            Bairro = r.Bairro,
                            CepLogradouro = r.CepLogradouro,
                            Cidade = r.Cidade,
                            ComplementoLogradouro = r.ComplementoLogradouro,
                            Estado = r.Estado,
                            IdBairro = r.IdBairro,
                            IdCidade = r.IdCidade,
                            IdEstado = r.IdEstado,
                            Logradouro = r.Logradouro,
                            NumeroLogradouro = r.NumeroLogradouro
                        }
                    });
                }
                return listaClinica;
            }
            else return null;

        }

        public int Criar(Clinica entity)
        {
            _inners.Clear();

            _inners.Add("@nomeFantasia", entity.NomeFantasia);
            _inners.Add("@razaoSocial", entity.RazaoSocial);
            _inners.Add("@cnpj", entity.Cnpj);
            _inners.Add("@email", entity.Email);
            _inners.Add("@idBairro", entity.Endereco.IdBairro == 0 ? null : (object)entity.Endereco.IdBairro);            
            _inners.Add("@uf", entity.Endereco.Estado);            
            _inners.Add("@cidade", entity.Endereco.Cidade);
            _inners.Add("@bairro", entity.Endereco.Bairro);
            _inners.Add("@logradouro", entity.Endereco.Logradouro);
            _inners.Add("@numeroLogradouro", entity.Endereco.NumeroLogradouro);
            _inners.Add("@complementoLogradouro", entity.Endereco.ComplementoLogradouro);
            _inners.Add("@cepLogradouro", entity.Endereco.CepLogradouro);
            return _context.Save("s_CriarClinica", _inners);
        }

        public void Deletar(Clinica entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            _context.Save("s_DeletarClinica", _inners);
        }

        public Clinica Obter(Clinica entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            var result = _context.Get<DataTransferencia>("s_ObterClinica", _inners).FirstOrDefault();

            if (result != null)
                return new Clinica()
                {
                    Id = result.Id,
                    NomeFantasia = result.NomeFantasia,
                    RazaoSocial = result.RazaoSocial,
                    Cnpj = result.Cnpj,
                    Email = result.Email,
                    Endereco = new Endereco()
                    {
                        Bairro = result.Bairro,
                        CepLogradouro = result.CepLogradouro,
                        Cidade = result.Cidade,
                        ComplementoLogradouro = result.ComplementoLogradouro,
                        Estado = result.Estado,
                        IdBairro = result.IdBairro,
                        IdCidade = result.IdCidade,
                        IdEstado = result.IdEstado,
                        Logradouro = result.Logradouro,
                        NumeroLogradouro = result.NumeroLogradouro
                    }
                };
            else return null;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class DataTransferencia
        {
            public int Id { get; set; }
            public string NomeFantasia { get; set; }
            public string RazaoSocial { get; set; }
            public string Cnpj { get; set; }
            public string Email { get; set; }
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
    }
}
