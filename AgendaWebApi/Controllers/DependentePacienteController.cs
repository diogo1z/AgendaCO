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
    public class DependentePacienteController : ApiController
    {
        [HttpGet]
        [Route("api/DependentePaciente")]
        public IEnumerable<DependentePaciente> Get(int idPaciente)
        {

            try
            {
                using (var repositorio = new DependenteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(new DependentePaciente() { IdPaciente = idPaciente }).Where(x => x.IdPaciente == idPaciente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/DependentePaciente")]
        public DependentePaciente Get(int idPaciente, int id)
        {
            try
            {
                using (var repositorio = new DependenteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(new DependentePaciente() { IdPaciente = idPaciente }).Where(x => x.Id == id).FirstOrDefault(); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("api/DependentePaciente")]
        public void Post([FromBody]DependentePaciente value)
        {
            try
            {
                #region Validaçãoes
                if (string.IsNullOrWhiteSpace(value.Nome))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Nome inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (!Validacoes.ValidaDataNascimento(value.DataNascimento))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Data de nascimento inválida."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (value.IdPaciente <= 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Informe o paciente."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (!string.IsNullOrEmpty(value.Celular))
                    if (!Validacoes.ValidaCelular(value.Celular))
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Celular inválido."),
                            ReasonPhrase = "Campo inválido"
                        });

                if (!string.IsNullOrEmpty(value.Telefone))
                    if (!Validacoes.ValidaTelefone(value.Telefone))
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Telefone inválido."),
                            ReasonPhrase = "Campo inválido"
                        });
                #endregion

                using (var repositorio = new DependenteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    int id = repositorio.Criar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/DependentePaciente")]
        public void Put([FromBody]DependentePaciente value)
        {
            try
            {
                #region Validaçãoes
                if (string.IsNullOrWhiteSpace(value.Nome))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Nome inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (!Validacoes.ValidaDataNascimento(value.DataNascimento))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Data de nascimento inválida."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (value.IdPaciente <= 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Informe o paciente."),
                        ReasonPhrase = "Campo inválido"
                    });
                if (value.Id <= 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Informe o dependente."),
                        ReasonPhrase = "Campo inválido"
                    });
                #endregion

                using (var repositorio = new DependenteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Atualizar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("api/DependentePaciente")]
        public void Delete(int id)
        {
            using (var repositorio = new DependenteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new DependentePaciente() { Id = id });
            }
        }

    }
}
