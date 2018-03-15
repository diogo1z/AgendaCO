using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgendaClinica.Models
{
    public class AgendaVM
    {
        public int IdOdontologista { get; set; }
        [Display(Name = "Tempo de Atendimento:")]
        public int TempoAtendimento { get; set; }
        //public List<DateTime> DayOff { get; set; }
        public List<DayOffVM> AgendaDayOff { get; set; }
        public List<DiaAgendaVM> AgendaDia { get; set; }
        public List<SemanaAgendaVM> AgendaSemana { get; set; }
    }

    [Validator(typeof(SemanaAgendaVMValidator))]
    public class SemanaAgendaVM
    {
        public SemanaAgendaVM()
        {

        }
        public SemanaAgendaVM(int id, int idOdontologista, int diaSemana, TimeSpan horarioAtendimentoInicio, TimeSpan horarioAtendimentoTermino)
        {
            Id = id;
            IdOdontologista = idOdontologista;
            HorarioAtendimentoInicio = horarioAtendimentoInicio;
            HorarioAtendimentoTermino = horarioAtendimentoTermino;

            if (diaSemana == 1)
                Domingo = true;
            else if (diaSemana == 2)
                Segunda = true;
            else if (diaSemana == 3)
                Terca = true;
            else if (diaSemana == 4)
                Quarta = true;
            else if (diaSemana == 5)
                Quinta = true;
            else if (diaSemana == 6)
                Sexta = true;
            else if (diaSemana == 7)
                Sabado = true;
        }
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        [Display(Name = "Domingo:")]
        public bool Domingo { get; set; }
        [Display(Name = "Segunda-Feira:")]
        public bool Segunda { get; set; }
        [Display(Name = "Terça-Feira:")]
        public bool Terca { get; set; }
        [Display(Name = "Quarta-Feira:")]
        public bool Quarta { get; set; }
        [Display(Name = "Quinta-Feira:")]
        public bool Quinta { get; set; }
        [Display(Name = "Sexta-Feira:")]
        public bool Sexta { get; set; }
        [Display(Name = "Sábado:")]
        public bool Sabado { get; set; }
        [Display(Name = "Horário de início:")]
        public TimeSpan HorarioAtendimentoInicio { get; set; }
        [Display(Name = "Horário de término:")]
        public TimeSpan HorarioAtendimentoTermino { get; set; }
    }
    [Validator(typeof(DiaAgendaVMValidator))]
    public class DiaAgendaVM
    {
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        [Display(Name = "Data:")]
        public DateTime Data { get; set; }
        [Display(Name = "Horário de início:")]
        public TimeSpan HorarioAtendimentoInicio { get; set; }
        [Display(Name = "Horário de término:")]
        public TimeSpan HorarioAtendimentoTermino { get; set; }
    }
    [Validator(typeof(DayOffVMValidator))]
    public class DayOffVM
    {
        public int Id { get; set; }
        public int IdOdontologista { get; set; }
        [Display(Name = "Data:")]
        public DateTime Data { get; set; }
    }
    public class SemanaAgendaVMValidator : AbstractValidator<SemanaAgendaVM>
    {
        public SemanaAgendaVMValidator()
        {
            this.RuleFor(x => x.HorarioAtendimentoInicio)
                .NotEmpty().WithMessage("Informe o horário de início do atendimento")
                .LessThan(new TimeSpan(23, 45, 0)).WithMessage("Horário de início do atendimento inválido")
                .LessThan(x => x.HorarioAtendimentoTermino).WithMessage("Horário de início do atendimento inválido");

            this.RuleFor(x => x.HorarioAtendimentoTermino)
               .NotEmpty().WithMessage("Informe o horário de termino do atendimento")
               .LessThan(new TimeSpan(23, 59, 0)).WithMessage("Horário de início do termino inválido")
               .GreaterThan(x => x.HorarioAtendimentoInicio).WithMessage("Horário de início do termino inválido");
        }
    }
    public class DiaAgendaVMValidator : AbstractValidator<DiaAgendaVM>
    {
        public DiaAgendaVMValidator()
        {
            this.RuleFor(x => x.Data)
                 .NotEmpty().WithMessage("Informe a data.")
                 .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Data inválida.");

            this.RuleFor(x => x.HorarioAtendimentoInicio)
                .NotEmpty().WithMessage("Informe o horário de início do atendimento")
                .LessThan(new TimeSpan(23, 45, 0)).WithMessage("Horário de início do atendimento inválido")
                .LessThan(x => x.HorarioAtendimentoTermino).WithMessage("Horário de início do atendimento inválido");

            this.RuleFor(x => x.HorarioAtendimentoTermino)
               .NotEmpty().WithMessage("Informe o horário de termino do atendimento")
               .LessThan(new TimeSpan(23, 59, 0)).WithMessage("Horário de início do termino inválido")
               .GreaterThan(x => x.HorarioAtendimentoInicio).WithMessage("Horário de início do termino inválido");
        }
    }

    public class DayOffVMValidator : AbstractValidator<DayOffVM>
    {
        public DayOffVMValidator()
        {
            this.RuleFor(x => x.Data)
                .NotEmpty().WithMessage("Informe a data.")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Data inválida.");
        }
    }
}