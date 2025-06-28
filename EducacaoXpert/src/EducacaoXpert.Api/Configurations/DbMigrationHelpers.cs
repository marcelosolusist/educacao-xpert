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
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        EnsureSeedData(app).Wait();
    }

    public static async Task EnsureSeedData(WebApplication application)
    {
        var service = application.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(service);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
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
            await contextGestaoAlunos.Database.EnsureDeletedAsync();
            await contextGestaoConteudos.Database.EnsureDeletedAsync();
            await contextIdentity.Database.EnsureDeletedAsync();
            await contextPagamentoFaturamento.Database.EnsureDeletedAsync();

            await contextGestaoConteudos.Database.MigrateAsync();
            await contextGestaoAlunos.Database.MigrateAsync();
            await contextIdentity.Database.MigrateAsync();
            await contextPagamentoFaturamento.Database.MigrateAsync();

            await SeedUsersAndRoles(scope.ServiceProvider);
            await SeedDataInitial(contextGestaoAlunos, contextGestaoConteudos, contextIdentity, contextPagamentoFaturamento, certificadoService);
        }
    }

    private static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
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
            Email = "usuario@aluno.com",
            EmailConfirmed = true,
            UserName = "usuario@aluno.com",
        };

        var userAdmin = new IdentityUser
        {
            Email = "usuario@admin.com",
            EmailConfirmed = true,
            UserName = "usuario@admin.com",
        };

        await userManager.CreateAsync(userAluno, "Teste@123");
        await userManager.CreateAsync(userAdmin, "Teste@123");

        await userManager.AddToRoleAsync(userAluno, "ALUNO");
        await userManager.AddToRoleAsync(userAdmin, "ADMIN");
    }

    private static async Task SeedDataInitial(
         GestaoAlunosContext dbAlunosContext,
         GestaoConteudosContext dbConteudosContext,
         ApiContext dbApiContext,
         PagamentoFaturamentoContext dbPagamentoFaturamentoContext,
         ICertificadoPdfService pdfService)
    {
        if (dbAlunosContext.Set<Aluno>().Any() || dbAlunosContext.Set<Matricula>().Any() || dbAlunosContext.Set<Usuario>().Any())
            return;
        if (dbConteudosContext.Set<Curso>().Any() || dbConteudosContext.Set<Aula>().Any())
            return;

        var userAluno = await dbApiContext.Users.FirstOrDefaultAsync(x => x.Email == "usuario@aluno.com");
        var userAdmin = await dbApiContext.Users.FirstOrDefaultAsync(x => x.Email == "usuario@admin.com");

        var admin = new Usuario(Guid.Parse(userAdmin.Id));
        var aluno = new Aluno(Guid.Parse(userAluno.Id), "Aluno Exemplar");

        var curso = new Curso("Clean Code", "Código Limpo", admin.Id, 35000);
        var aulaBoasVindas = new Aula("Boas Vindas", "Bem vindo ao curso de Clean Code");
        var aulaComoFazerOCurso = new Aula("Como fazer o curso", "Dicas de como aproveitar melhor o curso");
        var aulaMaoNaMassa = new Aula("Mão na massa", "Hora de colocar a mão na massa");
        curso.AdicionarAula(aulaBoasVindas);
        curso.AdicionarAula(aulaComoFazerOCurso);
        curso.AdicionarAula(aulaMaoNaMassa);

        var matricula = new Matricula(aluno.Id, curso.Id);
        matricula.Ativar();
        matricula.Concluir();

        var progressoBoasVindas = new ProgressoAula(aluno.Id, aulaBoasVindas.Id);
        progressoBoasVindas.ConcluirAula();
        var progressoComoFazerOCurso = new ProgressoAula(aluno.Id, aulaComoFazerOCurso.Id);
        progressoComoFazerOCurso.ConcluirAula();
        var progressoMaoNaMassa = new ProgressoAula(aluno.Id, aulaMaoNaMassa.Id);
        progressoMaoNaMassa.ConcluirAula();

        var progressoCursoConcluido = new ProgressoCurso(curso.Id, aluno.Id, curso.Aulas.Count);
        progressoCursoConcluido.IncrementarProgresso();

        // Certificado para o aluno
        var certificado = new Certificado(aluno.Nome, curso.Nome, matricula.Id, aluno.Id, matricula.DataConclusao);
        var pdf = pdfService.GerarPdf(certificado);
        certificado.AdicionarArquivo(pdf);

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
        await dbConteudosContext.Set<ProgressoAula>().AddRangeAsync([progressoBoasVindas, progressoComoFazerOCurso, progressoMaoNaMassa]);
        await dbConteudosContext.Set<ProgressoCurso>().AddAsync(progressoCursoConcluido);

        await dbPagamentoFaturamentoContext.Set<Pagamento>().AddAsync(pagamento);
        await dbPagamentoFaturamentoContext.Set<Transacao>().AddAsync(transacao);

        await dbAlunosContext.SaveChangesAsync();
        await dbConteudosContext.SaveChangesAsync();
        await dbPagamentoFaturamentoContext.SaveChangesAsync();
    }
}