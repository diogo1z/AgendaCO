using AgendaClinica.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace AgendaClinica.Controllers
{
    //Permite que esta controller seja acessada por usuários anonimos
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IAuthenticationManager _authenticationManager;

        public AccountController()
        {
            _authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult Login(string returnURL)
        {
            /*Recebe a url que o usuário tentou acessar*/
            ViewBag.ReturnUrl = returnURL;
            return View(new AcessoViewModel());
        }

        /// <param name="login"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AcessoViewModel usuario, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["service:ApiAddress"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("Usuario?login=" + usuario.Login +
                        "&senha=" + AgendaUtils.Criptografia.RetornarMD5(usuario.Senha) +
                        "&tipoAcesso=2").Result;

                    /*Verificar se a variavel usuario está vazia. Isso pode ocorrer caso o usuário não existe. 
                    Caso não exista ele vai cair na condição else.*/
                    if (response.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = response.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        var acesso = JsonConvert.DeserializeObject<AgendaDTL.Usuario>(EmpResponse);

                        /*Código abaixo verifica se a senha digitada no site é igual a senha que está sendo retornada 
                         do banco. Caso não cai direto no else*/
                        if (acesso != null)
                        {
                            
                            //var claimsIdentity = new ClaimsIdentity(
                            //    new[] { new Claim(ClaimTypes.Name, acesso.Login) },
                            //    DefaultAuthenticationTypes.ApplicationCookie);

                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, acesso.Login));
                            claims.Add(new Claim(ClaimTypes.Sid, acesso.idClinica.ToString(), ClaimValueTypes.Integer));
                            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

                            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                            

                            _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

                            if (Url.IsLocalUrl(returnUrl)
                            && returnUrl.Length > 1
                            && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//")
                            && returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index", "Home");
                        }
                        /*Else responsável da validação da senha*/
                        else
                        {
                            /*Escreve na tela a mensagem de erro informada*/
                            ModelState.AddModelError("", "Usuário ou senha inválidos.");
                            /*Retorna a tela de login*/
                            return View(new AcessoViewModel());
                        }

                    }
                    /*Else responsável por verificar se o usuário existe*/
                    else
                    {
                        /*Escreve na tela a mensagem de erro informada*/
                        ModelState.AddModelError("", "Usuário ou senha inválidos.");
                        /*Retorna a tela de login*/
                        return View(new AcessoViewModel());
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Não foi possível validar o usuário, tente novamente.");
                    return View(usuario);
                }
            }
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login com as mensagem dos campos*/
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        
    }
}