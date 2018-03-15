using AgendaClinica.Models;
using AgendaEnums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AgendaClinica.Controllers
{
    [Authorize]
    public class AgendaController : ExtensaoController
    {
        // GET: Agenda
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

            return View();
        }

        [HttpPost]
        public ActionResult Index(string OdontoId)
        {
            return View();
        }
        [HttpGet]
        [Route("Agenda/Obter/IdOdonto")]
        public ActionResult Obter(int IdOdonto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var model = new AgendaVM();
            model.IdOdontologista = (int)IdOdonto;

            var responseAgenda = client.GetAsync("Agenda?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + IdOdonto)
            .Result;

            var EmpResponseAgenda = responseAgenda.Content.ReadAsStringAsync().Result;

            var agenda = JsonConvert.DeserializeObject<AgendaDTL.Agenda>(EmpResponseAgenda);
            
            model.TempoAtendimento = agenda.TempoAtendimento;

            var response = client.GetAsync("SemanaAgenda?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + IdOdonto)
            .Result;

            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var semanasAgenda = JsonConvert.DeserializeObject<List<AgendaDTL.SemanaAgenda>>(EmpResponse);


            model.AgendaSemana = new List<SemanaAgendaVM>();
            foreach (var semana in semanasAgenda)
            {
                model.AgendaSemana.Add(
                    new SemanaAgendaVM(semana.Id, semana.IdOdontologista, semana.DiaSemana,
                    semana.HorarioAtendimentoInicio, semana.HorarioAtendimentoTermino)
                    );
            }

            response = client.GetAsync("DiaAgenda?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + IdOdonto)
            .Result;

            EmpResponse = response.Content.ReadAsStringAsync().Result;

            var diasAgenda = JsonConvert.DeserializeObject<List<AgendaDTL.DiaAgenda>>(EmpResponse);


            model.AgendaDia = new List<DiaAgendaVM>();
            foreach (var dia in diasAgenda)
            {
                model.AgendaDia.Add(
                    new DiaAgendaVM()
                    {
                        Id = dia.Id,
                        Data = dia.Data,
                        HorarioAtendimentoInicio = dia.HorarioAtendimentoInicio,
                        HorarioAtendimentoTermino = dia.HorarioAtendimentoTermino,
                        IdOdontologista = dia.IdOdontologista
                    }
                );
            }

            response = client.GetAsync("AgendaDayOff?"
           + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
           + "&idOdontologista=" + IdOdonto)
           .Result;

            EmpResponse = response.Content.ReadAsStringAsync().Result;

            var dayoffAgenda = JsonConvert.DeserializeObject<List<AgendaDTL.AgendaDayOff>>(EmpResponse);


            model.AgendaDayOff = new List<DayOffVM>();
            foreach (var dia in dayoffAgenda)
            {
                model.AgendaDayOff.Add(
                    new DayOffVM()
                    {
                        Id = dia.Id,
                        Data = dia.Data,
                        IdOdontologista = dia.IdOdontologista
                    }
                );
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Obter(AgendaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    
                    var response = client.PutAsync("Agenda",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("TempoAtendimento", model.TempoAtendimento.ToString())                        
                        })).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda salva.", "Sucesso!");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    return View(model);
                }
            }
            return View();
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult ExibirSemana()
        {
            return View();
        }

        [HttpGet]
        [Route("Agenda/AdicionarSemana/idOdontologista")]
        public ActionResult AdicionarSemana(int idOdontologista)
        {
            var model = new SemanaAgendaVM() { IdOdontologista = idOdontologista };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AdicionarSemana(SemanaAgendaVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    List<int> listaDias = new List<int>();
                    if (model.Domingo)
                        listaDias.Add(1);
                    if (model.Segunda)
                        listaDias.Add(2);
                    if (model.Terca)
                        listaDias.Add(3);
                    if (model.Quarta)
                        listaDias.Add(4);
                    if (model.Quinta)
                        listaDias.Add(5);
                    if (model.Sexta)
                        listaDias.Add(6);
                    if (model.Sabado)
                        listaDias.Add(7);

                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    bool temErros = false;
                    string msgErro = "";
                    foreach (var dia in listaDias)
                    {
                        var response = client.PostAsync("SemanaAgenda",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoInicio", model.HorarioAtendimentoInicio.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoTermino", model.HorarioAtendimentoTermino.ToString()),
                        new KeyValuePair<string, string>("DiaSemana", dia.ToString())
                        })).Result;
                        temErros = !temErros ? !response.IsSuccessStatusCode : temErros;
                        if (!response.IsSuccessStatusCode)
                            //msgErro += Enum.GetName(typeof(DiasSemana),dia).ToString() + ": " + response.Content.ReadAsStringAsync().Result + "  ";
                            ModelState.AddModelError(string.Empty, Enum.GetName(typeof(DiasSemana), dia).ToString() + ": " + response.Content.ReadAsStringAsync().Result);
                    }

                    if (!temErros)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;                        
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, msgErro);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("AdicionarSemana", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("AdicionarSemana", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("AdicionarSemana", model);
            return Json(r);
        }

        [HttpGet]
        [Route("Agenda/AdicionarSemana/idOdontologista/id")]
        public ActionResult EditarSemana(int idOdontologista, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("SemanaAgenda?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + idOdontologista + "&id=" + id)
            .Result;

            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var semana = JsonConvert.DeserializeObject<AgendaDTL.SemanaAgenda>(EmpResponse);

            var model = new SemanaAgendaVM(semana.Id, semana.IdOdontologista, semana.DiaSemana,
                    semana.HorarioAtendimentoInicio, semana.HorarioAtendimentoTermino);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditarSemana(SemanaAgendaVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    int dia = 0;

                    if (model.Domingo)
                        dia = 1;
                    if (model.Segunda)
                        dia = 2;
                    if (model.Terca)
                        dia = 3;
                    if (model.Quarta)
                        dia = 4;
                    if (model.Quinta)
                        dia = 5;
                    if (model.Sexta)
                        dia = 6;
                    if (model.Sabado)
                        dia = 7;

                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    var response = client.PutAsync("SemanaAgenda",
                    new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoInicio", model.HorarioAtendimentoInicio.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoTermino", model.HorarioAtendimentoTermino.ToString()),
                        new KeyValuePair<string, string>("DiaSemana", dia.ToString())
                    })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;                        
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("EditarSemana", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("EditarSemana", model);
                    return Json(r);                    
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("EditarSemana", model);
            return Json(r);
        }

        [HttpPost]
        public ActionResult ExcluirSemana(int id, int idOdontologista)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.DeleteAsync("SemanaAgenda?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
                    + "&id=" + id)
                    .Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Agenda Excluida.", "Sucesso!");
                    return RedirectToAction("Obter", new { IdOdonto = idOdontologista });
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
        [Route("Agenda/AdicionarDia/idOdontologista")]
        public ActionResult AdicionarDia(int idOdontologista)
        {
            var model = new DiaAgendaVM() { IdOdontologista = idOdontologista };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult AdicionarDia(DiaAgendaVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    var response = client.PostAsync("DiaAgenda",
                    new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoInicio", model.HorarioAtendimentoInicio.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoTermino", model.HorarioAtendimentoTermino.ToString()),
                        new KeyValuePair<string, string>("Data", model.Data.ToString("yyyy-MM-dd"))
                        })).Result;
                    
                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("AdicionarDia", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("AdicionarDia", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("AdicionarDia", model);
            return Json(r);
        }

        [HttpGet]
        [Route("Agenda/EditarDia/idOdontologista/id")]
        public ActionResult EditarDia(int idOdontologista, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("DiaAgenda?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + idOdontologista + "&id=" + id)
            .Result;

            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var dia = JsonConvert.DeserializeObject<AgendaDTL.DiaAgenda>(EmpResponse);

            var model = new DiaAgendaVM()
            {
                Id = dia.Id,
                Data = dia.Data,
                HorarioAtendimentoInicio = dia.HorarioAtendimentoInicio,
                HorarioAtendimentoTermino = dia.HorarioAtendimentoTermino,
                IdOdontologista = dia.IdOdontologista
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditarDia(DiaAgendaVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    var response = client.PutAsync("DiaAgenda",
                    new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoInicio", model.HorarioAtendimentoInicio.ToString()),
                        new KeyValuePair<string, string>("HorarioAtendimentoTermino", model.HorarioAtendimentoTermino.ToString()),
                        new KeyValuePair<string, string>("Data", model.Data.ToString("yyyy-MM-dd"))
                    })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;                        
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("EditarDia", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("EditarDia", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("EditarDia", model);
            return Json(r);
        }

        [HttpPost]
        public ActionResult ExcluirDia(int id, int idOdontologista)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.DeleteAsync("DiaAgenda?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
                    + "&id=" + id)
                    .Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Agenda Excluida.", "Sucesso!");
                    return RedirectToAction("Obter", new { IdOdonto = idOdontologista });
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

        public ActionResult AdicionarDayOff()
        {
            return View();
        }

        [HttpGet]
        [Route("Agenda/AdicionarDayOff/idOdontologista")]
        public ActionResult AdicionarDayOff(int idOdontologista)
        {
            var model = new DayOffVM() { IdOdontologista = idOdontologista };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AdicionarDayOff(DayOffVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    var response = client.PostAsync("AgendaDayOff",
                    new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("Data", model.Data.ToString("yyyy-MM-dd"))
                        })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("AdicionarDayOff", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("AdicionarDayOff", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("AdicionarDayOff", model);
            return Json(r);
        }
        

        [HttpGet]
        [Route("Agenda/EditarDayOff/idOdontologista/id")]
        public ActionResult EditarDayOff(int idOdontologista, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("AgendaDayOff?"
            + "idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
            + "&idOdontologista=" + idOdontologista + "&id=" + id)
            .Result;

            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var dia = JsonConvert.DeserializeObject<AgendaDTL.AgendaDayOff>(EmpResponse);

            var model = new DayOffVM()
            {
                Id = dia.Id,
                Data = dia.Data,
                IdOdontologista = dia.IdOdontologista
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditarDayOff(DayOffVM model)
        {
            var r = new ReturnArgs();
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    var response = client.PutAsync("AgendaDayOff",
                    new FormUrlEncodedContent(new[]
                    {   
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdOdontologista", model.IdOdontologista.ToString()),
                        new KeyValuePair<string, string>("Data", model.Data.ToString("yyyy-MM-dd"))
                    })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Agenda Salva.", "Sucesso!");
                        r.Status = 1;                        
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("EditarDayOff", model);                        
                        return Json(r);                        
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("EditarDayOff", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("EditarDayOff", model);
            return Json(r);
        }

        [HttpPost]
        public ActionResult ExcluirDayOff(int id, int idOdontologista)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.DeleteAsync("AgendaDayOff?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()
                    + "&id=" + id)
                    .Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Agenda Excluida.", "Sucesso!");                    
                    return RedirectToAction("Obter", new { IdOdonto = idOdontologista });
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