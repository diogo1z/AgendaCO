using AgendaPaciente.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AgendaPaciente.Controllers
{
    public class AccountController : Controller
    {
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
        public ActionResult Login(AcessoViewModel login, string returnUrl)
        {
            //if (ModelState.IsValid)
            //{
            //    //var usuario = new Dto.Usuario();
            //    //usuario.Login = login.Usuario;
            //    try
            //    {                    
            //        //using (var repositorio = new UsuarioRepositorio(new Connection(new System.Data.SqlClient.SqlConnection())))
            //        //{
            //        //    usuario = repositorio.Obter(usuario);
            //        //}
            //    }
            //    catch(Exception ex)
            //    {
            //        ModelState.AddModelError(string.Empty, "Não foi possível validar o usuário, tente novamente.");
            //        return View(login);
            //    }
                
            //    /*Verificar se a variavel usuario está vazia. Isso pode ocorrer caso o usuário não existe. 
            //    Caso não exista ele vai cair na condição else.*/
            //    if (usuario != null)
            //    {
            //        /*Código abaixo verifica se o usuário que retornou na variavel tem está 
            //          ativo. Caso não esteja cai direto no else*/
            //        if (usuario.Ativo)
            //        {
            //            /*Código abaixo verifica se a senha digitada no site é igual a senha que está sendo retornada 
            //             do banco. Caso não cai direto no else*/
            //            if (Utils.Criptografia.ComparaMD5(login.Senha, usuario.Senha))
            //            {
            //                FormsAuthentication.SetAuthCookie(login.Usuario, false);

            //                if (Url.IsLocalUrl(returnUrl)
            //                && returnUrl.Length > 1
            //                && returnUrl.StartsWith("/")
            //                && !returnUrl.StartsWith("//")
            //                && returnUrl.StartsWith("/\\"))
            //                {
            //                    return Redirect(returnUrl);
            //                }
            //                /*código abaixo cria uma session para armazenar o nome do usuário*/
            //                //Session["Usuario"] = "AINDA NAO";                            
            //                /*retorna para a tela inicial do Home*/
            //                return RedirectToAction("Index", "Home");
            //            }
            //            /*Else responsável da validação da senha*/
            //            else
            //            {
            //                /*Escreve na tela a mensagem de erro informada*/
            //                ModelState.AddModelError("", "Senha inválida.");
            //                /*Retorna a tela de login*/
            //                return View(new AcessoViewModel());
            //            }
            //        }
            //        /*Else responsável por verificar se o usuário está ativo*/
            //        else
            //        {
            //            /*Escreve na tela a mensagem de erro informada*/
            //            ModelState.AddModelError("", "Usuário sem acesso.");
            //            /*Retorna a tela de login*/
            //            return View(new AcessoViewModel());
            //        }
            //    }
            //    /*Else responsável por verificar se o usuário existe*/
            //    else
            //    {
            //        /*Escreve na tela a mensagem de erro informada*/
            //        ModelState.AddModelError("", "Usuário inválido.");
            //        /*Retorna a tela de login*/
            //        return View(new AcessoViewModel());
            //    }
            //}
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login com as mensagem dos campos*/
            return View(login);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login", "Account", null);
        }
    }
}