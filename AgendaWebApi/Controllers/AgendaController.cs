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
    public class AgendaController : ApiController
    {
        // GET: api/Agenda
        //[HttpGet]
        //[Route("api/Agenda")]
        //public IEnumerable<Agenda> Get(int idClinica, int idOdontologista)
        //{
        //    throw new NotImplementedException();
        //}

        // GET: api/Agenda
        [HttpGet]
        [Route("api/Agenda")]
        public Agenda Get(int idClinica, int idOdontologista)
        {
            using (var repositorio = new AgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorio.Buscar(new Agenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).FirstOrDefault();
            }
        }

        [HttpGet]
        [Route("api/Agenda/Completo")]
        public List<Agendamento> Completo(int meses, int idOdontologista, int idClinica)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());

            using (var repositorio = new AgendaRepositorio(conexao))
            {
                var agenda = repositorio.Buscar(new Agenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).FirstOrDefault();
                if (agenda == null)
                    return null;
                using (var repositorioAgendamento = new AgendamentoRepositorio(conexao))
                {
                    var agendamentos = repositorioAgendamento.Buscar(new Agendamento() { IdAgenda = agenda.Id });

                    using (var repositorioSemana = new SemanaAgendaRepositorio(conexao))
                    {
                        var semana = repositorioSemana.Buscar(new SemanaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista });

                        using (var repositorioDia = new DiaAgendaRepositorio(conexao))
                        {
                            var dia = repositorioDia.Buscar(new DiaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista });

                            using (var repositorioDayOff = new AgendaDayOffRepositorio(conexao))
                            {
                                var dayOff = repositorioDayOff.Buscar(new AgendaDayOff() { IdClinica = idClinica, IdOdontologista = idOdontologista });

                                var listaAgendamentos = new List<Agendamento>();
                                var dataAtual = DateTime.Now;

                                while (dataAtual <= DateTime.Now.AddMonths(meses))
                                {
                                    if (dayOff.Where(x => x.Data.Date == dataAtual.Date).Count() >= 1)
                                    {
                                        dataAtual.AddDays(1);
                                        continue;
                                    }

                                    if (dia.Where(x => x.Data.Date == dataAtual.Date).Count() >= 1)
                                    {
                                        foreach (var d in dia.Where(x => x.Data.Date == dataAtual.Date))
                                        {
                                            TimeSpan horario = d.HorarioAtendimentoInicio;
                                            do
                                            {
                                                if (agendamentos.Where(x => x.Data.Date == dataAtual.Date &&
                                                    x.Horario >= horario &&
                                                    x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                    ).Count() > 0)
                                                {
                                                    var agendamento = agendamentos.Where(x => x.Data.Date == dataAtual.Date &&
                                                    x.Horario >= horario &&
                                                    x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                    ).FirstOrDefault();

                                                    listaAgendamentos.Add(new Agendamento()
                                                    {
                                                        Id = agendamento.Id,
                                                        Data = agendamento.Data,
                                                        Horario = agendamento.Horario,
                                                        IdAgenda = agenda.Id,
                                                        IdOdonto = agenda.IdOdontologista,
                                                        IdPaciente = agendamento.IdPaciente
                                                    });
                                                }
                                                else
                                                {
                                                    listaAgendamentos.Add(new Agendamento()
                                                    {
                                                        Id = 0,
                                                        Data = dataAtual.Date,
                                                        Horario = horario,
                                                        IdAgenda = agenda.Id,
                                                        IdOdonto = agenda.IdOdontologista,
                                                        IdPaciente = 0
                                                    });
                                                }
                                                horario = horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0));
                                            } while (horario <= d.HorarioAtendimentoTermino);
                                        }
                                    }

                                    if (semana.Where(x => x.DiaSemana == (int)dataAtual.DayOfWeek + 1).Count() >= 1)
                                    {
                                        foreach (var s in semana.Where(x => x.DiaSemana == (int)dataAtual.DayOfWeek + 1))
                                        {
                                            TimeSpan horario = s.HorarioAtendimentoInicio;
                                            do
                                            {
                                                if (agendamentos.Where(x => x.Data.Date == dataAtual.Date &&
                                                    x.Horario >= horario &&
                                                    x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                    ).Count() > 0)
                                                {
                                                    var agendamento = agendamentos.Where(x => x.Data.Date == dataAtual.Date &&
                                                    x.Horario >= horario &&
                                                    x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                    ).FirstOrDefault();

                                                    listaAgendamentos.Add(new Agendamento()
                                                    {
                                                        Id = agendamento.Id,
                                                        Data = agendamento.Data,
                                                        Horario = agendamento.Horario,
                                                        IdAgenda = agenda.Id,
                                                        IdOdonto = agenda.IdOdontologista,
                                                        IdPaciente = agendamento.IdPaciente
                                                    });
                                                }
                                                else
                                                {
                                                    listaAgendamentos.Add(new Agendamento()
                                                    {
                                                        Id = 0,
                                                        Data = dataAtual.Date,
                                                        Horario = horario,
                                                        IdAgenda = agenda.Id,
                                                        IdOdonto = agenda.IdOdontologista,
                                                        IdPaciente = 0
                                                    });
                                                }
                                                horario = horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0));
                                            } while (horario <= s.HorarioAtendimentoTermino);
                                        }
                                    }
                                    dataAtual = dataAtual.AddDays(1);
                                }
                                return listaAgendamentos;
                            }
                        }
                    }
                }
            }
        }

        //POST: api/Agenda
        [HttpPost]
        [Route("api/Agenda")]
        public void Post([FromBody]Agenda value)
        {
            try
            {
                #region Validaçãoes                
                #endregion

                using (var repositorio = new AgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Criar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/Odontologista/
        [HttpPut]
        [Route("api/Agenda")]
        public void Put([FromBody]Agenda value)
        {
            try
            {
                #region Validaçãoes                
                #endregion

                using (var repositorio = new AgendaRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    repositorio.Atualizar(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/Agenda/5
        [HttpDelete]
        [Route("api/Agenda")]
        public void Delete(int idClinica, int id)
        {
            throw new NotImplementedException();
        }
    }
}
