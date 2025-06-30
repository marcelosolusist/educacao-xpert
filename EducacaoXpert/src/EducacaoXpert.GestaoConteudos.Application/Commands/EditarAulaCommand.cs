using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class EditarAulaCommand : Command
{
    public Guid AulaId { get; set; }
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public Guid CursoId { get; set; }
    public string? NomeMaterial { get; set; }
    public string? TipoMaterial { get; set; }

    public EditarAulaCommand(Guid aulaId, string nome, string conteudo, Guid cursoId,
                                string nomeMaterial, string tipoMaterial)
    {
        AggregateId = cursoId;
        AulaId = aulaId;
        Nome = nome;
        Conteudo = conteudo;
        CursoId = cursoId;
        NomeMaterial = nomeMaterial;
        TipoMaterial = tipoMaterial;
    }

    public override bool EhValido()
    {
        ValidationResult = new EditarAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class EditarAulaCommandValidation : AbstractValidator<EditarAulaCommand>
{
    public static string AulaIdErro = "O campo AulaId é obrigatório.";
    public static string NomeErro = "O campo Nome não pode ser vazio.";
    public static string ConteudoErro = "O campo Conteudo não pode ser vazio.";
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public EditarAulaCommandValidation()
    {
        RuleFor(c => c.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AulaIdErro);
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage(NomeErro);
        RuleFor(c => c.Conteudo)
            .NotEmpty()
            .WithMessage(ConteudoErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}
