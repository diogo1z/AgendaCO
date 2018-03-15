using AgendaClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgendaClinica.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RelColaboradores()
        {
            var model = new RelColaboradoresViewModel();
            model.DataInicio = DateTime.Now.AddMonths(-12);
            model.DataTermino = DateTime.Now;
            var list = new List<ColaboradorViewModel>();
            for (int i = 0; i < 25; i++)
            {
                list.Add(new ColaboradorViewModel() { Matricula = i, Nome = DateTime.Now.ToString("fff") + " Nome", Data = DateTime.Now.AddDays(i) });
            }
            model.ListaColaboradores = list;

            return View(model);
        }

        public ActionResult DetalhesRelColaboradores(int matricula)
        {
            var model = new ColaboradorViewModel()
            { Matricula = matricula, Data = DateTime.Now, Nome = "Teste Detalhes Colaborador Nome" };
            return View(model);
        }

        //public ActionResult RelColaboradores(RelColaboradoresViewModel model)
        //{
        //    return View();
        //}
    }
}