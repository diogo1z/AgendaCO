using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaEnums
{
    [Flags]
    public enum TipoTelefone
    {
        Telefone = 1,
        Celular = 2,
        Fax = 3,
    };

}
