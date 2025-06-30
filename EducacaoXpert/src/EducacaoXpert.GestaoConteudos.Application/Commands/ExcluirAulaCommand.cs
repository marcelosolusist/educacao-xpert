using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class ExcluirAulaCommand : Command
{
    public Guid AulaId { get; set; }

    public ExcluirAulaCommand(Guid cursoId)
    {
        AggregateId = cursoId;
        AulaId = cursoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new ExcluirAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ExcluirAulaCommandValidation : AbstractValidator<ExcluirAulaCommand>
{
    public static string AulaIdErro => "O ID da aula não pode ser vazio.";
    public ExcluirAulaCommandValidation()
    {
        RuleFor(c => c.AulaId).NotEmpty().WithMessage(AulaIdErro);
    }
}
