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
    public async Task<bool> EfetuarPagamentoCurso(PagamentoCurso pagamentoCurso)
    {
        var pedido = new Pedido
        {
            CursoId = pagamentoCurso.CursoId,
            AlunoId = pagamentoCurso.AlunoId,
            Valor = pagamentoCurso.Valor,
        };

        var pagamento = new Pagamento
        {
            Valor = pagamentoCurso.Valor,
            NomeCartao = pagamentoCurso.NomeCartao,
            NumeroCartaoMascarado = MascararNumeroCartao(pagamentoCurso.NumeroCartao),
            AlunoId = pagamentoCurso.AlunoId,
            CursoId = pagamentoCurso.CursoId
        };

        pagamentoCurso.PagamentoId = pagamento.Id;

        var transacao = pagamentoCartaoCreditoFacade.EfetuarPagamento(pedido, pagamentoCurso);

        if (transacao.StatusTransacao == StatusTransacao.Autorizado)
        {
            pagamento.IncluirEvento(new CursoPagamentoRealizadoEvent(pagamento.CursoId, pagamento.AlunoId));

            pagamentoRepository.Incluir(pagamento);
            pagamentoRepository.IncluirTransacao(transacao);

            await pagamentoRepository.UnitOfWork.Commit();
            return true;
        }

        await mediator.Publish(new DomainNotification("pagamento", "A operadora recusou o pagamento"));
        return false;
    }

    private string MascararNumeroCartao(string numeroCartao)
    {
        return numeroCartao[..4]+"********"+numeroCartao[4..];
    }
}
