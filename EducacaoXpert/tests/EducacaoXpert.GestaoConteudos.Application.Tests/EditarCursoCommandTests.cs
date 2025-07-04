using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class EditarCursoCommandTests
{
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly string NOME_CURSO = "Nome do Curso de Testes";
    private readonly string CONTEUDO_CURSO = "Conteúdo do Curso de Testes";
    private readonly int PRECO_CURSO = 100;

    [Fact(DisplayName = "EditarCursoCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - EditarCursoCommand")]
    public void EditarCursoCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new EditarCursoCommand(CURSO_ID, NOME_CURSO, CONTEUDO_CURSO, PRECO_CURSO);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(EditarCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarCursoCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarCursoCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarCursoCommandValidation.PrecoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "EditarCursoCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - EditarCursoCommand")]
    public void EditarCursoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new EditarCursoCommand(Guid.Empty, string.Empty, string.Empty, 0);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(EditarCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarCursoCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarCursoCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarCursoCommandValidation.PrecoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(4, command.ValidationResult.Errors.Count);
    }
}
