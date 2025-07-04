using EducacaoXpert.GestaoConteudos.Application.Commands;

namespace EducacaoXpert.GestaoConteudos.Application.Tests;

public class EditarAulaCommandTests
{
    private readonly Guid AULA_ID = Guid.NewGuid();
    private readonly string NOME_AULA = "Aula de Testes";
    private readonly string CONTEUDO_AULA = "Conteúdo da Aula de Testes";
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly string NOME_MATERIAL = "Nome do Material";
    private readonly string TIPO_MATERIAL = "Tipo do Material";

    [Fact(DisplayName = "EditarAulaCommand Válido")]
    [Trait("Categoria", "GestaoConteudos - EditarAulaCommand")]
    public void EditarAulaCommand_ComandoEstaValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new EditarAulaCommand(AULA_ID, NOME_AULA, CONTEUDO_AULA, CURSO_ID, NOME_MATERIAL, TIPO_MATERIAL);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.True(ehValido);
        Assert.DoesNotContain(EditarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarAulaCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarAulaCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.DoesNotContain(EditarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact(DisplayName = "EditarAulaCommand Inválido")]
    [Trait("Categoria", "GestaoConteudos - EditarAulaCommand")]
    public void EditarAulaCommand_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new EditarAulaCommand(Guid.Empty, string.Empty, string.Empty, Guid.Empty, string.Empty, string.Empty);

        // Act
        var ehValido = command.EhValido();

        // Assert
        Assert.False(ehValido);
        Assert.Contains(EditarAulaCommandValidation.AulaIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarAulaCommandValidation.NomeErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarAulaCommandValidation.ConteudoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EditarAulaCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(4, command.ValidationResult.Errors.Count);
    }
}
