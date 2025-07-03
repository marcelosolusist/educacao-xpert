using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class FinalizarAulaCommandTests
{
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly Guid AULA_ID = Guid.NewGuid();
    private readonly Guid ALUNO_ID = Guid.NewGuid();

    [Fact(DisplayName = "FinalizarAulaCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - FinalizarAulaCommand")]
    public void FinalizarAulaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new FinalizarAulaCommand(CURSO_ID, AULA_ID, ALUNO_ID);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(FinalizarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(FinalizarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(FinalizarAulaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "FinalizarAulaCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - FinalizarAulaCommand")]
    public void FinalizarAulaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new FinalizarAulaCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(FinalizarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(FinalizarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(FinalizarAulaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(3, command.ValidationResult.Errors.Count);
    }
}
