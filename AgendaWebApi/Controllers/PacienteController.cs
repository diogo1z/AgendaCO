using AgendaDAL;
using AgendaDTL;
using AgendaUtils;
using MVCorp.Db;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace AgendaWebApi.Controllers
{
    public class PacienteController : ApiController
    {
        [HttpGet]
        [Route("api/Paciente")]
        public IEnumerable<Paciente> Get(int idClinica)
        {

            try
            {
                using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(new Paciente() { IdClinicaDeCadastro = idClinica });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/Paciente/ComDependentes")]
        public IEnumerable<Paciente> ComDependentes(int idClinica)
        {

            try
            {
                using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(new Paciente() { IdClinicaDeCadastro = idClinica }, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/Paciente")]
        public IEnumerable<Paciente> Get(string cpf)
        {
            try
            {
                using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Buscar(new Paciente() { Cpf = cpf });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/Paciente")]
        public Paciente Get(int idClinica, int id)
        {
            try
            {
                using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    return repositorio.Obter(new Paciente() { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("api/Paciente")]
        public int Post([FromBody]Paciente value)
        {
            using (var transactionScope = new TransactionScope())
            {
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

                    if (!string.IsNullOrEmpty(value.Cep))
                        if (!Validacoes.ValidaCep(value.Cep))
                            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                            {
                                Content = new StringContent("Cep inválido."),
                                ReasonPhrase = "Campo inválido"
                            });
                        else
                            value.Cep = value.Cep.Replace("-", "");

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

                    var conexao = new Connection(new System.Data.SqlClient.SqlConnection());
                    using (var repositorio = new PacienteRepositorio(conexao))
                    {
                        int idPaciente = repositorio.Criar(value);

                        if (value.Usuario != null)
                            if (!string.IsNullOrEmpty(value.Usuario.Senha))
                                using (var repositorioUsuario = new UsuarioRepositorio(conexao))
                                {
                                    value.Usuario.Login = value.Email;
                                    value.Usuario.idPaciente = idPaciente;
                                    repositorioUsuario.Criar(value.Usuario);
                                }

                        transactionScope.Complete();
                        return idPaciente;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpPut]
        [Route("api/Paciente")]
        public void Put([FromBody]Paciente value)
        {
            #region Validaçãoes
            if (value.Id <= 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Paciente inválido."),
                    ReasonPhrase = "Campo inválido"
                });

            if (!string.IsNullOrEmpty(value.Cpf))
                if (!Validacoes.ValidaCpf(value.Cpf))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cpf inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                else
                    value.Cpf = value.Cpf.Replace(".", "").Replace("-", "");

            if (!string.IsNullOrEmpty(value.Cep))
                if (!Validacoes.ValidaCep(value.Cep))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Cep inválido."),
                        ReasonPhrase = "Campo inválido"
                    });
                else
                    value.Cep = value.Cep.Replace("-", "");

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

            using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Atualizar(value);
            }
        }

        [HttpDelete]
        [Route("api/Paciente")]
        public void Delete(int id)
        {
            using (var repositorio = new PacienteRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new Paciente() { Id = id });
            }
        }
    }
}
