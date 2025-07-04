using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Handlers;
using Moq.AutoMock;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class IncluirMatriculaCommandTests
{
    private readonly AutoMocker _mocker;
    private readonly MatriculaCommandHandler _handler;
    public IncluirMatriculaCommandTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<MatriculaCommandHandler>();
    }

    [Fact(DisplayName = "Incluir Matricula Command Válido")]
    [Trait("Categoria", "GestaoAlunos - IncluirMatriculaCommand")]
    public void IncluirMatriculaCommand_CommandValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
        Assert.Empty(command.ValidationResult.Errors);
        Assert.DoesNotContain(IncluirMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirMatriculaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }

    [Fact(DisplayName = "Incluir Matricula Command Inválido")]
    [Trait("Categoria", "GestaoAlunos - IncluirMatriculaCommand")]
    public void IncluirMatriculaCommand_CommandValido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirMatriculaCommand(Guid.Empty, Guid.Empty);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Equal(2, command.ValidationResult.Errors.Count);
        Assert.Contains(IncluirMatriculaCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirMatriculaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    }
}
