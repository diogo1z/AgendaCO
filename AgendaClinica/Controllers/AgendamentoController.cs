using AgendaClinica.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AgendaClinica.Controllers
{
    [Authorize]
    public class AgendamentoController : ExtensaoController
    {
        // GET: Agendamento
        public ActionResult Index()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Odontologista?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var dentistas = JsonConvert.DeserializeObject<List<AgendaDTL.Odontologista>>(EmpResponse);
            
            ViewBag.OdontoId = new SelectList(
                    dentistas,
                    "Id",
                    "Nome"
                );

            ViewBag.IdClinica = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();

            return View();
        }

        public ActionResult EscolherPaciente()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Paciente/ComDependentes?idClinica=" + identity.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            //var response = client.GetAsync("Paciente/ComDependentes?idClinica=" + identity.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var pacientes = JsonConvert.DeserializeObject<List<AgendaDTL.Paciente>>(EmpResponse);

            var model = new BuscarPacientesVM();
            model.Pacientes = new List<PacienteVM>();

            foreach (var paciente in pacientes)
            {
                model.Pacientes.Add(new PacienteVM()
                {
                    Id = paciente.Id,
                    DataNascimento = paciente.DataNascimento,
                    Cpf = paciente.Cpf,
                    Celular = paciente.Celular,
                    Email = paciente.Email,
                    Nome = paciente.Nome,
                    Telefone = paciente.Telefone
                });
            }
            return View(model);
        }

        public ActionResult BuscarPaciente(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Paciente?cpf=" + cpf).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var paciente = JsonConvert.DeserializeObject<List<AgendaDTL.Paciente>>(EmpResponse).FirstOrDefault();

            if (paciente == null)
                return View();

            var model = new PacienteVM()
            {
                Id = paciente.Id,
                DataNascimento = paciente.DataNascimento,
                Cpf = paciente.Cpf,
                Celular = paciente.Celular,
                Email = paciente.Email,
                Nome = paciente.Nome,
                Telefone = paciente.Telefone,
                Dependente = new Dependente() { DataNascimento = DateTime.Now.AddDays(-200) }
            };

            response = client.GetAsync("DependentePaciente?idPaciente=" + model.Id).Result;
            EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var dependentes = JsonConvert.DeserializeObject<List<AgendaDTL.DependentePaciente>>(EmpResponse);
            model.Dependentes = new List<Dependente>();
            foreach (var dependente in dependentes)
            {
                model.Dependentes.Add(
                    new Dependente()
                    {
                        Id = dependente.Id,
                        DataNascimento = dependente.DataNascimento,
                        //Celular = dependente.Celular,
                        Nome = dependente.Nome,
                        //Telefone = dependente.Telefone,
                        IdPaciente = dependente.IdPaciente
                    });
            }
            return View(model);
        }

        public ActionResult EscolherOdontologista(int idPaciente)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Odontologista?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var dentistas = JsonConvert.DeserializeObject<List<AgendaDTL.Odontologista>>(EmpResponse);

            var model = new AgendamentoVM();

            model.IdPaciente = idPaciente;

            ViewBag.OdontoId = new SelectList(
                    dentistas,
                    "Id",
                    "Nome"
                );

            ViewBag.IdClinica = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult EscolherOdontologista(int idPaciente, int idAgenda, string data, string horario)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.PostAsync("Agendamento",
                    new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("IdAgenda", idAgenda.ToString()),
                        new KeyValuePair<string, string>("IdPaciente", idPaciente.ToString()),
                        new KeyValuePair<string, string>("Data", data),//.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Horario", horario)//.ToString("hh:mm"))
                    })).Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Agendamento Realizado.", "Sucesso!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex);
                return View();
            }
        }


        [HttpGet]
        public ActionResult Protocolo(int idPaciente, int idAgenda, string data, string horario)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.PostAsync("Agendamento",
                    new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("IdAgenda", idAgenda.ToString()),
                        new KeyValuePair<string, string>("IdPaciente", idPaciente.ToString()),
                        new KeyValuePair<string, string>("Data", data),//.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Horario", horario)//.ToString("hh:mm"))
                    })).Result;
            
                var model = new AgendamentoVM()
                {
                    IdPaciente = idPaciente,
                    IdAgenda = idAgenda,
                    DataAgendamento = Convert.ToDateTime(data),
                    HoraAgendamento = TimeSpan.Parse(horario)
                };
                if (response.IsSuccessStatusCode)
                {   
                    return View(model);
                }
                else
                {
                    model.IdPaciente = 0;
                    ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex);
                return View();
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.DeleteAsync("Agendamento?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault() + "&id=" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Agendamento Excluido.", "Sucesso!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex);
                return View();
            }
        }
    }
}