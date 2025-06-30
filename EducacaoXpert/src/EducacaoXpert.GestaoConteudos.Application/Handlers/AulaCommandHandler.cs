using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Application.Events;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoConteudos.Application.Handlers;

public class AulaCommandHandler(IMediator mediator,
                                ICursoRepository cursoRepository,
                                IProgressoCursoRepository progressoCursoRepository) : CommandHandler,
                                IRequestHandler<IncluirAulaCommand, bool>
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
