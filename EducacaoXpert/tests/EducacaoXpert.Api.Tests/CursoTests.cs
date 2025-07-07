using Azure;
using EducacaoXpert.Api.Tests.Config;
using EducacaoXpert.Api.ViewModels;
using System.Net;
using System.Net.Http.Json;

namespace EducacaoXpert.Api.Tests;

[TestCaseOrderer("EducacaoXpert.Api.Tests.PriorityOrderer", "EducacaoXpert.Api.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class CursoTests
{
    private readonly IntegrationTestsFixture _fixture;
    public static string NOME_CURSO = "Curso de Testes";
    public static string NOME_SEGUNDO_CURSO = "Segundo Curso de Testes";
    public static string NOME_SEGUNDO_CURSO_ALTERADO = "Segundo Curso Alterado";
    public static string CONTEUDO_CURSO = "Conteúdo do Curso de Testes";
    public static int PRECO_CURSO = 10000;
    public static string NOME_AULA = "Aula de Testes";
    public static string NOME_SEGUNDA_AULA = "Segunda Aula de Testes";
    public static string NOME_TERCEIRA_AULA = "Terceira Aula de Testes";
    public static string NOME_TERCEIRA_AULA_ALTERADA = "Terceira Aula Alterada";
    public static string CONTEUDO_AULA = "Conteúdo da Aula de Testes";
    public static string NOME_MATERIAL_AULA = "Nome do Material";
    public static string TIPO_MATERIAL_AULA = "Tipo de Material";
    public static string NOME_ALUNO_TESTES = "Aluno de Testes";
    public static string EMAIL_ALUNO_TESTES = "aluno@testes.com";
    public static string SENHA_ALUNO_TESTES = "Teste@123";
    public static string EMAIL_ADMIN_TESTES = "admin@testes.com";
    public static string SENHA_ADMIN_TESTES = "Teste@123";

    public CursoTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "01 - Registrar Admin de Testes Inválido"), TestPriority(1)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task RegistrarAdminDeTestes_DadosInvalidos_NaoDeveRegistrar()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel()
        {
            Nome = string.Empty,
            Email = string.Empty,
            Senha = string.Empty,
            ConfirmacaoSenha = string.Empty
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar-admin", registerUser);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "02 - Registrar Admin de Testes Válido"), TestPriority(2)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task RegistrarAdminDeTestes_DadosValidos_DeveRegistrar()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel()
        {
            Nome = EMAIL_ADMIN_TESTES,
            Email = EMAIL_ADMIN_TESTES,
            Senha = SENHA_ADMIN_TESTES,
            ConfirmacaoSenha = SENHA_ADMIN_TESTES
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar-admin", registerUser);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "03 - Registrar Aluno de Testes Inválido"), TestPriority(3)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task RegistrarAlunoDeTestes_DadosInvalidos_NaoDeveRegistrar()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel()
        {
            Nome = string.Empty,
            Email = string.Empty,
            Senha = string.Empty,
            ConfirmacaoSenha = string.Empty
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar-aluno", registerUser);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "04 - Registrar Aluno de Testes Válido"), TestPriority(4)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task RegistrarAlunoDeTestes_DadosValidos_DeveRegistrar()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel()
        {
            Nome = NOME_ALUNO_TESTES,
            Email = EMAIL_ALUNO_TESTES,
            Senha = SENHA_ALUNO_TESTES,
            ConfirmacaoSenha = SENHA_ALUNO_TESTES
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/conta/registrar-aluno", registerUser);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "05 - Incluir Curso Dados Inválidos"), TestPriority(5)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirCurso_DadosInvalidos_NaoDeveIncluir()
    {
        // Arrange
        var data = new CursoViewModel
        {
            Nome = string.Empty,
            Conteudo = string.Empty,
            Preco = 0,
        };

        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/cursos", data);

        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O nome do curso não pode ser vazio.", erros.ToString());
        Assert.Contains("O conteúdo não pode ser vazio.", erros.ToString());
        Assert.Contains("O preço do curso deve ser maior que zero.", erros.ToString());
    }
    [Fact(DisplayName = "06 - Incluir Cursos Dados Válidos"), TestPriority(6)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirCursos_DadosValidos_DeveIncluir()
    {
        // Arrange
        var curso1 = new CursoViewModel
        {
            Nome = NOME_CURSO,
            Conteudo = CONTEUDO_CURSO,
            Preco = PRECO_CURSO,
        };

        var curso2 = new CursoViewModel
        {
            Nome = NOME_SEGUNDO_CURSO,
            Conteudo = CONTEUDO_CURSO,
            Preco = PRECO_CURSO,
        };

        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        // Act
        var response1 = await _fixture.Client.PostAsJsonAsync("/api/cursos", curso1);
        var response2 = await _fixture.Client.PostAsJsonAsync("/api/cursos", curso2);
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        var result1 = await response1.Content.ReadAsStringAsync();
        var result2 = await response2.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result1));
        Assert.True(!string.IsNullOrEmpty(result2));
    }
    [Fact(DisplayName = "07 - Editar Curso Dados Inválidos"), TestPriority(7)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task EditarCurso_DadosInvalidos_NaoDeveEditar()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = Guid.Empty;

        var curso = new CursoViewModel
        {
            Id = Guid.Empty,
            Nome = string.Empty,
            Conteudo = string.Empty,
            Preco = 0,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{cursoId}", curso);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "08 - Editar Curso Dados Válidos"), TestPriority(8)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task EditarCurso_DadosValidos_DeveEditar()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_SEGUNDO_CURSO);

        var curso = new CursoViewModel
        {
            Id = cursoId,
            Nome = NOME_SEGUNDO_CURSO_ALTERADO,
            Conteudo = CONTEUDO_CURSO,
            Preco = PRECO_CURSO,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{cursoId}", curso);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    [Fact(DisplayName = "09 - Excluir Curso Dados Inválidos"), TestPriority(9)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task ExcluirCurso_DadosInvalidos_NaoDeveExcluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = Guid.Empty;

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{cursoId}");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "10 - Excluir Curso Dados Válidos"), TestPriority(10)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task ExcluirCurso_DadosValidos_DeveExcluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_SEGUNDO_CURSO_ALTERADO);

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{cursoId}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "11 - Incluir Aula Dados Inválidos"), TestPriority(11)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirAula_DadosInvalidos_NaoDeveIncluir()
    {
        // Arrange
        var data = new AulaViewModel
        {
            Nome = string.Empty,
            Conteudo = string.Empty,
            NomeMaterial = string.Empty,
            TipoMaterial = string.Empty
        };

        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", data);

        var erros = _fixture.ObterErros(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("O campo Nome não pode ser vazio.", erros.ToString());
        Assert.Contains("O campo Conteúdo não pode ser vazio.", erros.ToString());
    }
    [Fact(DisplayName = "12 - Incluir Aulas Dados Válidos"), TestPriority(12)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirAulas_DadosValidos_DeveIncluir()
    {
        // Arrange
        var aula1 = new AulaViewModel
        {
            Nome = NOME_AULA,
            Conteudo = CONTEUDO_AULA,
            NomeMaterial = NOME_MATERIAL_AULA,
            TipoMaterial = TIPO_MATERIAL_AULA,
        };

        var aula2 = new AulaViewModel
        {
            Nome = NOME_SEGUNDA_AULA,
            Conteudo = CONTEUDO_AULA,
            NomeMaterial = NOME_MATERIAL_AULA,
            TipoMaterial = TIPO_MATERIAL_AULA,
        };

        var aula3 = new AulaViewModel
        {
            Nome = NOME_TERCEIRA_AULA,
            Conteudo = CONTEUDO_AULA,
            NomeMaterial = NOME_MATERIAL_AULA,
            TipoMaterial = TIPO_MATERIAL_AULA,
        };

        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        // Act
        var response1 = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", aula1);
        response1.EnsureSuccessStatusCode();
        var result01 = await response1.Content.ReadAsStringAsync();

        var response2 = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", aula2);
        response2.EnsureSuccessStatusCode();
        var result02 = await response2.Content.ReadAsStringAsync();

        var response3 = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/aulas", aula3);
        response2.EnsureSuccessStatusCode();
        var result03 = await response2.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result01));
        Assert.True(!string.IsNullOrEmpty(result02));
        Assert.True(!string.IsNullOrEmpty(result03));
    }
    [Fact(DisplayName = "13 - Editar Aula Dados Inválidos"), TestPriority(13)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task EditarAula_DadosInvalidos_NaoDeveEditar()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        var aula = new AulaViewModel
        {
            Id = aulaId,
            Nome = string.Empty,
            Conteudo = string.Empty,
            NomeMaterial = string.Empty,
            TipoMaterial = string.Empty,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{cursoId}/aulas", aula);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "14 - Editar Aula Dados Válidos"), TestPriority(14)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task EditarAula_DadosValidos_DeveEditar()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);
        var aulaId = await _fixture.ObterIdAula(NOME_TERCEIRA_AULA);

        var aula = new AulaViewModel
        {
            Id = aulaId,
            Nome = NOME_TERCEIRA_AULA_ALTERADA,
            Conteudo = CONTEUDO_AULA,
            NomeMaterial = NOME_MATERIAL_AULA,
            TipoMaterial = TIPO_MATERIAL_AULA,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/cursos/{cursoId}/aulas/{aulaId}", aula);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "15 - Excluir Aula Dados Inválidos"), TestPriority(15)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task ExcluirAula_DadosInvalidos_NaoDeveExcluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{cursoId}/aulas/{aulaId}");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "16 - Excluir Aula Dados Válidos"), TestPriority(16)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task ExcluirAula_DadosValidos_DeveExcluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAdminDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);
        var aulaId = await _fixture.ObterIdAula(NOME_TERCEIRA_AULA_ALTERADA);

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/cursos/{cursoId}/aulas/{aulaId}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "17 - Incluir Matrícula Dados Inválidos"), TestPriority(17)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirMatricula_DadosInvalidos_NaoDeveIncluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        var curso = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{curso}/matricular", null);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "18 - Incluir Matrícula Dados Válidos"), TestPriority(18)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task IncluirMatricula_DadosValidos_DeveIncluir()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{cursoId}/matricular", null);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "19 - Pagar Matrícula Dados Inválidos"), TestPriority(19)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task PagarMatricula_DadosInvalidos_NaoDevePagar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        var dadosPagamentoCartao = new DadosPagamentoViewModel()
        {
            NomeCartao = string.Empty,
            NumeroCartao = string.Empty,
            ExpiracaoCartao = string.Empty,
            CvvCartao = string.Empty
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/pagar-matricula", dadosPagamentoCartao);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "20 - Pagar Matrícula Dados Válidos"), TestPriority(20)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task PagarMatricula_DadosValidos_DevePagar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);

        _fixture.GerarDadosCartao();

        var dadosPagamentoCartao = new DadosPagamentoViewModel()
        {
            NomeCartao = _fixture.DadosPagamento.NomeCartao,
            NumeroCartao = _fixture.DadosPagamento.NumeroCartao,
            ExpiracaoCartao = _fixture.DadosPagamento.ExpiracaoCartao,
            CvvCartao = _fixture.DadosPagamento.CvvCartao
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/cursos/{cursoId}/pagar-matricula", dadosPagamentoCartao);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "21 - Iniciar Aula Dados Inválidos"), TestPriority(21)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Iniciar_AulaInvalida_NaoDeveIniciar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/cursos/{cursoId}/aulas/{aulaId}/iniciar", null);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "22 - Iniciar Aulas Dados Válidos"), TestPriority(22)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Iniciar_AulasValidas_DeveIniciar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);
        var aula1Id = await _fixture.ObterIdAula(NOME_AULA);
        var aula2Id = await _fixture.ObterIdAula(NOME_SEGUNDA_AULA);

        // Act
        var response1 = await _fixture.Client.PostAsync($"/api/cursos/{cursoId}/aulas/{aula1Id}/iniciar", null);
        var response2 = await _fixture.Client.PostAsync($"/api/cursos/{cursoId}/aulas/{aula2Id}/iniciar", null);

        // Assert
        Assert.True(response1.IsSuccessStatusCode);
        Assert.True(response2.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "23 - Assistir Aula Dados Inválidos"), TestPriority(23)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Assistir_AulaInvalida_NaoDeveAssistir()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.GetAsync($"/api/cursos/{cursoId}/aulas/{aulaId}/assistir");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "24 - Assistir Aulas Dados Válidos"), TestPriority(24)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Assistir_AulasValidas_DeveAssistir()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);
        var aula1Id = await _fixture.ObterIdAula(NOME_AULA);
        var aula2Id = await _fixture.ObterIdAula(NOME_SEGUNDA_AULA);

        // Act
        var response1 = await _fixture.Client.GetAsync($"/api/cursos/{cursoId}/aulas/{aula1Id}/assistir");
        var response2 = await _fixture.Client.GetAsync($"/api/cursos/{cursoId}/aulas/{aula2Id}/assistir");

        // Assert
        Assert.True(response1.IsSuccessStatusCode);
        Assert.True(response2.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "25 - Finalizar Aula Dados Inválidos"), TestPriority(25)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Finalizar_AulaInvalida_NaoDeveFinalizar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PutAsync($"/api/cursos/{cursoId}/aulas/{aulaId}/finalizar", null);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "26 - Finalizar Aulas Dados Válidos"), TestPriority(26)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task Finalizar_AulasValidas_DeveFinalizar()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);
        var cursoId = await _fixture.ObterIdCurso(NOME_CURSO);
        var aula1Id = await _fixture.ObterIdAula(NOME_AULA);
        var aula2Id = await _fixture.ObterIdAula(NOME_SEGUNDA_AULA);

        // Act
        var response1 = await _fixture.Client.PutAsync($"/api/cursos/{cursoId}/aulas/{aula1Id}/finalizar", null);
        var response2 = await _fixture.Client.PutAsync($"/api/cursos/{cursoId}/aulas/{aula2Id}/finalizar", null);

        // Assert
        Assert.True(response1.IsSuccessStatusCode);
        Assert.True(response2.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "27 - Baixar Certificado Dados Inválidos"), TestPriority(27)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task BaixarCertificado_DadosInvalidos_NaoDeveBaixarCertificado()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var certificadoId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.GetAsync($"/api/alunos/certificados/{certificadoId}/download");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    [Fact(DisplayName = "28 - Baixar Certificado Dados Válidos"), TestPriority(28)]
    [Trait("Categoria", "Integração Api - Curso")]
    public async Task BaixarCertificado_DadosValidos_DeveBaixarCertificado()
    {
        // Arrange
        await _fixture.EfetuarLoginAlunoDeTestes();
        _fixture.Client.AtribuirToken(_fixture.Token);

        var certificadoId = await _fixture.ObterIdCertificadoCursoTestes();

        // Act
        var response = await _fixture.Client.GetAsync($"/api/alunos/certificados/{certificadoId}/download");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }
    
}
