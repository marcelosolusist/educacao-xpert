using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.IntegrationEvents;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoConteudos.Application.Handlers;

public class AulaCommandHandler(IMediator mediator,
                                ICursoRepository cursoRepository,
                                IProgressoCursoRepository progressoCursoRepository) : CommandHandler,
                                IRequestHandler<IncluirAulaCommand, bool>,
                                IRequestHandler<EditarAulaCommand, bool>,
                                IRequestHandler<ExcluirAulaCommand, bool>,
                                IRequestHandler<IniciarAulaCommand, bool>,
                                IRequestHandler<FinalizarAulaCommand, bool>,
                                IRequestHandler<AssistirAulaCommand, bool>
{
    public async Task<bool> Handle(IncluirAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var curso = await cursoRepository.ObterPorId(command.CursoId);

        if (curso is null)
        {
            await IncluirNotificacao(command.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        var aula = new Aula(command.Nome, command.Conteudo);
        aula.AssociarCurso(command.CursoId);

        if (command is { NomeMaterial: not null, TipoMaterial: not null })
            aula.IncluirMaterial(new Material(command.NomeMaterial, command.TipoMaterial));

        cursoRepository.IncluirAula(aula);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(EditarAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var aula = await cursoRepository.ObterAulaPorId(command.AulaId);
        if (aula is null)
        {
            await IncluirNotificacao(command.MessageType, "Aula não encontrada.", cancellationToken);
            return false;
        }

        aula.EditarNome(command.Nome);
        aula.EditarConteudo(command.Conteudo);
        if (command is { NomeMaterial: not null, TipoMaterial: not null })
            aula.IncluirMaterial(new Material(command.NomeMaterial, command.TipoMaterial));

        cursoRepository.IncluirAula(aula);
        return await cursoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(ExcluirAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var aula = await cursoRepository.ObterAulaPorId(command.AulaId);
        if (aula is null)
        {
            await IncluirNotificacao(command.MessageType, "Aula não encontrada.", cancellationToken);
            return false;
        }

        if (aula.Materiais.Any())
        {
            await IncluirNotificacao(command.MessageType, "Aula não pode ser excluída pois possui materiais associados.", cancellationToken);
            return false;
        }

        cursoRepository.RemoverAula(aula);
        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(IniciarAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var progressoCurso = await progressoCursoRepository.Obter(command.CursoId, command.AlunoId);

        if (progressoCurso is null)
        {
            var curso = await cursoRepository.ObterCursoComAulas(command.CursoId);
            progressoCurso = new ProgressoCurso(command.CursoId, command.AlunoId, curso.Aulas.Count());
            progressoCursoRepository.Incluir(progressoCurso);
            await cursoRepository.UnitOfWork.Commit();
            progressoCurso = await progressoCursoRepository.Obter(command.CursoId, command.AlunoId);
        }

        var progressoAula = new ProgressoAula(command.AulaId);
        progressoCurso.IncluirProgressoAula(progressoAula);

        progressoCursoRepository.IncluirProgressoAula(progressoAula);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AssistirAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var progressoCurso = await progressoCursoRepository.Obter(command.CursoId, command.AlunoId);

        if (progressoCurso is null)
        {
            await IncluirNotificacao(command.MessageType, "Progresso do curso não encontrado.", cancellationToken);
            return false;
        }

        var progressoAula = await progressoCursoRepository.ObterProgressoAula(command.AulaId, command.AlunoId);
        if (progressoAula is null)
        {
            await IncluirNotificacao(command.MessageType, "Progresso de aula não encontrado.", cancellationToken);
            return false;
        }
        progressoCurso.MarcarAulaAssistindo(progressoAula);

        progressoCursoRepository.Editar(progressoCurso);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(FinalizarAulaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var progressoCurso = await progressoCursoRepository.Obter(command.CursoId, command.AlunoId);

        if (progressoCurso is null)
        {
            await IncluirNotificacao(command.MessageType, "Progresso do curso não encontrado.", cancellationToken);
            return false;
        }

        var progressoAula = await progressoCursoRepository.ObterProgressoAula(command.AulaId, command.AlunoId);
        if (progressoAula is null)
        {
            await IncluirNotificacao(command.MessageType, "Progresso de aula não encontrado.", cancellationToken);
            return false;
        }
        progressoCurso.FinalizarProgressoAula(progressoAula);

        if (progressoCurso.CursoConcluido && !progressoCurso.CertificadoGerado)
        {
            await mediator.Publish(new CursoConcluidoEvent(command.AlunoId, command.CursoId, progressoCurso.Curso.Nome));
            progressoCurso.MarcarCertificadoGerado();
        }

        progressoCursoRepository.Editar(progressoCurso);

        return await cursoRepository.UnitOfWork.Commit();
    }

    protected override async Task IncluirNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
    {
        await mediator.Publish(new DomainNotification(messageType, descricao), cancellationToken);
    }

    private bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;
        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}
