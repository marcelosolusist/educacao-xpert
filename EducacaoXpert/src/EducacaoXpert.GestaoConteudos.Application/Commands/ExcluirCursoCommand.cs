using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class ExcluirCursoCommand : Command
{
    public Guid CursoId { get; set; }

    public ExcluirCursoCommand(Guid cursoId)
    {
        AggregateId = cursoId;
        CursoId = cursoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new ExcluirCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ExcluirCursoCommandValidation : AbstractValidator<ExcluirCursoCommand>
{
    public static string CursoIdErro => "O ID do curso não pode ser vazio.";
    public ExcluirCursoCommandValidation()
    {
        RuleFor(c => c.CursoId).NotEmpty().WithMessage(CursoIdErro);
    }
}
