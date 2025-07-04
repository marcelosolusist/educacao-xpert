using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class ExcluirAulaCommandTests
{
    private readonly Guid AULA_ID = Guid.NewGuid();

    [Fact(DisplayName = "ExcluirAulaCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - ExcluirAulaCommand")]
    public void ExcluirAulaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new ExcluirAulaCommand(AULA_ID);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(ExcluirAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "ExcluirAulaCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - ExcluirAulaCommand")]
    public void ExcluirAulaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new ExcluirAulaCommand(Guid.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(ExcluirAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Single(command.ValidationResult.Errors);
    }
}
