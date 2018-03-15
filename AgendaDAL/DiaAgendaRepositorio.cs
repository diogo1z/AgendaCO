using AgendaDb.Interfaces;
using AgendaDTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL
{
    public class DiaAgendaRepositorio : Interfaces.IRepositorio<AgendaDTL.DiaAgenda>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public DiaAgendaRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.DiaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@data", entity.Data);
            _inners.Add("@horarioAtendimentoInicio", entity.HorarioAtendimentoInicio);
            _inners.Add("@horarioAtendimentoTermino", entity.HorarioAtendimentoTermino);
            return _context.Save("s_CriarDiaAgenda", _inners);
        }

        public void Atualizar(AgendaDTL.DiaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@data", entity.Data);
            _inners.Add("@horarioAtendimentoInicio", entity.HorarioAtendimentoInicio);
            _inners.Add("@horarioAtendimentoTermino", entity.HorarioAtendimentoTermino);
            _context.Save("s_AlterarDiaAgenda", _inners);
        }

        public void Deletar(AgendaDTL.DiaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);

            _context.Save("s_ExcluirDiaAgenda", _inners);
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

        public IEnumerable<AgendaDTL.DiaAgenda> Buscar(AgendaDTL.DiaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);

            return _context.Get<DiaAgenda>("s_ListarDiaAgenda", _inners);
        }

        public AgendaDTL.DiaAgenda Obter(AgendaDTL.DiaAgenda entity)
        {
            throw new NotImplementedException();
        }
    }
}
