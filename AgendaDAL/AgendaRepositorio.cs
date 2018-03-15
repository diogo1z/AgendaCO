using AgendaDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class AgendaRepositorio : Interfaces.IRepositorio<AgendaDTL.Agenda>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public AgendaRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.Agenda entity)
        {
            _inners.Clear();

            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@idOdontologista", entity.IdOdontologista);
            _inners.Add("@tempoAtendimento", entity.TempoAtendimento);

            entity.Id = _context.Save("s_CriarAgenda", _inners);
            
            return entity.Id;
        }

        public IEnumerable<AgendaDTL.Odontologista> Buscar(int idClinica)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(AgendaDTL.Agenda entity)
        {
            _inners.Clear();

            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@idOdontologista", entity.IdOdontologista);
            _inners.Add("@tempoAtendimento", entity.TempoAtendimento);

            entity.Id = _context.Save("s_AlterarAgenda", _inners);
        }

        public void Deletar(AgendaDTL.Agenda entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@idClinica", entity.IdClinica);

            _context.Save("s_DeletarAgenda", _inners);
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

        public IEnumerable<AgendaDTL.Agenda> Buscar(AgendaDTL.Agenda entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);

            return _context.Get<AgendaDTL.Agenda>("s_ListarAgenda", _inners);
        }

        public AgendaDTL.Agenda Obter(AgendaDTL.Agenda entity)
        {
            throw new NotImplementedException();
        }
    }
}
