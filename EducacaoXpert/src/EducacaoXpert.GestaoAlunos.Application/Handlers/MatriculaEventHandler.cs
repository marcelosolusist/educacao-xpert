using EducacaoXpert.Core.Messages.IntegrationEvents;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class MatriculaEventHandler(IMediator mediator)
                                    :   INotificationHandler<CursoPagamentoRealizadoEvent>,
                                        INotificationHandler<CursoConcluidoEvent>
{
    public async Task Handle(CursoPagamentoRealizadoEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AtivarMatriculaCommand(notification.AlunoId, notification.CursoId), cancellationToken);
    }

    public async Task Handle(CursoConcluidoEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new IncluirCertificadoCommand(notification.AlunoId, notification.CursoId, notification.NomeCurso), cancellationToken);
    }

}
