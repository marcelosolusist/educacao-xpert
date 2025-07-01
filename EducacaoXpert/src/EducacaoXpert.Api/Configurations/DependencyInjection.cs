using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoAlunos.Data.Context;
using EducacaoXpert.GestaoAlunos.Data.Repositories;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using EducacaoXpert.GestaoConteudos.Application.Queries;
using EducacaoXpert.GestaoConteudos.Data.Context;
using EducacaoXpert.GestaoConteudos.Data.Repositories;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Anticorruption.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Anticorruption;
using EducacaoXpert.PagamentoFaturamento.Data.Repository;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Services;
using MediatR;
using EducacaoXpert.GestaoAlunos.Application.Services;
using EducacaoXpert.PagamentoFaturamento.Data.Context;
using EducacaoXpert.Api.Extensions;

namespace EducacaoXpert.Api.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        // Notifications
        builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Gestão de Alunos
        builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<IAlunoQueries, AlunoQueries>();
        builder.Services.AddScoped<ICertificadoPdfService, CertificadoPdfService>();
        builder.Services.AddScoped<GestaoAlunosContext>();

        // Gestão de Conteúdos
        builder.Services.AddScoped<ICursoRepository, CursoRepository>();
        builder.Services.AddScoped<IProgressoCursoRepository, ProgressoCursoRepository>();
        builder.Services.AddScoped<ICursoQueries, CursoQueries>();
        builder.Services.AddScoped<GestaoConteudosContext>();

        // Mediator
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AtivarMatriculaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirAdminCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirAlunoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirCertificadoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirMatriculaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EditarAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ExcluirAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IniciarAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<FinalizarAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IncluirCursoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EditarCursoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ExcluirCursoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PagamentoService>());

        // Pagamentos
        builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        builder.Services.AddScoped<IPagamentoService, PagamentoService>();
        builder.Services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
        builder.Services.AddScoped<IPayPalGateway, PayPalGateway>();
        builder.Services.AddScoped<PagamentoFaturamentoContext>();

        //Identity User
        builder.Services.AddScoped<IAppIdentityUser, AppIdentityUser>();

        return builder;

    }
}