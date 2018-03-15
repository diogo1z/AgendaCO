using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCorp.Db
{
    public class Connection : AgendaDb.Interfaces.IConnection
    {
        private static string connectionString = ConfigurationManager.AppSettings["DataConnectionString"];
        private IDbConnection _conn = null;
        private const string isInvalid = "is invalid\nORA-06550";
        private const string mustBeDeclared = "must be declared\nORA-06550";
        private const string mensagemIsInvalid = "{0} está invalida. Verifique os objetos invalidos no banco de dados";
        private const string mensagemMustBeDeclared = "O objeto {0} não existe ou owner da aplicação precisa de permissão para executar esse objeto.";

        public IDbConnection DatabaseConnection
        {
            get
            {
                return _conn;
            }
        }

        public Connection(IDbConnection c)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            if (_conn == null)
            {
                c.ConnectionString = connectionString;
                _conn = c;
                _conn.Open();
            }

        }

        public IEnumerable<T> Get<T>(string query) where T : class
        {
            try
            {
                return _conn.Query<T>(query, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        public IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners) where T : class
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, null, null, null);
                return _conn.Query<T>(query, dynamicParameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        public IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners, IDictionary<string, object> outers, IDictionary<string, object> refcursor) where T : class
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, outers, refcursor, null);

                return _conn.Query<T>(query, dynamicParameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }
        
        public IEnumerable<T> Get<T>(string query, IDictionary<string, object> inners, IDictionary<string, object> refcursor) where T : class
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, null, refcursor, null);

                return _conn.Query<T>(query, dynamicParameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        public IDictionary<string, object> Get(string query, IDictionary<string, object> inners, IDictionary<string, object> outers)
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, outers, null, null);
                var dictionaryOuters = new Dictionary<string, object>();
                var dataReader = (IWrappedDataReader)_conn.ExecuteReader(query, dynamicParameters, commandType: CommandType.StoredProcedure);

                var parameters = dataReader.Command.Parameters;
                foreach (var i in outers)
                {
                    var param = parameters[i.Key];
                    dictionaryOuters.Add(i.Key, param.GetType().GetProperty("Value").GetValue(param));
                }

                return dictionaryOuters;
            }
            catch (Exception e)
            {
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        public long Get(string query, IDictionary<string, object> inners)
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, null, null, null);
                var retorno = _conn.Query<long>(query, dynamicParameters, commandType: CommandType.StoredProcedure);
                return retorno.AsList().Count == 0 ? retorno.AsList().Count : Convert.ToInt64(retorno.SingleOrDefault());
            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        public int Save(string query, IDictionary<string, object> inners)
        {
            try
            {
                var dynamicParameters = SetDynamicParameters(inners, null, null, null);
                var retorno = _conn.Query<int>(query, dynamicParameters, commandType: CommandType.StoredProcedure);
                return retorno.AsList().Count > 0 ? retorno.SingleOrDefault() : retorno.AsList().Count();
            }
            catch (Exception e)
            {
                //Dispose();
                if (e.Message.Contains(isInvalid))
                    throw new Exception(string.Format(mensagemIsInvalid, query));
                else if (e.Message.Contains(mustBeDeclared))
                    throw new Exception(string.Format(mensagemMustBeDeclared, query));

                throw;
            }
        }

        private static SqlMapper.IDynamicParameters SetDynamicParameters(IDictionary<string, object> inners, IDictionary<string, object> outers, IDictionary<string, object> refcursor, IDictionary<string, object> inouters)
        {

            var dynamicParameters = new DynamicParameters();

            if (inners != null)
                foreach (var keyPair in inners)
                    //if (keyPair.Value != null && keyPair.Value.GetType().ToString().ToLower() == "system.byte[]")
                    //    dynamicParameters.Add(keyPair.Key, keyPair.Value, dbType: OracleDbType.Blob);
                    //else
                    dynamicParameters.Add(keyPair.Key, keyPair.Value);

            //if (outers != null)
            //    foreach (var keyPair in outers)
            //        dynamicParameters.Add(keyPair.Key, DBNull.Value, dbType: (keyPair.Value != null ? (OracleDbType)Enum.Parse(typeof(OracleDbType), keyPair.Value.ToString(), true) : OracleDbType.Varchar2), direction: ParameterDirection.Output, size: 4000);

            //if (refcursor != null)
            //    foreach (var keyPair in refcursor)
            //        dynamicParameters.Add(keyPair.Key, dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

            //if (inouters != null)
            //    foreach (var keyPair in inouters)
            //        dynamicParameters.Add(keyPair.Key, keyPair.Value, direction: ParameterDirection.InputOutput);


            return dynamicParameters;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _conn.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
