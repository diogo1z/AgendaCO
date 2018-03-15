using AgendaDAL;
using AgendaDTL;
using AgendaUtils;
using MVCorp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgendaWebApi.Controllers
{
    public class OdontologistaController : ApiController
    {
        // GET: api/Odontologista
        [HttpGet]
        [Route("api/Odontologista")]
        public IEnumerable<Odontologista> Get(int idClinica)
        {

            try
            {
                using (var repositorio = new OdontologistaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(idClinica);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Odontologista
        [HttpGet]
        [Route("api/Odontologista")]
        public Odontologista Get(int idClinica, int id)
        {

            try
            {
                using (var repositorio = new OdontologistaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(idClinica).Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // POST: api/Odontologista
        [HttpPost]
        [Route("api/Odontologista")]
        public void Post([FromBody]Odontologista value)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());
            try
            {
                #region Validaçãoes
                if (!Validacoes.ValidaCpf(value.Cpf))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cpf inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                else
                    value.Cpf = value.Cpf.Replace(".", "").Replace("-", "");

                if (!Validacoes.ValidaCep(value.Cep))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cep inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                else
                    value.Cep = value.Cep.Replace("-", "");

                if (!Validacoes.ApenasNumeros(value.Cro))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cro inválido."),
                        ReasonPhrase = "Campo inválido"
                    });

                if (!Enum.IsDefined(typeof(AgendaEnums.Uf), value.CroEstado))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cro UF inválido."),
                        ReasonPhrase = "Campo inválido"
                    });

                if (string.IsNullOrWhiteSpace(value.Nome))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Nome inválido."),
                        ReasonPhrase = "Campo inválido"
                    });

                if (value.IdClinica <= 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Clínica inválida."),
                        ReasonPhrase = "Campo inválido"
                    });

                if (!Validacoes.ValidaDataNascimento(value.DataNascimento))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Data de nascimento inválida."),
                        ReasonPhrase = "Campo inválido"
                    });
                #endregion

                using (var repositorio = new OdontologistaRepositorio(conexao))
                {
                    int id = repositorio.Criar(value);
                    if (id > 0)
                    {
                        using (var repositorioAgenda = new AgendaRepositorio(conexao))
                        {
                            repositorioAgenda.Criar(new Agenda(id, value.IdClinica, 30));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexao.Dispose();
            }
        }

        // PUT: api/Odontologista/
        [HttpPut]
        [Route("api/Odontologista")]
        public void Put([FromBody]Odontologista value)
        {
            #region Validaçãoes
            if (value.Id <= 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Objeto inválido."),
                    ReasonPhrase = "Campo inválido"
                });

            if (!Validacoes.ValidaCpf(value.Cpf))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Cpf inválido."),
                    ReasonPhrase = "Campo inválido"
                });
            else
                value.Cpf = value.Cpf.Replace(".", "").Replace("-", "");

            if (!Validacoes.ValidaCep(value.Cep))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Cep inválido."),
                    ReasonPhrase = "Campo inválido"
                });
            else
                value.Cep = value.Cep.Replace("-", "");

            if (!Validacoes.ApenasNumeros(value.Cro))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Cro inválido."),
                    ReasonPhrase = "Campo inválido"
                });

            if (!Enum.IsDefined(typeof(AgendaEnums.Uf), value.CroEstado))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Cro UF inválido."),
                    ReasonPhrase = "Campo inválido"
                });

            if (string.IsNullOrWhiteSpace(value.Nome))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Nome inválido."),
                    ReasonPhrase = "Campo inválido"
                });

            if (value.IdClinica <= 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Clínica inválida."),
                    ReasonPhrase = "Campo inválido"
                });

            if (!Validacoes.ValidaDataNascimento(value.DataNascimento))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Data de nascimento inválida."),
                    ReasonPhrase = "Campo inválido"
                });
            #endregion
            using (var repositorio = new OdontologistaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Atualizar(value);
            }
        }

        // DELETE: api/Odontologista/5
        [HttpDelete]
        [Route("api/Odontologista")]
        public void Delete(int idClinica, int id)
        {
            using (var repositorio = new OdontologistaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new Odontologista() { Id = id, IdClinica = idClinica });
            }
        }
    }
}
