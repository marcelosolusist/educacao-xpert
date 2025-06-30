using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoConteudos.Application.Commands;

public class IncluirCursoCommand : Command
{
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public Guid UsuarioCriacaoId { get; set; }
    public int Preco { get; set; }

    public IncluirCursoCommand(string nome, string conteudo, Guid usuarioCriacaoId, int preco)
    {
        AggregateId = usuarioCriacaoId;
        Nome = nome;
        Conteudo = conteudo;
        UsuarioCriacaoId = usuarioCriacaoId;
        Preco = preco;
    }

    public override bool EhValido()
    {
        ValidationResult = new IncluirCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class IncluirCursoCommandValidation : AbstractValidator<IncluirCursoCommand>
{
    public static string NomeErro => "O nome do curso não pode ser vazio.";
    public static string ConteudoErro => "O conteúdo não pode ser vazio.";
    public static string UsuarioCriacaoIdErro => "O ID do usuário de criação não pode ser vazio.";
    public static string PrecoErro => "O preço do curso deve ser maior que zero.";

    public IncluirCursoCommandValidation()
    {
        RuleFor(c => c.Nome).NotEmpty().WithMessage(NomeErro);

        RuleFor(c => c.Conteudo).NotEmpty().WithMessage(ConteudoErro);

        RuleFor(c => c.UsuarioCriacaoId).NotEmpty().WithMessage(UsuarioCriacaoIdErro);

        RuleFor(c => c.Preco)
            .GreaterThan(0)
            .WithMessage(PrecoErro);
    }
}