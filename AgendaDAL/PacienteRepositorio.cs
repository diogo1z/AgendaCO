using AgendaDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class PacienteRepositorio : Interfaces.IRepositorio<AgendaDTL.Paciente>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public PacienteRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.Paciente entity)
        {
            _inners.Clear();

            _inners.Add("@idClinicaDeCadastro", entity.IdClinicaDeCadastro);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@cpf", entity.Cpf);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@telefone", entity.Telefone);
            _inners.Add("@celular", entity.Celular);
            _inners.Add("@email", entity.Email);
            _inners.Add("@endereco", entity.Endereco);
            _inners.Add("@cep", entity.Cep);
            _inners.Add("@numero", entity.Numero);
            _inners.Add("@complemento", entity.Complemento);

            return _context.Save("s_CriarPaciente", _inners);
        }

        public IEnumerable<AgendaDTL.Paciente> Buscar(AgendaDTL.Paciente entity)
        {
            return Buscar(entity, false);
        }

        public IEnumerable<AgendaDTL.Paciente> Buscar(AgendaDTL.Paciente entity, bool comDependentes)
        {
            _inners.Clear();

            if (entity.IdClinicaDeCadastro > 0)
                _inners.Add("@idClinica", entity.IdClinicaDeCadastro);

            _inners.Add("@cpf", entity.Cpf);
            _inners.Add("@comDependentes", comDependentes);

            var pacientes = _context.Get<DataTransferencia>("s_ListarPaciente", _inners);
            if (pacientes != null)
            {
                var retorno = new List<AgendaDTL.Paciente>();
                foreach (var paciente in pacientes)
                {
                    retorno.Add(new AgendaDTL.Paciente()
                    {
                        Id = paciente.Id,
                        DataNascimento = paciente.DataNascimento,
                        Cpf = paciente.Cpf,
                        Celular = paciente.Celular,
                        Email = paciente.Email,
                        Nome = paciente.Nome,
                        Telefone = paciente.Telefone,
                        IdClinicaDeCadastro = paciente.IdClinica,
                        Cep = paciente.Cep,
                        Complemento = paciente.Complemento,
                        Endereco = paciente.Endereco,
                        Numero = paciente.Numero
                    });
                }
                return retorno;
            }
            return null;
        }

        public AgendaDTL.Paciente Obter(AgendaDTL.Paciente entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            var paciente = _context.Get<DataTransferencia>("s_ObterPaciente", _inners).FirstOrDefault(); ;
            if (paciente != null)
            {
                return new AgendaDTL.Paciente()
                {
                    Id = paciente.Id,
                    DataNascimento = paciente.DataNascimento,
                    Cpf = paciente.Cpf,
                    Celular = paciente.Celular,
                    Email = paciente.Email,
                    Nome = paciente.Nome,
                    Telefone = paciente.Telefone,
                    IdClinicaDeCadastro = paciente.IdClinica,
                    Cep = paciente.Cep,
                    Complemento = paciente.Complemento,
                    Endereco = paciente.Endereco,
                    Numero = paciente.Numero
                };
            }
            return null;
        }

        public void Atualizar(AgendaDTL.Paciente entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@idClinica", entity.IdClinicaDeCadastro);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@cpf", entity.Cpf);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@telefone", entity.Telefone);
            _inners.Add("@celular", entity.Celular);
            _inners.Add("@email", entity.Email);
            _inners.Add("@endereco", entity.Endereco);
            _inners.Add("@cep", entity.Cep);
            _inners.Add("@numero", entity.Numero);
            _inners.Add("@complemento", entity.Complemento);

            _context.Save("s_AtualizarPaciente", _inners);
        }

        public void Deletar(AgendaDTL.Paciente entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            _context.Save("s_DeletarPaciente", _inners);
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
            public int IdClinica { get; set; }
            public int IdPacienteTit { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Cep { get; set; }
            public string Cidade { get; set; }
            public int IdEstado { get; set; }
            public string Estado { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Nome { get; set; }
            public string Telefone { get; set; }
            public string Celular { get; set; }
            public string Cpf { get; set; }
            public string Email { get; set; }
        }
    }
}
