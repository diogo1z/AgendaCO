using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{

    [Validator(typeof(ClinicaVMValidator))]
    public class ClinicaVM
    {
        public int Id { get; set; }
        [Display(Name = "Razão Social:")]
        public string RazaoSocial { get; set; }
        [Display(Name = "Nome Fantasia:")]
        public string NomeFantasia { get; set; }
        [Display(Name = "Cnpj:")]
        public string Cnpj { get; set; }
        [Display(Name = "E-mail:")]
        public string Email { get; set; }
        [Display(Name = "Logradouro:")]
        public string Logradouro { get; set; }
        [Display(Name = "Número do Logradouro:")]
        public string NumeroLogradouro { get; set; }
        [Display(Name = "Complemento do Logradouro:")]
        public string ComplementoLogradouro { get; set; }
        [Display(Name = "Cep do Logradouro:")]
        public string CepLogradouro { get; set; }
        [Display(Name = "Cidade:")]
        public string Cidade { get; set; }
        [Display(Name = "Estado:")]
        public string Estado { get; set; }
        [Display(Name = "Bairro:")]
        public string Bairro { get; set; }
        public long IdBairro { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }
        [Display(Name = "Senha:")]
        public string Senha { get; set; }
        [Display(Name = "Confirme sua senha:")]
        public string ConfirmacaoSenha { get; set; }
    }

    public class ClinicaVMValidator : AbstractValidator<ClinicaVM>
    {
        public ClinicaVMValidator()
        {
            this.RuleFor(x => x.NomeFantasia)
                .NotEmpty().WithMessage("Informe o nome fantasia.")
                .MinimumLength(5).WithMessage("Nome fantasia inválido.");

            this.RuleFor(x => x.RazaoSocial)
                .NotEmpty().WithMessage("Informe a razão social.")
                .MinimumLength(5).WithMessage("Razão social inválido.");

            this.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Informe o e-mail.")
                .EmailAddress().WithMessage("E-mail inválido.");

            this.RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("Informe o cnpj.")
                .Matches(@"^\d{2}.\d{3}.\d{3}/\d{4}-\d{2}$").WithMessage("CPF inválido.");
            
            this.RuleFor(x => x.CepLogradouro)
                 .NotEmpty().WithMessage("Informe o cep.")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("Cep inválido.");

            this.RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("Informe o logradouro.")
                .MinimumLength(5).WithMessage("Logradouro inválido.");

            this.RuleFor(x => x.NumeroLogradouro)
                .NotEmpty().WithMessage("Informe o número do logradouro.")
                .MinimumLength(1).WithMessage("Númeo logradouro inválido.");

            this.RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Informe a senha.");

            this.RuleFor(x => x.ConfirmacaoSenha)
                .NotEmpty().WithMessage("Confirme a senha.");

        }
    }
}