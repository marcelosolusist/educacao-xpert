using EducacaoXpert.GestaoAlunos.Application.Commands;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class AtivarMatriculaCommandTests
{
    [Fact(DisplayName = "Ativar Matricula Command Valido")]
    [Trait("Categoria", "GestaoAlunos - AtivarMatriculaCommand")]
    public void AtivarMatriculaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new AtivarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Ativar Matricula Command Inválido")]
    [Trait("Categoria", "GestaoAlunos - AtivarMatriculaCommand")]
    public void AtivarMatriculaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new AtivarMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(AtivarMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(AtivarMatriculaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}
