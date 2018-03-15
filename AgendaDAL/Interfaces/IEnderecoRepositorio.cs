using AgendaDTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL.Interfaces
{
    interface IEnderecoRepositorio<T> : IDisposable
    {
        long ObterIdBairro(T entity);
        IEnumerable<Estado> ListarEstadoClinica();
        IEnumerable<Cidade> ListarCidadeClinica(int idEstado);
        IEnumerable<string> ListarBairroClinica(int idCidade);
    }
}
