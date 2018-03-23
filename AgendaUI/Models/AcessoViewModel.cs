using System.ComponentModel.DataAnnotations;

namespace AgendaUI.Models
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