using AgendaDTL;
using AgendaUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AgendaUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calendario()
        {
            return View();
        }

        public ActionResult SelecioneClinica(int idCidade)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = client.GetAsync("Clinica?idCidade=" + idCidade + "&idBairro=0").Result;
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var EmpResponse = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                var listaCidades = JsonConvert.DeserializeObject<IEnumerable<Clinica>>(EmpResponse);
                var model = new ListaClinicasVM();
                model.ListaClinicas = listaCidades;
                return View(model);
            }

            return View();
            //var listaCidades = response.Content.xReadAsAsync<IEnumerable<Usuario>>().Result;

        }


        [HttpGet]
        public ActionResult DataClinica(int idClinica)
        {
            var model = new Models.DataClinicaVM();
            model.IdClinica = idClinica;
            ViewBag.IdClinica = idClinica;
            return View();
        }

        [HttpPost]
        public ActionResult DataClinica(Models.Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                var cookie = new HttpCookie("agendamento", JsonConvert.SerializeObject(agendamento));
                cookie.Expires.AddDays(1);
                HttpContext.Response.Cookies.Add(cookie);
                return Json(new { ok = true, Url = Url.Action("Entrar") });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Entrar()
        {
            if (Request.Cookies["agendamento"] != null)
                return View();

            return View();
        }

        [HttpGet]
        public ActionResult CadastroPaciente()
        {
            if (Request.Cookies["agendamento"] != null)
                return View();

            return View();
        }

        [HttpPost]
        public ActionResult CadastroPaciente(CadastroPacienteAgendamentoVM model)
        {
            if (!ModelState.IsValid)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsync("Paciente",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("Cpf", model.Cpf),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Telefone", string.IsNullOrWhiteSpace(model.Telefone) ? "" : model.Telefone.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        new KeyValuePair<string, string>("Celular", model.Celular.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        new KeyValuePair<string, string>("Usuario.Senha",AgendaUtils.Criptografia.RetornarMD5(model.Senha)),
                        })).Result;

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    return View(model);
                }

                var agendamento = JsonConvert.DeserializeObject<Models.Agendamento>(Request.Cookies["agendamento"].Value);

                response = client.PostAsync("Agendamento",
                   new FormUrlEncodedContent(new[]
                   {
                        new KeyValuePair<string, string>("IdAgenda", agendamento.IdAgenda.ToString()),
                        new KeyValuePair<string, string>("IdPaciente", response.Content.ReadAsStringAsync().Result),
                        new KeyValuePair<string, string>("Data", agendamento.DataAgendamento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Horario", agendamento.HoraAgendamento.ToString("hh:mm"))
                   })).Result;

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    return View(model);
                }

                var tempModel = new ProtocoloVM()
                {
                    Clinica = "Clinicão",
                    Nome = model.Nome == null ? "teste nome" : model.Nome,
                    DataAgendamento = agendamento.DataAgendamento < new DateTime(1000, 1, 1) ? DateTime.Now : agendamento.DataAgendamento,
                    HoraAgendamento = agendamento.HoraAgendamento == null ? TimeSpan.MaxValue : agendamento.HoraAgendamento,
                    Protocolo = 123
                };
                TempData["protocolo"] = tempModel;
                return RedirectToAction("ProtocoloAgendamento", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ProtocoloAgendamento()
        {
            if (TempData["protocolo"] != null)
                return View((ProtocoloVM)TempData["protocolo"]);

            return RedirectToAction("Index", "Home");
        }
    }


}