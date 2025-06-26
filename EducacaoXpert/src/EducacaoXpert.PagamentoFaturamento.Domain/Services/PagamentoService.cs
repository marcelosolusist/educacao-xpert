using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.Core.Messages.IntegrationEvents;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Services;

public class PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                              IPagamentoRepository pagamentoRepository,
                              IMediator mediator) : IPagamentoService
{
    public async Task<bool> RealizarPagamentoCurso(PagamentoCurso pagamentoCurso)
    {
        var pedido = new Pedido
        {
            CursoId = pagamentoCurso.CursoId,
            AlunoId = pagamentoCurso.AlunoId,
            Valor = pagamentoCurso.Total,
        };

        var pagamento = new Pagamento
        {
            Valor = pagamentoCurso.Total,
            NomeCartao = pagamentoCurso.NomeCartao,
            NumeroCartao = pagamentoCurso.NumeroCartao,
            ExpiracaoCartao = pagamentoCurso.ExpiracaoCartao,
            CvvCartao = pagamentoCurso.CvvCartao,
            AlunoId = pagamentoCurso.AlunoId,
            CursoId = pagamentoCurso.CursoId
        };

        var transacao = pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

        if (transacao.StatusTransacao == StatusTransacao.Autorizado)
        {
            pagamento.AdicionarEvento(new CursoPagamentoRealizadoEvent(pagamento.CursoId, pagamento.AlunoId));

            pagamentoRepository.Adicionar(pagamento);
            pagamentoRepository.AdicionarTransacao(transacao);

            await pagamentoRepository.UnitOfWork.Commit();
            return true;
        }

        await mediator.Publish(new DomainNotification("pagamento", "A operadora recusou o pagamento"));
        return false;
    }
}
