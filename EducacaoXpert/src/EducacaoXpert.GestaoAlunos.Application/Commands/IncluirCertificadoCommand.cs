using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoAlunos.Application.Commands;

public class IncluirCertificadoCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; }

    public IncluirCertificadoCommand(Guid alunoId, Guid cursoId, string nomeCurso)
    {
        AggregateId = alunoId;
        AlunoId = alunoId;
        CursoId = cursoId;
        NomeCurso = nomeCurso;
    }
    public override bool EhValido()
    {
        ValidationResult = new GerarCertificadoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class GerarCertificadoCommandValidator : AbstractValidator<IncluirCertificadoCommand>
{
    public static string AlunoIdErro => "O campo AlunoId é obrigatório.";
    public static string CursoIdErro => "O campo CursoId é obrigatório.";
    public static string NomeCursoErro => "O campo NomeCurso é obrigatório.";

    public GerarCertificadoCommandValidator()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
        RuleFor(c => c.NomeCurso)
            .NotEmpty()
            .WithMessage(NomeCursoErro);
    }
}
