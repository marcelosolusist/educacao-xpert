using Bogus;
using Bogus.DataSets;
using EducacaoXpert.Api.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EducacaoXpert.Api.Tests.Config;

[CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }

public class IntegrationTestsFixture : IDisposable
{
    public readonly EducacaoXpertAppFactory Factory;
    public HttpClient Client;
    public string ConnectionString { get; set; }
    public string NomeUsuario { get; set; }
    public string SenhaUsuario { get; set; }
    public string EmailUsuario { get; set; }
    public string SenhaConfirmacao { get; set; }
    public string Token { get; set; }
    public DadosPagamentoViewModel DadosPagamento { get; set; }
    public Guid CursoId { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public Guid AulaId { get; set; }
    public Guid CertificadoId { get; set; }

    public IntegrationTestsFixture()
    {
        var options = new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5229")
        };
        Factory = new EducacaoXpertAppFactory();
        Client = Factory.CreateClient(options);
        DadosPagamento = new DadosPagamentoViewModel();
        var configuration = Factory.Services.GetRequiredService<IConfiguration>();
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public void GerarDadosCartao()
    {
        var faker = new Faker("pt_BR");
        DadosPagamento.NomeCartao = faker.Name.FullName();
        DadosPagamento.NumeroCartao = Regex.Replace(faker.Finance.CreditCardNumber(CardType.Visa), @"[^\d]", "") ;
        DadosPagamento.ExpiracaoCartao = faker.Date.Future(4, DateTime.Now).ToString("MM/yy");
        DadosPagamento.CvvCartao = Regex.Replace(faker.Finance.CreditCardCvv(), @"[^\d]", "");
    }

    public void SalvarUserToken(string token)
    {
        var response = JsonSerializer.Deserialize<LoginResponseWrapper>(token,
             new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new LoginResponseWrapper();
        Token = response.Data.AccessToken;
        AlunoId = Guid.Parse(response.Data.UserToken.Id);
    }

    public async Task EfetuarLoginAlunoDeTestes(string? email = null, string? senha = null)
    {
        var userData = new LoginUserViewModel()
        {
            Email = email ?? CursoTests.EMAIL_ALUNO_TESTES,
            Senha = senha ?? CursoTests.SENHA_ALUNO_TESTES
        };

        var response = await Client.PostAsJsonAsync("/api/conta/login", userData);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }

    public async Task EfetuarLoginAluno(string? email = null, string? senha = null)
    {
        var userData = new LoginUserViewModel()
        {
            Email = email ?? "usuario@aluno.com",
            Senha = senha ?? "Teste@123"
        };

        var response = await Client.PostAsJsonAsync("/api/conta/login", userData);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }

    public async Task EfetuarLoginAdmin(string? email = null, string? senha = null)
    {
        var userData = new LoginUserViewModel()
        {
            Email = email ?? "usuario@admin.com",
            Senha = senha ?? "Teste@123"
        };

        var response = await Client.PostAsJsonAsync("/api/conta/login", userData);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }

    public async Task<Guid> ObterIdCursoTestes()
    {
        var response = await Client.GetAsync("/api/cursos");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        var retorno = JsonSerializer.Deserialize<RetornoGenericoGet>(data);
        if (retorno == null) return Guid.NewGuid();
        foreach (JsonElement registro in retorno.data)
        {
            if (registro.GetProperty("nome").GetString() == CursoTests.NOME_CURSO) return registro.GetProperty("id").GetGuid();
        }
        return Guid.NewGuid();
    }

    public async Task<Guid> ObterIdAulaTestes()
    {
        var idCursoTestes = await ObterIdCursoTestes();
        var response = await Client.GetAsync($"/api/cursos/{idCursoTestes}/aulas");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        var retorno = JsonSerializer.Deserialize<RetornoGenericoGet>(data);
        if (retorno == null) return Guid.NewGuid();
        foreach (JsonElement registro in retorno.data)
        {
            if (registro.GetProperty("nome").GetString() == CursoTests.NOME_AULA) return registro.GetProperty("id").GetGuid();
        }
        return Guid.NewGuid();
    }

    public async Task<Guid> ObterIdCertificadoCursoTestes()
    {
        var response = await Client.GetAsync("/api/alunos/certificados");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        var retorno = JsonSerializer.Deserialize<RetornoGenericoGet>(data);
        if (retorno == null) return Guid.NewGuid();
        foreach (JsonElement registro in retorno.data)
        {
            if (registro.GetProperty("nomeAluno").GetString() == CursoTests.NOME_ALUNO_TESTES &&
                    registro.GetProperty("nomeCurso").GetString() == CursoTests.NOME_CURSO) return registro.GetProperty("id").GetGuid();
        }
        return Guid.NewGuid();
    }

    public JsonElement ObterErros(string result)
    {
        var json = JsonSerializer.Deserialize<JsonElement>(result);
        return json.GetProperty("erros");
    }

    public void Dispose()
    {
        Factory.Dispose();
        Client.Dispose();
    }

}
