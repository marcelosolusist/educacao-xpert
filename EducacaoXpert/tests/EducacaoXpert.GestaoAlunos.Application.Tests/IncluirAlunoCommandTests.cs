using EducacaoXpert.GestaoAlunos.Application.Commands;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class IncluirAlunoCommandTests
{
    [Fact(DisplayName = "Incluir Aluno Command Válido")]
    [Trait("Categoria", "GestaoAlunos - IncluirAlunoCommand")]
    public void IncluirAlunoCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid().ToString();
        var command = new IncluirAlunoCommand(usuarioId, "Aluno de Teste");

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(IncluirAlunoCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirAlunoCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "Incluir Aluno Command Inválido")]
    [Trait("Categoria", "GestaoAlunos - IncluirAlunoCommand")]
    public void IncluirAlunoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var usuarioId = string.Empty;
        var command = new IncluirAlunoCommand(string.Empty, string.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(IncluirAlunoCommandValidation.IdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirAlunoCommandValidation.NomeErro,
                        command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(2, command.ValidationResult.Errors.Count);
    }
}
