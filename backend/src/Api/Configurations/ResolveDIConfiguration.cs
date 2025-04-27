using Api.Extensions;
using Business.Interfaces;
using Business.Notificacoes;
using Business.Services;
using Data.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Api.Configurations
{
    public static class ResolveDiConfiguration
    {
        public static WebApplicationBuilder AddResolveDependencie(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            builder.Services.AddScoped<INotificador, Notificador>();

            builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
            builder.Services.AddScoped<IAulaRepository, AulaRepository>();
            builder.Services.AddScoped<IAulaAssistidaRepository, AulaAssistidaRepository>();
            builder.Services.AddScoped<ICertificadoRepository, CertificadoRepository>();
            builder.Services.AddScoped<ICursoRepository, CursoRepository>();
            builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            builder.Services.AddScoped<IPagamentoHistoricoRepository, PagamentoHistoricoRepository>();

            //builder.Services.AddScoped<IAlunoService, AlunoService>();
            builder.Services.AddScoped<IAulaService, AulaService>();
            //builder.Services.AddScoped<IAulaAssistidaService, AulaAssistidaService>();
            //builder.Services.AddScoped<ICertificadoService, CertificadoService>();
            //builder.Services.AddScoped<ICursoService, CursoService>();
            //builder.Services.AddScoped<IMatriculaService, MatriculaService>();
            //builder.Services.AddScoped<IPagamentoService, PagamentoService>();
            //builder.Services.AddScoped<IPagamentoHistoricoService, PagamentoHistoricoService>();

            return builder;
        }
    }
}
