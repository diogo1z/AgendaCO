using AgendaDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class DependenteRepositorio : Interfaces.IRepositorio<AgendaDTL.DependentePaciente>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public DependenteRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.DependentePaciente entity)
        {
            _inners.Clear();

            _inners.Add("@@idPacienteTit", entity.IdPaciente);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@cpf", entity.Cpf);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@telefone", entity.Telefone);
            _inners.Add("@celular", entity.Celular);
            _inners.Add("@email", entity.Email);

            return _context.Save("s_CriarPaciente", _inners);
        }

        public IEnumerable<AgendaDTL.DependentePaciente> Buscar(AgendaDTL.DependentePaciente entity)
        {
            _inners.Clear();

            _inners.Add("@idPacienteTit", entity.IdPaciente);
            
            var dependentes = _context.Get<DataTransferencia>("s_ListarPaciente", _inners);
            if (dependentes != null)
            {
                var retorno = new List<AgendaDTL.DependentePaciente>();
                foreach (var dependente in dependentes)
                {
                    retorno.Add(new AgendaDTL.DependentePaciente()
                    {
                        Id = dependente.Id,
                        DataNascimento = dependente.DataNascimento,
                        Cpf = dependente.Cpf,
                        Celular = dependente.Celular,
                        Email = dependente.Email,
                        Nome = dependente.Nome,
                        Telefone = dependente.Telefone,
                        IdPaciente = dependente.IdPacienteTit
                    });
                }
                return retorno;
            }
            return null;
        }

        public AgendaDTL.DependentePaciente Obter(AgendaDTL.DependentePaciente entity)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(AgendaDTL.DependentePaciente entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@cpf", entity.Cpf);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@telefone", entity.Telefone);
            _inners.Add("@celular", entity.Celular);
            _inners.Add("@email", entity.Email);
            _inners.Add("@idPacienteTit", entity.IdPaciente);

            _context.Save("s_AtualizarPaciente", _inners);
        }

        public void Deletar(AgendaDTL.DependentePaciente entity)
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
