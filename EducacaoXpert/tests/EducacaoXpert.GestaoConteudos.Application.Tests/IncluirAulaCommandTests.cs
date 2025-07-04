using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class IncluirAulaCommandTests
{
    private readonly string NOME_AULA = "Aula de Testes";
    private readonly string CONTEUDO_AULA = "Conteúdo da Aula de Testes";
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly string NOME_MATERIAL = "Nome do Material";
    private readonly string TIPO_MATERIAL = "Tipo do Material";

    [Fact(DisplayName = "IncluirAulaCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - IncluirAulaCommand")]
    public void IncluirAulaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirAulaCommand(NOME_AULA, CONTEUDO_AULA, CURSO_ID, NOME_MATERIAL, TIPO_MATERIAL);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(IncluirAulaCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirAulaCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(IncluirAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "IncluirAulaCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - IncluirAulaCommand")]
    public void IncluirAulaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirAulaCommand(string.Empty, string.Empty, Guid.Empty, string.Empty, string.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(IncluirAulaCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirAulaCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(IncluirAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(3, command.ValidationResult.Errors.Count);
    }
}
