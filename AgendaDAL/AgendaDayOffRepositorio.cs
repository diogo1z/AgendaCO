using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaDTL;
using AgendaDb.Interfaces;

namespace AgendaDAL
{
    public class AgendaDayOffRepositorio : Interfaces.IRepositorio<AgendaDTL.AgendaDayOff>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;

        public AgendaDayOffRepositorio(IConnection c)        
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDayOff entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@data", entity.Data);            
            return _context.Save("s_CriarAgendaDayOff", _inners);
        }

        public IEnumerable<AgendaDayOff> Buscar(AgendaDayOff entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);

            return _context.Get<AgendaDayOff>("s_ListarAgendaDayOff", _inners);
        }

        public AgendaDayOff Obter(AgendaDayOff entity)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(AgendaDayOff entity)
        {
            throw new NotImplementedException();
        }

        public void Deletar(AgendaDayOff entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);            

            _context.Save("s_ExcluirAgendaDayOff", _inners);
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
    }
}
