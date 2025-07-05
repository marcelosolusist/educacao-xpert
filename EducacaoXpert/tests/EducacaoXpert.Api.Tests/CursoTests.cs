using System.Net.Http.Json;
using EducacaoXpert.Api.Tests.Config;
using EducacaoXpert.Api.ViewModels;
using EducacaoXpert.Core.DomainObjects.Enums;

namespace EducacaoXpert.Api.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class CursoTests
{
    private readonly IntegrationTestsFixture _fixture;
    private readonly string NOME_CURSO = "Curso de Testes";
    private readonly string CONTEUDO_CURSO = "Conteúdo do Curso de Testes";
    private readonly int PRECO_CURSO = 10000;
    private readonly string NOME_AULA = "Aula de Testes";
    private readonly string CONTEUDO_AULA = "Conteúdo da Aula de Testes";
    private readonly string NOME_MATERIAL_AULA = "Nome do Material da Aula";
    private readonly string TIPO_MATERIAL_AULA = "Tipo de Material da Aula";

    public CursoTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }
    [Fact(DisplayName = "Incluir Curso Dados Válidos")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Incluir_CursoValido_DeveIncluir()
    {
        // Arrange
        var data = new CursoViewModel
        {
            Nome = NOME_CURSO,
            Conteudo = CONTEUDO_CURSO,
            Preco = PRECO_CURSO,
        };

        await _fixture.EfetuarLoginAdmin();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/cursos", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "Incluir Curso Dados Inválidos")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Incluir_CursoInvalido_NaoDeveIncluir()
    {
        // Arrange
        var data = new CursoViewModel
        {
            Nome = string.Empty,
            Conteudo = string.Empty,
            Preco = 0,
        };

        await _fixture.EfetuarLoginAdmin();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/cursos", data);

        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O nome do curso não pode ser vazio.", erros.ToString());
        Assert.Contains("O conteúdo não pode ser vazio.", erros.ToString());
        Assert.Contains("O preço do curso deve ser maior que zero.", erros.ToString());
    }
    [Fact(DisplayName = "Incluir Aula Dados Válidos")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Incluir_AulaValida_DeveIncluir()
    {
        // Arrange
        var data = new AulaViewModel
        {
            Nome = NOME_AULA,
            Conteudo = CONTEUDO_AULA,
            NomeMaterial = NOME_MATERIAL_AULA,
            TipoMaterial = TIPO_MATERIAL_AULA,
        };

        await _fixture.EfetuarLoginAdmin();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso();

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "Incluir Aula Dados Inválidos")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Incluir_AulaInvalida_NaoDeveIncluir()
    {
        // Arrange
        var data = new AulaViewModel
        {
            Nome = string.Empty,
            Conteudo = string.Empty,
            NomeMaterial= string.Empty,
            TipoMaterial= string.Empty
        };

        await _fixture.EfetuarLoginAdmin();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso();

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", data);

        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O campo Nome não pode ser vazio.", erros.ToString());
        Assert.Contains("O campo Conteúdo não pode ser vazio.", erros.ToString());
    }

    [Fact(DisplayName = "Realizar Matrícula com Sucesso")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task MatriculaTests_RealizarMatriculaValida_DeveRealizar()
    {
        // Arrange
        await _fixture.RegistrarAluno();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{cursoId}/matricular", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Realizar Matrícula com Erro")]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task MatriculaTests_RealizarMatriculaInvalida_NaoDeveRealizar()
    {
        // Arrange
        await _fixture.RegistrarAluno();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var curso = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{curso}/matricular", null);
        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Curso não encontrado.", erros.ToString());
    }
}
