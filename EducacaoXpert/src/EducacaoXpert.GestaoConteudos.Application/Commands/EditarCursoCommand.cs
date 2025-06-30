using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class EditarCursoCommand : Command
{
    public Guid CursoId { get; set; }
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public int Preco { get; set; }

    public EditarCursoCommand(Guid cursoId, string nome, string conteudo, int preco)
    {
        AggregateId = cursoId;
        CursoId = cursoId;
        Nome = nome;
        Conteudo = conteudo;
        Preco = preco;
    }

    public override bool EhValido()
    {
        ValidationResult = new EditarCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class EditarCursoCommandValidation : AbstractValidator<EditarCursoCommand>
{
    public static string CursoIdErro => "O ID do curso não pode ser vazio.";
    public static string NomeErro => "O nome do curso não pode ser vazio.";
    public static string ConteudoErro => "O conteúdo não pode ser vazio.";
    public static string PrecoErro => "O preço do curso deve ser maior que zero.";
    public EditarCursoCommandValidation()
    {
        RuleFor(c => c.CursoId).NotEmpty().WithMessage(CursoIdErro);
        RuleFor(c => c.Nome).NotEmpty().WithMessage(NomeErro);
        RuleFor(c => c.Conteudo).NotEmpty().WithMessage(ConteudoErro);
        RuleFor(c => c.Preco)
            .GreaterThan(0)
            .WithMessage(PrecoErro);
    }
}
