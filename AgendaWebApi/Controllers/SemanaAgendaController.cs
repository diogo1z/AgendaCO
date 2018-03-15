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
    public class SemanaAgendaController : ApiController
    {

        // GET: api/Agenda
        [HttpGet]
        [Route("api/SemanaAgenda")]
        public IEnumerable<SemanaAgenda> Get(int idClinica, int idOdontologista)
        {
            using (var repositorio = new SemanaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new SemanaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista });
            }
        }

        // GET: api/Agenda
        [HttpGet]
        [Route("api/SemanaAgenda")]
        public SemanaAgenda Get(int idClinica, int idOdontologista, int id)
        {
            using (var repositorio = new SemanaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new SemanaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.Id == id).FirstOrDefault();
            }
        }


        // POST: api/Agenda
        [HttpPost]
        [Route("api/SemanaAgenda")]
        public void Post([FromBody]SemanaAgenda value)
        {
            try
            {
                #region Validaçãoes
                if (!Enum.IsDefined(typeof(AgendaEnums.DiasSemana), value.DiaSemana))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Dia da semana inválido."),
                        ReasonPhrase = "Dia inválido"
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


                if (!ValidaIntervalo(0,value.IdOdontologista, value.IdClinica, value.DiaSemana, value.HorarioAtendimentoInicio, value.HorarioAtendimentoTermino))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent("Intervalo inválido, já existe na agenda essa data."),
                        ReasonPhrase = "Intervalo inválido"
                    });

                #endregion

                using (var repositorio = new SemanaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Criar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/SemanaAgenda/
        [HttpPut]
        [Route("api/SemanaAgenda")]
        public void Put([FromBody]SemanaAgenda value)
        {
            #region Validaçãoes
            if (!Enum.IsDefined(typeof(AgendaEnums.DiasSemana), value.DiaSemana))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Dia da semana inválido."),
                    ReasonPhrase = "Dia inválido"
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


            if (!ValidaIntervalo(value.Id, value.IdOdontologista, value.IdClinica, value.DiaSemana, value.HorarioAtendimentoInicio, value.HorarioAtendimentoTermino))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Intervalo inválido, já existe na agenda essa data."),
                    ReasonPhrase = "Intervalo inválido"
                });
            #endregion
            using (var repositorio = new SemanaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Atualizar(value);
            }
        }

        // DELETE: api/SemanaAgenda/5
        [HttpDelete]
        [Route("api/SemanaAgenda")]
        public void Delete(int id)
        {
            using (var repositorio = new SemanaAgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new SemanaAgenda() { Id = id });
            }
        }

        private bool ValidaIntervalo(int id, int idOdonto, int idClinica, int dia, TimeSpan horaInicio, TimeSpan horaFim)
        {
            return this.Get(idClinica, idOdonto)
                .Where(x => ((x.HorarioAtendimentoInicio >= horaInicio && x.HorarioAtendimentoInicio < horaFim) ||
                            (x.HorarioAtendimentoTermino > horaInicio && x.HorarioAtendimentoTermino <= horaFim))
                            && x.DiaSemana == dia && (x.Id != id || id == 0)).ToList().Count == 0;
        }
    }
}
