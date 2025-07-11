﻿using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class FinalizarAulaCommand : Command
{
    public Guid CursoId { get; set; }
    public Guid AulaId { get; set; }
    public Guid AlunoId { get; set; }

    public FinalizarAulaCommand(Guid cursoId, Guid aulaId, Guid alunoId)
    {
        CursoId = cursoId;
        AulaId = aulaId;
        AlunoId = alunoId;
    }

    public override bool EhValido()
    {
        ValidationResult = new FinalizarAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class FinalizarAulaCommandValidation : AbstractValidator<FinalizarAulaCommand>
{
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public static string AulaIdErro = "O campo AulaId é obrigatório.";
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public FinalizarAulaCommandValidation()
    {
        RuleFor(a => a.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
        RuleFor(a => a.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AulaIdErro);
        RuleFor(a => a.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
    }
}
