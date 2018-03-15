using AgendaClinica.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace AgendaClinica.Controllers
{
    [Authorize]
    public class PacienteController : ExtensaoController
    {
        // GET: Paciente
        public ActionResult Index()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Paciente?idClinica=" + identity.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var pacientes = JsonConvert.DeserializeObject<List<AgendaDTL.Paciente>>(EmpResponse);

            var model = new List<PacienteVM>();

            foreach (var paciente in pacientes)
            {
                model.Add(new PacienteVM()
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

        public ActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Adicionar(PacienteVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Dependente != null)
                        if (model.Dependente.DataNascimento != null)
                            if (model.Dependente.DataNascimento >= DateTime.Now.AddMonths(-6) || model.Dependente.DataNascimento <= new DateTime(1900, 1, 1))
                            {
                                ModelState.AddModelError(string.Empty, "Data de nascimento do dependente está inválida.");
                                return View(model);
                            }
                            else
                                if (string.IsNullOrWhiteSpace(model.Dependente.Nome))
                            {
                                ModelState.AddModelError(string.Empty, "Informe o nome do dependente.");
                                return View(model);
                            }

                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    var response = client.PostAsync("Paciente",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("IdClinicaDeCadastro", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("Cpf", model.Cpf),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Telefone", string.IsNullOrWhiteSpace(model.Telefone) ? "" : model.Telefone.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        new KeyValuePair<string, string>("Celular", model.Celular.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        })).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        return View(model);
                    }

                    int idPaciente = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);

                    if (model.Dependente.Nome != null && model.Dependente.DataNascimento != null)
                    {
                        response = client.PostAsync("DependentePaciente",
                            new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("IdClinicaDeCadastro", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("IdPaciente", idPaciente.ToString()),
                        new KeyValuePair<string, string>("Nome", model.Dependente.Nome),
                        new KeyValuePair<string, string>("DataNascimento", model.Dependente.DataNascimento.Value.ToString("yyyy-MM-dd"))
                            })).Result;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Paciente Salvo.", "Sucesso!");
                        return RedirectToAction("Editar", new { idPaciente = idPaciente });
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

        public ActionResult Editar(int idPaciente)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Paciente?id=" + idPaciente
                + "&idClinica=" + identity.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()

                ).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var paciente = JsonConvert.DeserializeObject<AgendaDTL.Paciente>(EmpResponse);

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

            response = client.GetAsync("DependentePaciente?idPaciente=" + idPaciente).Result;
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

        [HttpPost]
        public ActionResult Editar(PacienteVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    var response = client.PutAsync("Paciente",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("IdClinicaDeCadastro", identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("Cpf", model.Cpf),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("Telefone", string.IsNullOrWhiteSpace(model.Telefone) ? null : model.Telefone.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        new KeyValuePair<string, string>("Celular", model.Celular.Replace("(","").Replace(")","").Replace(" ","").Replace("-","")),
                        })).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        RedirectToAction("Index");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Paciente Salvo.", "Sucesso!");
                        return View(model);
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
            return View(model);
        }

        [HttpGet]
        [Route("Paciente/EditarDependente/id")]
        public ActionResult EditarDependente(int idPaciente, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("DependentePaciente?idPaciente=" + idPaciente + "&id=" + id).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            var dependente = JsonConvert.DeserializeObject<AgendaDTL.DependentePaciente>(EmpResponse);

            var model = new Dependente()
            {
                Id = dependente.Id,
                DataNascimento = dependente.DataNascimento,
                //Celular = dependente.Celular,
                Nome = dependente.Nome,
                //Telefone = dependente.Telefone
                IdPaciente = dependente.IdPaciente
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditarDependente(Dependente model)
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

                    var response = client.PutAsync("DependentePaciente",
                            new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("IdPaciente", model.IdPaciente.ToString()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.Value.ToString("yyyy-MM-dd"))
                            })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Dependente Salvo.", "Sucesso!");
                        r.Status = 1;
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("EditarDependente", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("EditarDependente", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("EditarDependente", model);
            return Json(r);
        }

        [HttpGet]
        [Route("Paciente/AdicionarDependente/id")]
        public ActionResult AdicionarDependente(int idPaciente)
        {
            var model = new Dependente()
            {
                IdPaciente = idPaciente
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AdicionarDependente(Dependente model)
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

                    var response = client.PostAsync("DependentePaciente",
                            new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("IdPaciente", model.IdPaciente.ToString()),
                        new KeyValuePair<string, string>("Nome", model.Nome),
                        new KeyValuePair<string, string>("DataNascimento", model.DataNascimento.Value.ToString("yyyy-MM-dd"))
                            })).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Dependente Salvo.", "Sucesso!");
                        r.Status = 1;
                        return Json(r);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                        r.Status = -1;
                        r.View = this.RenderRazorViewToString("AdicionarDependente", model);
                        return Json(r);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex);
                    r.Status = -1;
                    r.View = this.RenderRazorViewToString("AdicionarDependente", model);
                    return Json(r);
                }
            }
            r.Status = -1;
            r.View = this.RenderRazorViewToString("AdicionarDependente", model);
            return Json(r);
        }

        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.DeleteAsync("Paciente?id=" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Paciente Excluido.", "Sucesso!");
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

        [HttpPost]
        public ActionResult ExcluirDependente(int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.DeleteAsync("DependentePaciente?id=" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    this.ShowMessage("Paciente Excluido.", "Sucesso!");
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
