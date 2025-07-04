using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class ExcluirCursoCommandTests
{
    private readonly Guid CURSO_ID = Guid.NewGuid();

    [Fact(DisplayName = "ExcluirCursoCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - ExcluirCursoCommand")]
    public void ExcluirCursoCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new ExcluirCursoCommand(CURSO_ID);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(ExcluirCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "ExcluirCursoCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - ExcluirCursoCommand")]
    public void ExcluirCursoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new ExcluirCursoCommand(Guid.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(ExcluirCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Single(command.ValidationResult.Errors);
    }
}
