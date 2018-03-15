using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaEnums
{
    public enum DiasSemana
    {

        /// <summary> Domingo: por fundamentação bíblica e etimológica, é considerado o primeiro dia da semana </summary>
        [Description("Domingo")]
        Domingo = 1,

        /// <summary> Segunda-Feira: segundo dia da semana </summary>
        [Description("Segunda-Feira")]
        Segunda = 2,

        /// <summary> Terça-Feira: terceiro dia da semana </summary>
        [Description("Terça-Feira")]
        Terca = 3,

        /// <summary> Quarta-Feira: quarto dia da semana </summary>
        [Description("Quarta-Feira")]
        Quarta = 4,

        /// <summary> Quinta-Feira: quinto dia da semana </summary>
        [Description("Quinta-Feira")]
        Quinta = 5,

        /// <summary> Sexta-Feira: sexto dia da semana </summary>
        [Description("Sexta-Feira")]
        Sexta = 6,

        /// <summary> Sábado: sétimo dia da semana </summary>
        [Description("Sabado")]
        Sábado = 7

    }
}
