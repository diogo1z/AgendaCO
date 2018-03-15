using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDb.Interfaces
{
    public interface IConnection
    {
        IDbConnection DatabaseConnection { get; }

        IEnumerable<T> Get<T>(string query) where T : class;
        IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners) where T : class;
        IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners, IDictionary<string, object> refcursor) where T : class;
        IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners, IDictionary<string, object> outers, IDictionary<string, object> refcursor) where T : class;
        IDictionary<string, object> Get(string query, IDictionary<string, object> inners, IDictionary<string, object> outers);
        long Get(string query, IDictionary<string, object> inners);
        int Save(string query, IDictionary<string, object> inners);
        void Dispose();
    }
}
