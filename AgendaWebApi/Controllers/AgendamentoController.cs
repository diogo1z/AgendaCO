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
    public class AgendamentoController : ApiController
    {
        [HttpGet]
        [Route("api/Agendamento/DiasDisponiveis")]
        public IEnumerable<DateTime> DiasDisponiveis(int meses, int idOdontologista, int idClinica)
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

                                var listaDatas = new List<DateTime>();
                                var dataAtual = DateTime.Now;

                                while (dataAtual <= DateTime.Now.AddMonths(meses))
                                {
                                    if (dayOff.Where(x => x.Data.Date == dataAtual.Date).Count() >= 1)
                                    {
                                        dataAtual = dataAtual.AddDays(1);
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
                                                    ).Count() == 0)
                                                {
                                                    listaDatas.Add(dataAtual);
                                                    horario = d.HorarioAtendimentoTermino.Add(new TimeSpan(90000));
                                                }
                                                else
                                                {
                                                    horario = horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0));
                                                }
                                            } while (horario <= d.HorarioAtendimentoTermino);
                                        }
                                    }

                                    if (semana.Where(x => x.DiaSemana == (int)dataAtual.DayOfWeek + 1).Count() >= 1
                                        && listaDatas.Where(x => x.Date == dataAtual.Date).Count() == 0)
                                    {
                                        foreach (var s in semana.Where(x => x.DiaSemana == (int)dataAtual.DayOfWeek + 1))
                                        {
                                            TimeSpan horario = s.HorarioAtendimentoInicio;
                                            do
                                            {
                                                if (agendamentos.Where(x => x.Data.Date == dataAtual.Date &&
                                                    x.Horario >= horario &&
                                                    x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                    ).Count() == 0)
                                                {
                                                    listaDatas.Add(dataAtual);
                                                    horario = s.HorarioAtendimentoTermino.Add(new TimeSpan(90000));
                                                }
                                                horario = horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0));
                                            } while (horario <= s.HorarioAtendimentoTermino);
                                        }
                                    }
                                    dataAtual = dataAtual.AddDays(1);
                                }
                                return listaDatas;
                            }
                        }
                    }
                }
            }
        }

        [HttpGet]
        [Route("api/Agendamento")]
        public IEnumerable<Agendamento> Get(DateTime data, int idOdontologista, int idClinica)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());

            using (var repositorio = new AgendaRepositorio(conexao))
            {
                var agenda = repositorio.Buscar(new Agenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).FirstOrDefault();
                if (agenda == null)
                    return null;
                using (var repositorioAgendamento = new AgendamentoRepositorio(conexao))
                {
                    var agendamentos = repositorioAgendamento.Buscar(new Agendamento() { IdAgenda = agenda.Id }).Where(x => x.Data == data.Date);

                    using (var repositorioSemana = new SemanaAgendaRepositorio(conexao))
                    {
                        var semana = repositorioSemana.Buscar(new SemanaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.DiaSemana == (int)data.DayOfWeek + 1);

                        using (var repositorioDia = new DiaAgendaRepositorio(conexao))
                        {
                            var dia = repositorioDia.Buscar(new DiaAgenda() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.Data == data.Date);

                            using (var repositorioDayOff = new AgendaDayOffRepositorio(conexao))
                            {
                                var dayOff = repositorioDayOff.Buscar(new AgendaDayOff() { IdClinica = idClinica, IdOdontologista = idOdontologista }).Where(x => x.Data == data.Date);

                                var listaAgendamentos = new List<Agendamento>();

                                if (dayOff.Count() >= 1)
                                    return null;

                                if (dia.Count() >= 1)
                                {
                                    foreach (var d in dia.Where(x => x.Data.Date == data.Date))
                                    {
                                        TimeSpan horario = d.HorarioAtendimentoInicio;
                                        do
                                        {
                                            if (agendamentos.Where(x => x.Data.Date == data.Date &&
                                                x.Horario >= horario &&
                                                x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                ).Count() > 0)
                                            {
                                                var agendamento = agendamentos.Where(x => x.Data.Date == data.Date &&
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
                                                    IdPaciente = agendamento.IdPaciente,
                                                    Nome = agendamento.Nome
                                                });
                                            }
                                            else
                                            {
                                                listaAgendamentos.Add(new Agendamento()
                                                {
                                                    Id = 0,
                                                    Data = data.Date,
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

                                if (semana.Count() >= 1)
                                {
                                    foreach (var s in semana.Where(x => x.DiaSemana == (int)data.DayOfWeek + 1))
                                    {
                                        TimeSpan horario = s.HorarioAtendimentoInicio;
                                        do
                                        {
                                            if (agendamentos.Where(x => x.Data.Date == data.Date &&
                                                x.Horario >= horario &&
                                                x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                ).Count() > 0)
                                            {
                                                var agendamento = agendamentos.Where(x => x.Data.Date == data.Date &&
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
                                                    IdPaciente = agendamento.IdPaciente,
                                                    Nome = agendamento.Nome
                                                });
                                            }
                                            else
                                            {
                                                listaAgendamentos.Add(new Agendamento()
                                                {
                                                    Id = 0,
                                                    Data = data.Date,
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
                                return listaAgendamentos;
                            }
                        }
                    }
                }
            }
        }

        [HttpGet]
        [Route("api/Agendamento/Todos")]
        public IEnumerable<OdontologistaAgendamento> Todos(DateTime data, int idClinica)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());
            try
            {
                using (var repositorioOdonto = new OdontologistaRepositorio(conexao))
                {
                    var listaOdonto = new List<OdontologistaAgendamento>();

                    repositorioOdonto.Buscar(idClinica).ToList().ForEach(
                        x => listaOdonto.Add(new OdontologistaAgendamento() { Odontologista = x }));
                    using (var repositorioAgenda = new AgendaRepositorio(conexao))
                    {
                        using (var repositorioAgendamento = new AgendamentoRepositorio(conexao))
                        {
                            using (var repositorioSemana = new SemanaAgendaRepositorio(conexao))
                            {
                                using (var repositorioDia = new DiaAgendaRepositorio(conexao))
                                {
                                    using (var repositorioDayOff = new AgendaDayOffRepositorio(conexao))
                                    {
                                        foreach (var item in listaOdonto)
                                        {
                                            var agenda = repositorioAgenda.Buscar(new Agenda() { IdClinica = idClinica, IdOdontologista = item.Odontologista.Id }).FirstOrDefault();
                                            if (agenda != null)
                                            {
                                                var agendamentos = repositorioAgendamento.Buscar(new Agendamento() { IdAgenda = agenda.Id }).Where(x => x.Data == data.Date);


                                                var semana = repositorioSemana.Buscar(new SemanaAgenda() { IdClinica = idClinica, IdOdontologista = item.Odontologista.Id }).Where(x => x.DiaSemana == (int)data.DayOfWeek + 1);


                                                var dia = repositorioDia.Buscar(new DiaAgenda() { IdClinica = idClinica, IdOdontologista = item.Odontologista.Id }).Where(x => x.Data == data.Date);


                                                var dayOff = repositorioDayOff.Buscar(new AgendaDayOff() { IdClinica = idClinica, IdOdontologista = item.Odontologista.Id }).Where(x => x.Data == data.Date);

                                                var listaAgendamentos = new List<Agendamento>();

                                                if (dayOff.Count() >= 1)
                                                    return null;

                                                if (dia.Count() >= 1)
                                                {
                                                    foreach (var d in dia.Where(x => x.Data.Date == data.Date))
                                                    {
                                                        TimeSpan horario = d.HorarioAtendimentoInicio;
                                                        do
                                                        {
                                                            if (agendamentos.Where(x => x.Data.Date == data.Date &&
                                                                x.Horario >= horario &&
                                                                x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                                ).Count() > 0)
                                                            {
                                                                var agendamento = agendamentos.Where(x => x.Data.Date == data.Date &&
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
                                                                    IdPaciente = agendamento.IdPaciente,
                                                                    Nome = agendamento.Nome
                                                                });
                                                            }
                                                            else
                                                            {
                                                                listaAgendamentos.Add(new Agendamento()
                                                                {
                                                                    Id = 0,
                                                                    Data = data.Date,
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

                                                if (semana.Count() >= 1)
                                                {
                                                    foreach (var s in semana.Where(x => x.DiaSemana == (int)data.DayOfWeek + 1))
                                                    {
                                                        TimeSpan horario = s.HorarioAtendimentoInicio;
                                                        do
                                                        {
                                                            if (agendamentos.Where(x => x.Data.Date == data.Date &&
                                                                x.Horario >= horario &&
                                                                x.Horario < horario.Add(new TimeSpan(0, 0, agenda.TempoAtendimento, 0, 0))
                                                                ).Count() > 0)
                                                            {
                                                                var agendamento = agendamentos.Where(x => x.Data.Date == data.Date &&
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
                                                                    IdPaciente = agendamento.IdPaciente,
                                                                    Nome = agendamento.Nome
                                                                });
                                                            }
                                                            else
                                                            {
                                                                listaAgendamentos.Add(new Agendamento()
                                                                {
                                                                    Id = 0,
                                                                    Data = data.Date,
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
                                                item.Agendamentos = listaAgendamentos;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return listaOdonto;
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

        [HttpGet]
        [Route("api/Agendamento")]
        public IEnumerable<Agendamento> Get(DateTime data, int idAgenda)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());

            using (var repositorioAgendamento = new AgendamentoRepositorio(conexao))
            {
                return repositorioAgendamento.Buscar(new Agendamento() { IdAgenda = idAgenda }).Where(x => x.Data == data.Date);
            }
        }

        [HttpGet]
        [Route("api/Agendamento")]
        public Agendamento Get(int idAgendamento)
        {
            using (var repositorioAgendamento = new AgendamentoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                return repositorioAgendamento.Buscar(new Agendamento() { Id = idAgendamento }).FirstOrDefault();
            }
        }

        //POST: api/Agenda
        [HttpPost]
        [Route("api/Agendamento")]
        public void Post([FromBody]Agendamento value)
        {
            var conexao = new Connection(new System.Data.SqlClient.SqlConnection());
            try
            {
                using (var repositorio = new AgendamentoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
                {
                    #region Validaçãoes
                    if (repositorio.Buscar(new Agendamento() { IdAgenda = value.IdAgenda }).Where(x => x.Data == value.Data && x.Horario == value.Horario).Count() > 0)
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new StringContent("Existe um outro agendamento para esse mesmo horário."),
                            ReasonPhrase = "Agendamento inválido"
                        });
                    #endregion


                    repositorio.Criar(value);
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

        [HttpDelete]
        [Route("api/Agendamento")]
        public void Delete(int id)
        {
            using (var repositorio = new AgendamentoRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            {
                repositorio.Deletar(new Agendamento() { Id = id });
            }
        }

    }
}
