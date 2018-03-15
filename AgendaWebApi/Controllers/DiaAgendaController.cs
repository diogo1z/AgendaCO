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
    public class DiaAgendaController : ApiController
    {
        // GET: api/Agenda
        [HttpGet]
        [Route("api/DiaAgenda")]
        public IEnumerable<DiaAgenda> Get(int idClinica, int idOdontologista)
        {
            using (var repositorio = new DiaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new DiaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista });
            }
        }

        // GET: api/Agenda
        [HttpGet]
        [Route("api/DiaAgenda")]
        public DiaAgenda Get(int idClinica, int idOdontologista, int id)
        {
            using (var repositorio = new DiaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new DiaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.Id == id).FirstOrDefault();
            }
        }


        // POST: api/Agenda
        [HttpPost]
        [Route("api/DiaAgenda")]
        public void Post([FromBody]DiaAgenda value)
        {
            try
            {
                #region Validaçãoes
                if (value.Data == null)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Data inválida."),
                        ReasonPhrase = "Dia inválido"
                    });

                if (value.Data <= DateTime.Now.Date)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Data inválida."),
                        ReasonPhrase = "Data inválida"
                    });

                if (!Validacoes.ValidaTimeSpan24Hrs(value.HorarioAtendimentoInicio))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Horário de atendimento de início inválido."),
                        ReasonPhrase = "Horário inválido"
                    });

                if (!Validacoes.ValidaTimeSpan24Hrs(value.HorarioAtendimentoTermino))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Horário de atendimento de término inválido."),
                        ReasonPhrase = "Horário inválido"
                    });


                if (value.HorarioAtendimentoInicio > value.HorarioAtendimentoTermino)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Intervalo de atendimento inválido."),
                        ReasonPhrase = "Intervalo inválido"
                    });


                if (value.HorarioAtendimentoInicio > value.HorarioAtendimentoTermino)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Intervalo de atendimento inválido."),
                        ReasonPhrase = "Intervalo inválido"
                    });


                if (!ValidaIntervalo(0, value.IdOdontologista, value.IdClinica, value.Data, value.HorarioAtendimentoInicio, value.HorarioAtendimentoTermino))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Intervalo inválido, já existe na agenda essa data."),
                        ReasonPhrase = "Intervalo inválido"
                    });
                #endregion
                using (var repositorio = new DiaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Criar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/DiaAgenda/
        [HttpPut]
        [Route("api/DiaAgenda")]
        public void Put([FromBody]DiaAgenda value)
        {
            #region Validaçãoes
            if (value.Data == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Data inválida."),
                    ReasonPhrase = "Dia inválido"
                });

            if (value.Data <= DateTime.Now.Date)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Data inválida."),
                    ReasonPhrase = "Data inválida"
                });

            if (!Validacoes.ValidaTimeSpan24Hrs(value.HorarioAtendimentoInicio))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Horário de atendimento de início inválido."),
                    ReasonPhrase = "Horário inválido"
                });

            if (!Validacoes.ValidaTimeSpan24Hrs(value.HorarioAtendimentoTermino))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Horário de atendimento de término inválido."),
                    ReasonPhrase = "Horário inválido"
                });


            if (value.HorarioAtendimentoInicio > value.HorarioAtendimentoTermino)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Intervalo de atendimento inválido."),
                    ReasonPhrase = "Intervalo inválido"
                });


            if (value.HorarioAtendimentoInicio > value.HorarioAtendimentoTermino)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Intervalo de atendimento inválido."),
                    ReasonPhrase = "Intervalo inválido"
                });


            if (!ValidaIntervalo(value.Id, value.IdOdontologista, value.IdClinica, value.Data, value.HorarioAtendimentoInicio, value.HorarioAtendimentoTermino))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Intervalo inválido, já existe na agenda essa data."),
                    ReasonPhrase = "Intervalo inválido"
                });
            #endregion
            using (var repositorio = new DiaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Atualizar(value);
            }
        }

        // DELETE: api/DiaAgenda/5
        [HttpDelete]
        [Route("api/DiaAgenda")]
        public void Delete(int id)
        {
            using (var repositorio = new DiaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new DiaAgenda() { Id = id });
            }
        }

        private bool ValidaIntervalo(int id, int idOdonto, int idClinica, DateTime data, TimeSpan horaInicio, TimeSpan horaFim)
        {
            return this.Get(idClinica, idOdonto).Where(x => ((x.HorarioAtendimentoInicio >= horaInicio && x.HorarioAtendimentoInicio < horaFim) ||
                            (x.HorarioAtendimentoTermino > horaInicio && x.HorarioAtendimentoTermino <= horaFim))
                            && x.Data == data && (x.Id != id || id == 0)).ToList().Count == 0;
        }
    }
}
