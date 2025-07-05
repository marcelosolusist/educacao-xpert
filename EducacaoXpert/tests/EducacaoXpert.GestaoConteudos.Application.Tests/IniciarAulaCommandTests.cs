using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class IniciarAulaCommandTests
{
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly Guid AULA_ID = Guid.NewGuid();
    private readonly Guid ALUNO_ID = Guid.NewGuid();

    [Fact(DisplayName = "IniciarAulaCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - IniciarAulaCommand")]
    public void IniciarAulaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new AssistirAulaCommand(CURSO_ID, AULA_ID, ALUNO_ID);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(IniciarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IniciarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IniciarAulaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "IniciarAulaCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - IniciarAulaCommand")]
    public void IniciarAulaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new AssistirAulaCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(IniciarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IniciarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IniciarAulaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(3, command.ValidationResult.Errors.Count);
    }
}
