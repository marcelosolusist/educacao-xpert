using EducacaoXpert.Core.Messages;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace EducacaoXpert.GestaoAlunos.Application.Commands;

public class IncluirAdminCommand : Command
{
    public string UsuarioId { get; set; }

    public IncluirAdminCommand(string usuarioId)
    {
        if (Guid.TryParse(usuarioId, out var parsedGuid))
        {
            AggregateId = parsedGuid;
        }
        UsuarioId = usuarioId;
    }

    public override bool EhValido()
    {
        ValidationResult = new IncluirAdminCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class IncluirAdminCommandValidation : AbstractValidator<IncluirAdminCommand>
{
    public static string IdErro => "O campo UsuarioId deve ser informado";
    public IncluirAdminCommandValidation()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty()
            .WithMessage(IdErro);
    }
}