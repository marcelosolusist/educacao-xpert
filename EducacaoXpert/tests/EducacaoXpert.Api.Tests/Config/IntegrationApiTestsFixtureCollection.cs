using Bogus;
using Bogus.DataSets;
using Dapper;
using EducacaoXpert.Api.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EducacaoXpert.Api.ViewModels;
using EducacaoXpert.Core.DomainObjects.Enums;
using System.Net.Http.Json;
using System.Text.Json;

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
            BaseAddress = new Uri("http://localhost:7139")
        };
        Factory = new EducacaoXpertAppFactory();
        Client = Factory.CreateClient(options);
        DadosPagamento = new DadosPagamentoViewModel();
        var configuration = Factory.Services.GetRequiredService<IConfiguration>();
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public void GerarDadosUsuario()
    {
        var faker = new Faker("pt_BR");
        EmailUsuario = faker.Internet.Email().ToLower();
        NomeUsuario = EmailUsuario;
        SenhaUsuario = faker.Internet.Password(8, false, "", "@1Ab_");
        SenhaConfirmacao = SenhaUsuario;
    }

    public void GerarDadosCartao()
    {
        var faker = new Faker("pt_BR");
        DadosPagamento.NomeCartao = faker.Name.FullName();
        DadosPagamento.NumeroCartao = faker.Finance.CreditCardNumber(CardType.Mastercard);
        DadosPagamento.ExpiracaoCartao = faker.Date.Future(1, DateTime.Now).ToString("MM/yy");
        DadosPagamento.CvvCartao = faker.Finance.CreditCardCvv();
    }

    public void SalvarUserToken(string token)
    {
        var response = JsonSerializer.Deserialize<LoginResponseWrapper>(token,
             new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new LoginResponseWrapper();
        Token = response.Data.AccessToken;
        AlunoId = Guid.Parse(response.Data.UserToken.Id);
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

    public async Task RegistrarAluno()
    {
        GerarDadosUsuario();
        var registerUser = new RegisterUserViewModel()
        {
            Nome = NomeUsuario,
            Email = EmailUsuario,
            Senha = SenhaUsuario,
            ConfirmacaoSenha = SenhaConfirmacao
        };

        var response = await Client.PostAsJsonAsync("/api/conta/registrar-aluno", registerUser);
        response.EnsureSuccessStatusCode();

        SalvarUserToken(await response.Content.ReadAsStringAsync());
    }

    public async Task<Guid> ObterIdCurso()
    {
        var response = await Client.GetAsync("/api/cursos");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStringAsync();

        var json = JsonSerializer.Deserialize<JsonElement>(data);
        return json.GetProperty("data")[0].GetProperty("id").GetGuid();
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
