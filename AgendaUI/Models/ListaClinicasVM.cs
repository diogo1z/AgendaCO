using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaUI.Models
{
    public class ListaClinicasVM
    {
        public IEnumerable<AgendaDTL.Clinica> ListaClinicas { get; set; }
    }


}