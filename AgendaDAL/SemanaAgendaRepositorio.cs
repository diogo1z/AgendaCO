using AgendaDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaDTL;

namespace AgendaDAL
{
    public class SemanaAgendaRepositorio : Interfaces.IRepositorio<AgendaDTL.SemanaAgenda>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;
        
        public SemanaAgendaRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public int Criar(AgendaDTL.SemanaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);
            _inners.Add("@diaSemana", entity.DiaSemana);
            _inners.Add("@horarioAtendimentoInicio", entity.HorarioAtendimentoInicio);
            _inners.Add("@horarioAtendimentoTermino", entity.HorarioAtendimentoTermino);
            return _context.Save("s_CriarSemanaAgenda", _inners);
        }
        
        public void Atualizar(AgendaDTL.SemanaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);
            _inners.Add("@horarioAtendimentoInicio", entity.HorarioAtendimentoInicio);
            _inners.Add("@horarioAtendimentoTermino", entity.HorarioAtendimentoTermino);
            _context.Save("s_AlterarSemanaAgenda", _inners);
        }

        public void Deletar(AgendaDTL.SemanaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@id", entity.Id);            

            _context.Save("s_ExcluirSemanaAgenda", _inners);
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

        public IEnumerable<AgendaDTL.SemanaAgenda> Buscar(AgendaDTL.SemanaAgenda entity)
        {
            _inners.Clear();

            _inners.Add("@idOdonto", entity.IdOdontologista);
            _inners.Add("@idClinica", entity.IdClinica);
            
            return _context.Get<SemanaAgenda>("s_ListarSemanaAgenda", _inners);
        }

        public AgendaDTL.SemanaAgenda Obter(AgendaDTL.SemanaAgenda entity)
        {
            throw new NotImplementedException();
        }
    }
}
