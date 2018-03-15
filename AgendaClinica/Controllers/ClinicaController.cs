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

    public class ClinicaController : ExtensaoController
    {
        [Authorize]
        [HttpGet]
        public ActionResult Editar()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var response = client.GetAsync("Clinica?id=" + identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()).Result;
            var EmpResponse = response.Content.ReadAsStringAsync().Result;

            //Deserializing the response recieved from web api and storing into the Employee list  
            var clinica = JsonConvert.DeserializeObject<AgendaDTL.Clinica>(EmpResponse);

            var model = new ClinicaVM()
            {
                CepLogradouro = clinica.Endereco.CepLogradouro,
                ComplementoLogradouro = clinica.Endereco.ComplementoLogradouro,
                Cnpj = clinica.Cnpj,
                Email = clinica.Email,
                Bairro = clinica.Endereco.Bairro,
                Cidade = clinica.Endereco.Cidade,
                Logradouro = clinica.Endereco.Logradouro,
                NumeroLogradouro = clinica.Endereco.NumeroLogradouro,
                Id = clinica.Id,
                NomeFantasia = clinica.NomeFantasia,
                RazaoSocial = clinica.RazaoSocial,
                IdBairro = clinica.Endereco.IdBairro,
                IdCidade = clinica.Endereco.IdCidade,
                IdEstado = clinica.Endereco.IdEstado
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(ClinicaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;


                    var response = client.PutAsync("Clinica",
                        new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("Id", model.Id.ToString()),
                        new KeyValuePair<string, string>("Endereco.CepLogradouro", model.CepLogradouro),
                        new KeyValuePair<string, string>("Endereco.ComplementoLogradouro", model.ComplementoLogradouro),
                        new KeyValuePair<string, string>("Cnpj", model.Cnpj),
                        new KeyValuePair<string, string>("Email", model.Email),
                        new KeyValuePair<string, string>("Endereco.Bairro", model.Bairro),
                        new KeyValuePair<string, string>("Endereco.Cidade", model.Cidade),
                        new KeyValuePair<string, string>("Endereco.Estado", model.Estado),
                        new KeyValuePair<string, string>("Endereco.Logradouro", model.Logradouro),
                        new KeyValuePair<string, string>("Endereco.NumeroLogradouro", model.NumeroLogradouro),
                        new KeyValuePair<string, string>("NomeFantasia", model.NomeFantasia),
                        new KeyValuePair<string, string>("RazaoSocial", model.RazaoSocial)
                        })).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        this.ShowMessage("Clínica Salva.", "Sucesso!");
                        return RedirectToAction("Index", "Home");
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

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(ClinicaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;


                    var response = client.PostAsync("Clinica",
                        new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("Cnpj", model.Cnpj),
                            new KeyValuePair<string, string>("Email", model.Email),
                            new KeyValuePair<string, string>("NomeFantasia", model.NomeFantasia),
                            new KeyValuePair<string, string>("RazaoSocial", model.RazaoSocial),
                            new KeyValuePair<string, string>("Endereco.CepLogradouro", model.CepLogradouro),
                            new KeyValuePair<string, string>("Endereco.ComplementoLogradouro", model.ComplementoLogradouro),
                            new KeyValuePair<string, string>("Endereco.Bairro", model.Bairro),
                            new KeyValuePair<string, string>("Endereco.Cidade", model.Cidade),
                            new KeyValuePair<string, string>("Endereco.Estado", model.Estado),
                            new KeyValuePair<string, string>("Endereco.Logradouro", model.Logradouro),
                            new KeyValuePair<string, string>("Endereco.NumeroLogradouro", model.NumeroLogradouro),
                            new KeyValuePair<string, string>("Endereco.IdBairro", model.IdBairro.ToString()),
                            new KeyValuePair<string, string>("Endereco.IdCidade", model.IdCidade.ToString()),
                            new KeyValuePair<string, string>("Endereco.IdEstado", model.IdEstado.ToString()),
                            new KeyValuePair<string, string>("Usuario.Senha", AgendaUtils.Criptografia.RetornarMD5(model.Senha)),
                            new KeyValuePair<string, string>("Usuario.Login", model.Email)
                        })).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
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
    }
}