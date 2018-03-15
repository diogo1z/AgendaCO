using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    [Validator(typeof(PacienteVMValidator))]
    public class PacienteVM
    {
        public int Id { get; set; }
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
        public Dependente Dependente { get; set; }
        public List<Dependente> Dependentes { get; set; }
    }

    public class PacienteVMValidator : AbstractValidator<PacienteVM>
    {
        public PacienteVMValidator()
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
                .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage("Celular inválido.");

            this.RuleFor(x => x.Telefone)
                .Matches(@"^\(\d{2}\) \d{4}-\d{4}$").WithMessage("Telefone inválido.");
            
            this.RuleFor(x => x.Dependente.Nome)
                .MinimumLength(5).WithMessage("Nome inválido.");
        }
    }

    public class Dependente
    {
        [DisplayName("Nome dependente:")]
        public string Nome { get; set; }
        [DisplayName("Data de nascimento:")]
        public DateTime? DataNascimento { get; set; }
        public int Id { get; set; }
        public int IdPaciente { get; set; }
    }

    public class BuscarPacientesVM
    {
        public int IdPaciente { get; set; }
        public string Cpf { get; set; }
        public List<PacienteVM> Pacientes { get; set; }
    }
}