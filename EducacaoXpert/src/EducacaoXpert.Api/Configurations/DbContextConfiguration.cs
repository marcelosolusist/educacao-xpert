using EducacaoXpert.Api.Context;
using EducacaoXpert.GestaoAlunos.Data.Context;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.PagamentoFaturamento.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.Api.Configurations;

public static class DbContextConfiguration
{
    public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.Services.AddDbContext<GestaoConteudosContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<GestaoAlunosContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);
            builder.Services.AddDbContext<ApiContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<PagamentoFaturamentoContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
        else
        {
            builder.Services.AddDbContext<GestaoConteudosContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<GestaoAlunosContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);
            builder.Services.AddDbContext<ApiContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<PagamentoFaturamentoContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        return builder;
    }
}