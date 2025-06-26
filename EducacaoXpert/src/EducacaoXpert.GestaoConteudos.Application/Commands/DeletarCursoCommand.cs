using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class DeletarCursoCommand : Command
{
    public Guid CursoId { get; set; }

    public DeletarCursoCommand(Guid cursoId)
    {
        AggregateId = cursoId;
        CursoId = cursoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new DeletarCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class DeletarCursoCommandValidation : AbstractValidator<DeletarCursoCommand>
{
    public static string CursoIdErro => "O ID do curso não pode ser vazio.";
    public DeletarCursoCommandValidation()
    {
        RuleFor(c => c.CursoId).NotEmpty().WithMessage(CursoIdErro);
    }
}
