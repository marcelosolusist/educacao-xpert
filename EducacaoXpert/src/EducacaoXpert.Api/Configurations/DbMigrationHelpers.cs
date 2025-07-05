using EducacaoXpert.Api.Context;
using EducacaoXpert.GestaoAlunos.Data.Context;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Data.Context;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.Api.Configurations;

public static class DbMigrationHelpers
{
    public const string GUID_USER_ADMIN = "7dd867ad-4e56-4e6a-9c52-d863ce68d1d6";
    public const string GUID_USER_ALUNO = "1703abf6-94ea-4f36-b3a8-7de9471865a2";

    public static void UseDbMigrationHelper(this WebApplication app)
    {
        ExecutarMigrationsEEfetuarCargaDeDados(app).Wait();
    }

    public static async Task ExecutarMigrationsEEfetuarCargaDeDados(WebApplication application)
    {
        var service = application.Services.CreateScope().ServiceProvider;
        await ExecutarMigrationsEEfetuarCargaDeDados(service);
    }

    public static async Task ExecutarMigrationsEEfetuarCargaDeDados(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextGestaoConteudos = scope.ServiceProvider.GetRequiredService<GestaoConteudosContext>();
        var contextGestaoAlunos = scope.ServiceProvider.GetRequiredService<GestaoAlunosContext>();
        var contextIdentity = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var contextPagamentoFaturamento = scope.ServiceProvider.GetRequiredService<PagamentoFaturamentoContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        var certificadoService = scope.ServiceProvider.GetRequiredService<ICertificadoPdfService>();

        if (env.IsDevelopment() || env.IsEnvironment("Testing"))
        {
            //Apagando objetos de dados
            await contextGestaoConteudos.Database.EnsureDeletedAsync();
            await contextGestaoAlunos.Database.EnsureDeletedAsync();
            await contextIdentity.Database.EnsureDeletedAsync();
            await contextPagamentoFaturamento.Database.EnsureDeletedAsync();

            //Executando migrations
            await contextGestaoConteudos.Database.MigrateAsync();
            await contextGestaoAlunos.Database.MigrateAsync();
            await contextIdentity.Database.MigrateAsync();
            await contextPagamentoFaturamento.Database.MigrateAsync();

            //Efetuando carga de dados
            await CargaDadosUsuariosEPerfis(scope.ServiceProvider);
            await CargaDadosIniciais(contextGestaoAlunos, contextGestaoConteudos, contextIdentity, contextPagamentoFaturamento, certificadoService);
        }
    }

    private static async Task CargaDadosUsuariosEPerfis(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var contextIdentity = scope.ServiceProvider.GetRequiredService<ApiContext>();

        if (contextIdentity.Users.Any()) return;

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var roles = new List<string>() { "ADMIN", "ALUNO" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var userAluno = new IdentityUser
        {
            Id = GUID_USER_ALUNO,
            Email = "usuario@aluno.com",
            EmailConfirmed = true,
            UserName = "usuario@aluno.com",
        };

        var userAdmin = new IdentityUser
        {
            Id = GUID_USER_ADMIN,
            Email = "usuario@admin.com",
            EmailConfirmed = true,
            UserName = "usuario@admin.com",
        };

        await userManager.CreateAsync(userAluno, "Teste@123");
        await userManager.CreateAsync(userAdmin, "Teste@123");

        await userManager.AddToRoleAsync(userAluno, "ALUNO");
        await userManager.AddToRoleAsync(userAdmin, "ADMIN");
    }

    private static async Task CargaDadosIniciais(
         GestaoAlunosContext dbAlunosContext,
         GestaoConteudosContext dbConteudosContext,
         ApiContext dbApiContext,
         PagamentoFaturamentoContext dbPagamentoFaturamentoContext,
         ICertificadoPdfService pdfService)
    {
        if (dbAlunosContext.Set<Aluno>().Any()) return;

        var admin = new Usuario(Guid.Parse(GUID_USER_ADMIN));
        var aluno = new Aluno(Guid.Parse(GUID_USER_ALUNO), "Aluno Exemplar");

        var curso = new Curso( "Clean Code", "Código Limpo", admin.Id, 35000);
        var aulaBoasVindas = new Aula("Boas Vindas", "Bem vindo ao curso de Clean Code");
        var aulaComoFazerOCurso = new Aula("Como fazer o curso", "Dicas de como aproveitar melhor o curso");
        var aulaMaoNaMassa = new Aula("Mão na massa", "Hora de colocar a mão na massa");
        curso.IncluirAula(aulaBoasVindas);
        curso.IncluirAula(aulaComoFazerOCurso);
        curso.IncluirAula(aulaMaoNaMassa);

        var matricula = new Matricula(aluno.Id, curso.Id);
        matricula.Ativar();

        var progressoCurso = new ProgressoCurso(curso.Id, aluno.Id, 3);
        var progressoAulaBoasVindas = new ProgressoAula(aulaBoasVindas.Id);
        progressoCurso.IncluirProgressoAula(progressoAulaBoasVindas);
        progressoCurso.FinalizarProgressoAula(progressoAulaBoasVindas);
        var progressoAulaComoFazerOCurso = new ProgressoAula(aulaComoFazerOCurso.Id);
        progressoCurso.IncluirProgressoAula(progressoAulaComoFazerOCurso);
        progressoCurso.FinalizarProgressoAula(progressoAulaComoFazerOCurso);
        var progressoAulaMaoNaMassa = new ProgressoAula(aulaMaoNaMassa.Id);
        progressoCurso.IncluirProgressoAula(progressoAulaMaoNaMassa);
        progressoCurso.FinalizarProgressoAula(progressoAulaMaoNaMassa);

        // Certificado para o aluno
        var certificado = new Certificado(aluno.Nome, curso.Nome, aluno.Id);
        var pdf = pdfService.GerarPdf(certificado);
        certificado.IncluirArquivo(pdf);

        // Pagamento
        var pagamento = new Pagamento
        {
            AlunoId = aluno.Id,
            CursoId = curso.Id,
            NomeCartao = "Aluno Exemplar",
            NumeroCartaoMascarado = "5502********8294",
            Valor = curso.Preco,
        };
        var transacao = new Transacao
        {
            PagamentoId = pagamento.Id,
            MatriculaId = matricula.Id,
            StatusTransacao = StatusTransacao.Autorizado,
            Pagamento = pagamento,
            Total = pagamento.Valor,
        };

        await dbAlunosContext.Set<Aluno>().AddRangeAsync([aluno]);
        await dbAlunosContext.Set<Usuario>().AddAsync(admin);
        await dbAlunosContext.Set<Matricula>().AddRangeAsync([matricula]);
        await dbAlunosContext.Set<Certificado>().AddAsync(certificado);

        await dbConteudosContext.Set<Curso>().AddRangeAsync([curso]);
        await dbConteudosContext.Set<Aula>().AddRangeAsync([aulaBoasVindas, aulaComoFazerOCurso, aulaMaoNaMassa]);
        await dbConteudosContext.Set<ProgressoCurso>().AddAsync(progressoCurso);
        await dbConteudosContext.Set<ProgressoAula>().AddRangeAsync([progressoAulaBoasVindas, progressoAulaComoFazerOCurso, progressoAulaMaoNaMassa]);

        await dbPagamentoFaturamentoContext.Set<Pagamento>().AddAsync(pagamento);
        await dbPagamentoFaturamentoContext.Set<Transacao>().AddAsync(transacao);

        await dbAlunosContext.SaveChangesAsync();
        await dbConteudosContext.SaveChangesAsync();
        await dbPagamentoFaturamentoContext.SaveChangesAsync();
    }
}