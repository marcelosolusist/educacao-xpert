using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.Core.Messages;
using EducacaoXpert.PagamentoFaturamento.Domain.Commands;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Handlers;

public class PagamentoCommandHandler(IPagamentoService pagamentoService, IMediator mediator) : CommandHandler, IRequestHandler<RealizarPagamentoCursoCommand, bool>
{
    public async Task<bool> Handle(RealizarPagamentoCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var pagamentoCurso = new PagamentoCurso
        {
            AlunoId = command.AlunoId,
            CursoId = command.CursoId,
            CvvCartao = command.CvvCartao,
            ExpiracaoCartao = command.ExpiracaoCartao,
            NomeCartao = command.NomeCartao,
            NumeroCartao = command.NumeroCartao,
            Valor = command.Valor
        };

        return await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);
    }

    private bool ValidarComando(Command command)
    {
        if (command.EhValido())
            return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage), CancellationToken.None);
        }
        return false;
    }
}
