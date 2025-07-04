using EducacaoXpert.Core.Messages;
using FluentValidation;

namespace EducacaoXpert.GestaoAlunos.Application.Commands;

public class IncluirAlunoCommand : Command
{
    public string UsuarioId { get; set; }
    public string Nome { get; set; }

    public IncluirAlunoCommand(string usuarioId, string nome)
    {
        if (Guid.TryParse(usuarioId, out var parsedGuid))
        {
            AggregateId = parsedGuid;
        }
        UsuarioId = usuarioId;
        Nome = nome;
    }

    public override bool EhValido()
    {
        ValidationResult = new IncluirAlunoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class IncluirAlunoCommandValidation : AbstractValidator<IncluirAlunoCommand>
{
    public static string IdErro => "O campo UsuarioId deve ser informado";
    public static string NomeErro => "O campo Nome deve ser informado";

    public IncluirAlunoCommandValidation()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty()
            .WithMessage(IdErro);
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage(NomeErro);
    }
}

