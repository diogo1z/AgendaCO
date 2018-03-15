using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    public class AgendamentoVM
    {
        public int IdAgenda { get; set; }
        public int IdOdonto { get; set; }
        public string NomeOdontologista { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan HoraAgendamento { get; set; }
        public int IdPaciente { get; set; }
        public string NomePaciente { get; set; }
        public IEnumerable<AgendaDTL.Odontologista> Dentistas { get; set; }
    }

    public class AgendasOdontoVM
    {
        List<TimeSpan> Horarios { get; set; }
        public int IdOdonto { get; set; }
        public string NomeOdontologista { get; set; }
        public DateTime DataAgendamento { get; set; }
    }

    

}