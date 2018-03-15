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
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("api/Usuario")]
        public Usuario Get(string login, string senha, int tipoAcesso)
        {
            var usuario = new Usuario();
            usuario.Login = login;
            usuario.Senha = senha;

            try
            {
                using (var repositorio = new UsuarioRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    usuario = repositorio.Obter(usuario);
                    if (usuario != null)
                        return tipoAcesso == usuario.Perfil.Id ? usuario : null;
                    else return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET api/usuario/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/usuario
        public void Post([FromBody]string value)
        {
        }

        // PUT api/usuario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/usuario/5
        public void Delete(int id)
        {
        }
    }
}
