using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoAlunos.Application.Commands;

public class IncluirMatriculaCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public IncluirMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        AggregateId = alunoId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }

    public override bool EhValido()
    {
        ValidationResult = new IncluirMatriculaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class IncluirMatriculaCommandValidation : AbstractValidator<IncluirMatriculaCommand>
{
    public static string AlunoIdErro = "O campo AlunoId não pode ser vazio.";
    public static string CursoIdErro = "O campo CursoId não pode ser vazio.";
    public IncluirMatriculaCommandValidation()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}
