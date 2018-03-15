using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDAL.Interfaces
{
    public interface IRepositorio<T> : IDisposable
    {
        int Criar(T entity);
        IEnumerable<T> Buscar(T entity);
        T Obter(T entity);
        void Atualizar(T entity);
        void Deletar(T entity);
    }
}
