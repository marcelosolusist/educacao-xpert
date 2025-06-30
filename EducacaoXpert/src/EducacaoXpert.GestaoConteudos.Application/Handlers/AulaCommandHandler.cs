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
                                IRequestHandler<AdicionarAulaCommand, bool>
{
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var curso = await cursoRepository.ObterPorId(request.CursoId);

        if (curso is null)
        {
            await AdicionarNotificacao(request.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        var aula = new Aula(request.Nome, request.Conteudo);
        aula.AssociarCurso(request.CursoId);

        if (request is { NomeMaterial: not null, TipoMaterial: not null })
            aula.AdicionarMaterial(new Material(request.NomeMaterial, request.TipoMaterial));

        cursoRepository.AdicionarAula(aula);

        return await cursoRepository.UnitOfWork.Commit();
    }

    protected override async Task AdicionarNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
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
