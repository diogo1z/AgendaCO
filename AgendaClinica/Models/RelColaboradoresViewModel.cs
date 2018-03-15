using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    public class RelColaboradoresViewModel
    {

        [Display(Name = "Data de Início")]
        /*[DataType(DataType.Date)]*/
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        //[DataType(DataType.Date)]
        public DateTime DataTermino { get; set; }
        public IEnumerable<ColaboradorViewModel> ListaColaboradores { get; set; }
    }

    public class ColaboradorViewModel
    {
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }

    }
}