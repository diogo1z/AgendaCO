using AgendaDTL;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    public class ListaOdontologistaVM
    {
        [Display(Name = "Nome:")]
        public string Nome { get; set; }
        [Display(Name = "CRO:")]
        public string CRO { get; set; }
        public IEnumerable<Odontologista> Dentistas { get; set; }
    }


    [Validator(typeof(OdontologistaVMValidator))]
    public class OdontologistaVM
    {
        public int Id { get; set; }
        [Display(Name = "Nome:")]
        public string Nome { get; set; }
        [Display(Name = "CPF:")]
        public string Cpf { get; set; }
        [Display(Name = "CRO:")]
        public string Cro { get; set; }
        [Display(Name = "CRO UF:")]
        public int CroEstado { get; set; }
        [Display(Name = "Data de Nascimento:")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Display(Name = "E-mail:")]
        public string Email { get; set; }
        [Display(Name = "Endereço:")]
        public string Endereco { get; set; }
        [Display(Name = "Número:")]
        public string Numero { get; set; }
        [Display(Name = "Complemento:")]
        public string Complemento { get; set; }
        [Display(Name = "Cep:")]
        public string Cep { get; set; }
        public IEnumerable<Telefone> Telefones { get; set; }
    }

    public class OdontologistaVMValidator : AbstractValidator<OdontologistaVM>
    {
        public OdontologistaVMValidator()
        {
            this.RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Informe o nome.")
                .MinimumLength(5).WithMessage("Nome inválido.");

            this.RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Informe o número.")
                .MinimumLength(1).WithMessage("Número inválido.");

            this.RuleFor(x => x.Endereco)
                .NotEmpty().WithMessage("Informe o endereço.")
                .MinimumLength(5).WithMessage("Endereço inválido.");

            this.RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("Informe o cep.")
                .MinimumLength(5).WithMessage("Cep inválido.");

            this.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Informe o e-mail.")
                .EmailAddress().WithMessage("E-mail inválido.");

            this.RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("Informe a data de nascimento.")
                .LessThan(DateTime.Now.AddDays(-10)).WithMessage("Data de nascimento inválida.")
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Data de nascimento inválida.");

            this.RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("Informe o CPF.")
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("CPF inválido.");

            long i = 0;
            this.RuleFor(x => x.Cro)
                .NotEmpty().WithMessage("Informe o CRO.")
                .Must(x => long.TryParse(x, out i)).WithMessage("CRO inválido.")
               .MinimumLength(2).WithMessage("CRO inválido.");

            this.RuleFor(x => x.CroEstado)
                .NotNull().WithMessage("Selecione o UF do CRO.")
                .GreaterThan(0).WithMessage("Selecione o Uf do CRO.");
        }
    }
}