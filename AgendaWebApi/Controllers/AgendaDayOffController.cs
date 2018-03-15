using AgendaDAL;
using AgendaDTL;
using MVCorp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgendaWebApi.Controllers
{
    public class AgendaDayOffController : ApiController
    {
        // GET: api/Agenda
        [HttpGet]
        [Route("api/AgendaDayOff")]
        public IEnumerable<AgendaDayOff> Get(int idClinica, int idOdontologista)
        {
            using (var repositorio = new AgendaDayOffRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new AgendaDayOff() { IdClinica = idClinica, IdOdontologista = idOdontologista });
            }
        }

        // GET: api/Agenda
        [HttpGet]
        [Route("api/AgendaDayOff")]
        public AgendaDayOff Get(int idClinica, int idOdontologista, int id)
        {
            using (var repositorio = new AgendaDayOffRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new AgendaDayOff() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.Id == id).FirstOrDefault();
            }
        }


        // POST: api/Agenda
        [HttpPost]
        [Route("api/AgendaDayOff")]
        public void Post([FromBody]AgendaDayOff value)
        {
            try
            {
                #region Validaçãoes                
                #endregion

                using (var repositorio = new AgendaDayOffRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Criar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        // DELETE: api/AgendaDayOff/5
        [HttpDelete]
        [Route("api/AgendaDayOff")]
        public void Delete(int id)
        {
            using (var repositorio = new AgendaDayOffRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new AgendaDayOff() { Id = id });
            }
        }
    }
}
