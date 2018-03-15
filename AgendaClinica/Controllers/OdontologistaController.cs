using AgendaClinica.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Security;
using System.Security.Claims;
using System.Threading;

namespace AgendaClinica.Controllers
{
    [Authorize]
    public class OdontologistaController : ExtensaoController
    {
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

            var model = new ListaOdontologistaVM();

            model.Dentistas = dentistas;

            return View(model);
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Adicionar(OdontologistaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;


                    var response = client.PostAsync("Odontologista",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("Cpf", model.Cpf),
                        new KeyValuePair<string, string>("Numero", model.Numero),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Endereco", model.Endereco),
                        new KeyValuePair<string, string>("Cep", model.Cep),
                        new KeyValuePair<string, string>("Cro", model.Cro),
                        new KeyValuePair<string, string>("CroEstado", model.CroEstado.ToString()),
                        new KeyValuePair<string, string>("Complemento", model.Complemento)
                        })).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Odontologista Salvo.", "Sucesso!");
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

        public ActionResult Editar(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Odontologista?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault() + "&id=" + id).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var odonto = JsonConvert.DeserializeObject<AgendaDTL.Odontologista>(EmpResponse);

            var model = new OdontologistaVM()
            {
                Cep = odonto.Cep,
                Complemento = odonto.Complemento,
                Cpf = odonto.Cpf,
                Cro = odonto.Cro,
                CroEstado = odonto.CroEstado,
                DataNascimento = odonto.DataNascimento,
                Email = odonto.Email,
                Endereco = odonto.Endereco,
                Id = odonto.Id,
                Nome = odonto.Nome,
                Numero = odonto.Numero
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Editar(OdontologistaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;


                    var response = client.PutAsync("Odontologista",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("IdClinica", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("Cpf", model.Cpf),
                        new KeyValuePair<string, string>("Numero", model.Numero),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Endereco", model.Endereco),
                        new KeyValuePair<string, string>("Cep", model.Cep),
                        new KeyValuePair<string, string>("Cro", model.Cro),
                        new KeyValuePair<string, string>("CroEstado", model.CroEstado.ToString()),
                        new KeyValuePair<string, string>("Complemento", model.Complemento)
                        })).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Odontologista Salvo.", "Sucesso!");
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

        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var response = client.DeleteAsync("Odontologista?idClinica=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault() + "&id=" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Odontologista Excluido.", "Sucesso!");
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