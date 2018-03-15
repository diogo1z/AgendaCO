using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Agenda
    {
        public Agenda()
        {

        }
        public Agenda(int idOdontologista, int idClinica, int tempoAtendimento)
        {
            TempoAtendimento = tempoAtendimento;
            IdOdontologista = idOdontologista;
            IdClinica = idClinica;
        }
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        public int IdClinica { get; set; }
        public int TempoAtendimento { get; set; }        
    }
    public class SemanaAgenda
    {
        public int IdAgenda { get; set; }
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        public int IdClinica { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan HorarioAtendimentoInicio { get; set; }
        public TimeSpan HorarioAtendimentoTermino { get; set; }
    }
    public class DiaAgenda
    {
        public int IdAgenda { get; set; }
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        public int IdClinica { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HorarioAtendimentoInicio { get; set; }
        public TimeSpan HorarioAtendimentoTermino { get; set; }
    }

    public class AgendaDayOff
    {
        public int IdAgenda { get; set; }
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        public int IdClinica { get; set; }
        public DateTime Data { get; set; }
    }
}
