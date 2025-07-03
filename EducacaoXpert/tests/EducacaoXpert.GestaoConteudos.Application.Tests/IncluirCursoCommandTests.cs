using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class IncluirCursoCommandTests
{
    private readonly string NOME_CURSO = "Nome do Curso de Testes";
    private readonly string CONTEUDO_CURSO = "Conteúdo do Curso de Testes";
    private readonly Guid USUARIO_CRIACAO_ID = Guid.NewGuid();
    private readonly int PRECO_CURSO = 100;

    [Fact(DisplayName = "IncluirCursoCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - IncluirCursoCommand")]
    public void IncluirCursoCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCursoCommand(NOME_CURSO, CONTEUDO_CURSO, USUARIO_CRIACAO_ID, PRECO_CURSO);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(IncluirCursoCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirCursoCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirCursoCommandValidation.UsuarioCriacaoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirCursoCommandValidation.PrecoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "IncluirCursoCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - IncluirCursoCommand")]
    public void IncluirCursoCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCursoCommand(string.Empty, string.Empty, Guid.Empty, 0);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(IncluirCursoCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirCursoCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirCursoCommandValidation.UsuarioCriacaoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirCursoCommandValidation.PrecoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(4, command.ValidationResult.Errors.Count);
    }
}
