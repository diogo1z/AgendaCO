using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AgendaUI.Models
{
    public class ProtocoloVM
    {
        [DisplayName("Nome:")]
        public string Nome { get; set; }
        [DisplayName("Clinica:")]
        public string Clinica { get; set; }
        [DisplayName("Data agendada:")]
        public DateTime DataAgendamento { get; set; }
        [DisplayName("Horário agendado:")]
        public TimeSpan HoraAgendamento { get; set; }
        [DisplayName("Protocolo:")]
        public int Protocolo { get; set; }
    }
}