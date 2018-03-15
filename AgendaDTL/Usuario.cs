using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDTL
{
    public class Usuario : Perfil
    {   
        public string Login { get; set; }
        public string Senha { get; set; }
        public int idClinica { get; set; }
        public int idPaciente { get; set; }
        public int idOdontologista { get; set; }
        public Perfil Perfil { get; set; }
    }
}
