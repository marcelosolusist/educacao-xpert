using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Tests;

public class EntityAulaTests
{
    private readonly string NOME_AULA = "Nome da aula";
    private readonly string CONTEUDO_AULA = "Conteúdo da aula";
    private readonly string NOME_MATERIAL = "Nome do material";
    private readonly string TIPO_MATERIAL = "Tipo do material";

    [Fact(DisplayName = "Incluir Aula Dados Válidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirAulaValida_DevePassarNaValidacao()
    {
        // Arrange && Act
        var aula = new Aula(NOME_AULA, CONTEUDO_AULA);

        // Assert
        Assert.True(aula.Nome == NOME_AULA);
        Assert.True(aula.Conteudo == CONTEUDO_AULA);

    }
    [Fact(DisplayName = "Incluir Aula Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirAulaInvalida_DeveLancarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Aula(string.Empty, string.Empty));

    }
    [Fact(DisplayName = "Incluir Material Válido na Aula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirMaterialValidoNaAula_DevePassarNaValidacao()
    {
        // Arrange
        var aula = new Aula(NOME_AULA, CONTEUDO_AULA);
        var material = new Material(NOME_MATERIAL, TIPO_MATERIAL);

        // Act
        aula.IncluirMaterial(material);

        // Assert
        Assert.True(material.Nome == NOME_MATERIAL);
        Assert.True(material.Tipo == TIPO_MATERIAL);
        Assert.Equal(1, aula.Materiais.Count(a => a.AulaId == aula.Id));
    }
    [Fact(DisplayName = "Incluir Material Inválido")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirMaterialInvalido_DeveLancarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Material(string.Empty, string.Empty));
    }
    [Fact(DisplayName = "Incluir Material Duplicado na Aula")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirMaterialDuplicadoNaAula_NaoDevePassarNaValidacao()
    {
        // Arrange
        var aula = new Aula(NOME_AULA, CONTEUDO_AULA);
        var material = new Material(NOME_MATERIAL, TIPO_MATERIAL);

        // Act
        aula.IncluirMaterial(material);

        // Assert
        Assert.Throws<DomainException>(() => aula.IncluirMaterial(material));
        Assert.Equal(1, aula.Materiais.Count(a => a.AulaId == aula.Id));
    }
}
