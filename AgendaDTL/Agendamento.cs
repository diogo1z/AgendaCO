using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdAgenda { get; set; }
        public int IdOdonto { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Horario { get; set; }
    }

    public class OdontologistaAgendamento
    {
        public IEnumerable<Agendamento> Agendamentos { get; set; }
        public Odontologista Odontologista { get; set; }
    }

    public class DetalhesAgendamento
    {
        public int Id { get; set; }
        public Paciente Paciente { get; set; }
        public Clinica Clinica { get; set; }
        public Odontologista Odontologista { get; set; }
    }


}
