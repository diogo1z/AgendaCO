using AgendaDb.Interfaces;
using AgendaDTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class OdontologistaRepositorio : Interfaces.IRepositorio<AgendaDTL.Odontologista>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public OdontologistaRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.Odontologista entity)
        {
            _inners.Clear();

            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@croEstado", entity.CroEstado);
            _inners.Add("@cro", entity.Cro);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@email", entity.Email);
            _inners.Add("@endereco", entity.Endereco);
            _inners.Add("@cep", entity.Cep);
            _inners.Add("@numero", entity.Numero);
            _inners.Add("@complemento", entity.Complemento);
            _inners.Add("@cpf", entity.Cpf);

            return _context.Save("s_CriarOdontologista", _inners);            
        }
        
        public IEnumerable<AgendaDTL.Odontologista> Buscar(int idClinica)
        {
            _inners.Clear();

            _inners.Add("@idClinica", idClinica);

            return _context.Get<Odontologista>("s_ListarOdontologista", _inners);
        }

        public void Atualizar(AgendaDTL.Odontologista entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@nome", entity.Nome);
            _inners.Add("@croEstado", entity.CroEstado);
            _inners.Add("@cro", entity.Cro);
            _inners.Add("@dataNascimento", entity.DataNascimento);
            _inners.Add("@email", entity.Email);
            _inners.Add("@endereco", entity.Endereco);
            _inners.Add("@cep", entity.Cep);
            _inners.Add("@numero", entity.Numero);
            _inners.Add("@complemento", entity.Complemento);
            _inners.Add("@cpf", entity.Cpf);

            _context.Save("s_AtualizarOdontologista", _inners);
        }

        public void Deletar(Odontologista entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@idClinica", entity.IdClinica);

            _context.Save("s_DeletarOdontologista", _inners);
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

        public IEnumerable<Odontologista> Buscar(Odontologista entity)
        {
            throw new NotImplementedException();
        }

        public Odontologista Obter(Odontologista entity)
        {
            throw new NotImplementedException();
        }
    }
}
