using AgendaDAL;
using AgendaDTL;
using MVCorp.Db;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace AgendaWebApi.Controllers
{
    public class ClinicaController : ApiController
    {
        //// GET: api/Clinica
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        [HttpGet]
        [Route("api/Clinica")]
        public Clinica Get(int id)
        {
            using (var repositorio = new ClinicaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Obter(new Clinica() { Id = id });
            }
        }

        [HttpGet]
        [Route("api/Clinica")]
        public IEnumerable<Clinica> Get(int idCidade, int idBairro)
        {
            using (var repositorio = new ClinicaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                var listaClinicas = repositorio.Buscar(new Clinica() { Endereco = new Endereco() { IdCidade = idCidade } });
                if (idBairro > 0)
                    return listaClinicas.Where(x => x.Endereco.IdBairro == idBairro);
                else
                    return listaClinicas;
            }
        }

        [HttpPost]
        [Route("api/Clinica")]
        public void Post([FromBody]Clinica value)
        {
            using (var transactionScope = new TransactionScope())
            {
                var conexao = new Connection(new SqlConnection());
                try
                {
                    #region Validações  
                    if (value.Usuario == null)
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Usuário não informado."),
                            ReasonPhrase = "Campo inválido"
                        });

                    if (value.Endereco == null)
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Endereço não informado."),
                            ReasonPhrase = "Campo inválido"
                        });

                    if (string.IsNullOrWhiteSpace(value.Usuario.Login))
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Login não informado."),
                            ReasonPhrase = "Campo inválido"
                        });

                    if (string.IsNullOrWhiteSpace(value.Usuario.Senha))
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Senha não informada."),
                            ReasonPhrase = "Campo inválido"
                        });
                    #endregion
                    using (var repositorioEndereco = new EnderecoRepositorio(conexao))
                    using (var repositorioClinica = new ClinicaRepositorio(conexao))
                    using (var repositorioUsuario = new UsuarioRepositorio(conexao))
                    {
                        if (value.Endereco.IdBairro <= 0)
                                value.Endereco.IdBairro = repositorioEndereco.ObterIdBairro(value.Endereco);

                        value.Id = repositorioClinica.Criar(value);
                        value.Usuario.idClinica = value.Id;
                        repositorioUsuario.Criar(value.Usuario);
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        [HttpPut]
        [Route("api/Clinica")]
        public void Put([FromBody]Clinica value)
        {
            #region Validaçãoes            
            #endregion
            using (var repositorio = new ClinicaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Atualizar(value);
            }
        }

        [HttpDelete]
        [Route("api/Clinica")]
        public void Delete(int id)
        {
            using (var repositorio = new ClinicaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new Clinica() { Id = id });
            }
        }

        [HttpGet]
        [Route("api/Clinica/GetEstados")]
        public IEnumerable<Estado> GetEstados()
        {
            using (var repositorio = new EnderecoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.ListarEstadoClinica();
            }
            
        }

        [HttpGet]
        [Route("api/Clinica/GetCidades")]
        public IEnumerable<Cidade> GetCidades(int idEstado)
        {
            using (var repositorio = new EnderecoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.ListarCidadeClinica(idEstado);
            }
        }

        [HttpGet]
        [Route("api/Clinica/GetBairros")]
        public IEnumerable<string> GetBairros(int idCidade)
        {
            using (var repositorio = new EnderecoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.ListarBairroClinica(idCidade);
            }
        }
    }
}
