using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaUI.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Informe o usuário.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public Agendamento Agendamento { get; set; }
    }


    [Validator(typeof(CadastroPacienteAgendamentoVMValidator))]
    public class CadastroPacienteAgendamentoVM
    {
        public Agendamento Agendamento { get; set; }

        [DisplayName("Nome:")]
        public string Nome { get; set; }
        [DisplayName("CPF:")]
        public string Cpf { get; set; }
        [DisplayName("Data de nascimento:")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [DisplayName("Email:")]
        public string Email { get; set; }
        [DisplayName("Telefone:")]
        public string Telefone { get; set; }
        [DisplayName("Celular:")]
        public string Celular { get; set; }
        [DisplayName("Senha:")]
        public string Senha { get; set; }
        [DisplayName("Confirme sua senha:")]
        public string ConfirmacaoSenha { get; set; }

        public List<Dependente> Dependentes { get; set; }

    }

    public class CadastroPacienteAgendamentoVMValidator : AbstractValidator<CadastroPacienteAgendamentoVM>
    {
        public CadastroPacienteAgendamentoVMValidator()
        {
            this.RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Informe o nome.")
                .MinimumLength(5).WithMessage("Nome inválido.");

            this.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Informe o e-mail.")
                .EmailAddress().WithMessage("E-mail inválido.");

            this.RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("Informe a data de nascimento.")
                .LessThan(DateTime.Now.AddDays(-10)).WithMessage("Data de nascimento inválida.")
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Data de nascimento inválida.");

            this.RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("Informe o cpf.")
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("Cpf inválido.");

            this.RuleFor(x => x.Celular)
                .NotEmpty().WithMessage("Informe o celular.")
                .Matches(@"^\(\d{2}\)\d{5}-\d{4}$").WithMessage("Celular inválido.");

            this.RuleFor(x => x.Telefone)                
                .Matches(@"^\(\d{2}\)\d{4}-\d{4}$").WithMessage("Telefone inválido.");

            this.RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Informe a senha.");

            this.RuleFor(x => x.ConfirmacaoSenha)
                .NotEmpty().WithMessage("Confirme a senha.")
                .Equal(x => x.Senha)
                .WithMessage("As senhas não conferem.");
        }
    }

    public class Dependente
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
    }

    public class Agendamento
    {
        public int IdClinica { get; set; }
        public int IdOdonto { get; set; }
        public int IdAgenda { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAgendamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HoraAgendamento { get; set; }
    }

}