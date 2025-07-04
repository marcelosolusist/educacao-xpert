using EducacaoXpert.GestaoAlunos.Application.Commands;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class IncluirAdminCommandTests
{
    [Fact(DisplayName = "Incluir Admin Command Válido")]
    [Trait("Categoria", "GestaoAlunos - IncluirAdminCommand")]
    public void IncluirAdminCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new IncluirAdminCommand(usuarioId);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(IncluirAdminCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "Incluir Admin Command Inválido")]
    [Trait("Categoria", "GestaoAlunos - IncluirAdminCommand")]
    public void IncluirAdminCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var usuarioId = string.Empty;
        var command = new IncluirAdminCommand(usuarioId);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(IncluirAdminCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Single(command.ValidationResult.Errors);
    }
}
