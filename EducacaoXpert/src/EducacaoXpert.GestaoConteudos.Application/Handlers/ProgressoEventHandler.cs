using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Application.Events;
using MediatR;

namespace EducacaoXpert.GestaoConteudos.Application.Handlers;

public class ProgressoEventHandler(IMediator mediator) : INotificationHandler<AulaConcluidaEvent>
{
    public async Task Handle(AulaConcluidaEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AtualizarProgressoCursoCommand(notification.CursoId, notification.AlunoId), cancellationToken);
    }
}
