using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaDTL;
using AgendaDb.Interfaces;

namespace AgendaDAL
{
    public class AgendamentoRepositorio : Interfaces.IRepositorio<AgendaDTL.Agendamento>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public AgendamentoRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }
        public void Atualizar(Agendamento entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agendamento> Buscar(Agendamento entity)
        {
            _inners.Clear();

            if (entity.IdAgenda > 0)
                _inners.Add("@idAgenda", entity.IdAgenda);

            if (entity.IdPaciente > 0)
                _inners.Add("@idPaciente", entity.IdPaciente);

            if (entity.IdOdonto > 0)
                _inners.Add("@idOdontologista", entity.IdOdonto);

            if (entity.IdOdonto > 0)
                _inners.Add("@idAgendamento", entity.Id);

            return _context.Get<AgendaDTL.Agendamento>("s_ListarAgendamento", _inners);
        }

        public int Criar(Agendamento entity)
        {
            _inners.Clear();

            _inners.Add("@idAgenda", entity.IdAgenda);
            _inners.Add("@idPaciente", entity.IdPaciente);
            _inners.Add("@data", entity.Data);
            _inners.Add("@horario", entity.Horario);
            
            return _context.Save("s_CriarAgendamento", _inners);
        }

        public void Deletar(Agendamento entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            _context.Save("s_ExcluirAgendamento", _inners);
        }
        
        public Agendamento Obter(Agendamento entity)
        {
            throw new NotImplementedException();
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
