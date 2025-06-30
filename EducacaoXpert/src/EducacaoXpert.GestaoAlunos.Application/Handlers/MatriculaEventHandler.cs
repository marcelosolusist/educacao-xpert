using EducacaoXpert.Core.Messages.IntegrationEvents;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class MatriculaEventHandler(IMediator mediator)
    : INotificationHandler<CursoPagamentoRealizadoEvent>,
      INotificationHandler<MatriculaConcluidaEvent>
{
    public async Task Handle(CursoPagamentoRealizadoEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new AtivarMatriculaCommand(notification.AlunoId, notification.CursoId), cancellationToken);
    }

    public async Task Handle(MatriculaConcluidaEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new IncluirCertificadoCommand(notification.AlunoId, notification.MatriculaId, notification.CursoId, notification.NomeCurso), cancellationToken);
    }
}
