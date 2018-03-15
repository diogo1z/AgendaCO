using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    public class AcessoViewModel
    {
        [Required(ErrorMessage = "Informe o usuário.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public string Perfil { get; set; }        
    }
}