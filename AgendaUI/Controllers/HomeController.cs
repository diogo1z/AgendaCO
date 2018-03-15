using AgendaDTL;
using AgendaUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["URLAPI"].ToString());
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
            return View();
        }

        [HttpPost]
        public ActionResult DataClinica()
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("CadastroPaciente", "Home", new { idClinica = 1, dataAgendamento = DateTime.Now, horaAgendamento = TimeSpan.FromHours(2) });
            }
            return View();
        }

        [HttpGet]
        public ActionResult CadastroPaciente(int idClinica, DateTime dataAgendamento, TimeSpan horaAgendamento)
        {
            var model = new Models.CadastroPacienteAgendamentoVM();
            model.IdClinica = idClinica;
            model.DataAgendamento = dataAgendamento;
            model.HoraAgendamento = horaAgendamento;
            return View(model);
        }

        [HttpPost]
        public ActionResult CadastroPaciente(CadastroPacienteAgendamentoVM model)
        {
            if (!ModelState.IsValid)
            {
                var tempModel = new ProtocoloVM()
                {
                    Clinica = "Clinicão",
                    Nome = model.Nome == null ? "teste nome" : model.Nome,
                    DataAgendamento = model.DataAgendamento < new DateTime(1000,1,1) ? DateTime.Now: model.DataAgendamento,
                    HoraAgendamento = model.HoraAgendamento == null ? TimeSpan.MaxValue : model.HoraAgendamento, 
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