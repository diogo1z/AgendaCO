using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaDTL;
using AgendaDb.Interfaces;

namespace AgendaDAL
{
    public class EnderecoRepositorio : Interfaces.IEnderecoRepositorio<AgendaDTL.Endereco>
    {
        private IConnection _context;
        private IDictionary<string, object> _inners;

        public EnderecoRepositorio(IConnection c)
        {
            _context = c;
            _inners = new Dictionary<string, object>();
        }

        public long ObterIdBairro(Endereco entity)
        {
            _inners.Clear();

            _inners.Add("@uf", entity.Estado);
            _inners.Add("@cidade", entity.Cidade);
            _inners.Add("@bairro", entity.Bairro);

            return _context.Get("s_ObterIdBairro", _inners);
        }


        public IEnumerable<Estado> ListarEstadoClinica()
        {
            _inners.Clear();

            return _context.Get<Estado>("s_ListarClinicaEstado", _inners);
        }

        public IEnumerable<Cidade> ListarCidadeClinica(int idEstado)
        {
            _inners.Clear();

            _inners.Add("@idEstado", idEstado);

            return _context.Get<Cidade>("s_ListarClinicaCidade", _inners);
        }

        public IEnumerable<string> ListarBairroClinica(int idCidade)
        {
            _inners.Clear();

            _inners.Add("@idCidade", idCidade);

            return _context.Get<string>("s_ListarClinicaBairro", _inners);
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
