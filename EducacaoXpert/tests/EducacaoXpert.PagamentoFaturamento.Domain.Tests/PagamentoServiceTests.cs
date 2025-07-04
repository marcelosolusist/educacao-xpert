using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.PagamentoFaturamento.Domain.Commands;
using EducacaoXpert.PagamentoFaturamento.Domain.Entities;
using EducacaoXpert.PagamentoFaturamento.Domain.Enums;
using EducacaoXpert.PagamentoFaturamento.Domain.Handlers;
using EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Services;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Tests;

public class PagamentoServiceTests
{
    private readonly AutoMocker _mocker;
    private readonly PagamentoCommandHandler _handler;
    private readonly PagamentoCursoFixture _pagamentoCursoFixture;

    public PagamentoServiceTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<PagamentoCommandHandler>();
        _pagamentoCursoFixture = new PagamentoCursoFixture();
    }
    [Fact(DisplayName = "Realizar Pagamento com Sucesso")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public async Task PagamentoService_RealizarPagamentoCursoValido_DeveSalvarDados()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var pagamentoService = _mocker.CreateInstance<PagamentoService>();
        _mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
            .ReturnsAsync(true);
        _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
            .Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<PagamentoCurso>()))
            .Returns(new Transacao { StatusTransacao = StatusTransacao.Autorizado });
        _mocker.GetMock<IPagamentoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Incluir(It.IsAny<Pagamento>()));
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.IncluirTransacao(It.IsAny<Transacao>()));
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit());
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
            Times.Never);
    }
    [Fact(DisplayName = "Realizar Pagamento com Falha")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public async Task PagamentoService_RealizarPagamentoCursoInvalido_NaoDeveSalvarDados()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var pagamentoService = _mocker.CreateInstance<PagamentoService>();
        _mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
            .ReturnsAsync(false);
        _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
            .Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<PagamentoCurso>()))
            .Returns(new Transacao { StatusTransacao = StatusTransacao.Negado });

        // Act
        var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Incluir(It.IsAny<Pagamento>()), Times.Never);
        _mocker.GetMock<IPagamentoRepository>()
            .Verify(r => r.IncluirTransacao(It.IsAny<Transacao>()), Times.Never);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
            Times.Once);
    }
    [Fact(DisplayName = "Realizar Pagamento Command Válido")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public void PagamentoService_RealizarPagamentoCursoCommandValido_DevePassarNaValidacao()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var command = new RealizarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }
    [Fact(DisplayName = "Realizar Pagamento Command Inválido")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public void PagamentoService_RealizarPagamentoCursoCommandInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoInvalido();

        var command = new RealizarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Contains(RealizarPagamentoCursoCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.NomeCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.NumeroCartaoInvalidoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.ExpiracaoCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.CvvCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(RealizarPagamentoCursoCommandValidation.ValorErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(7, command.ValidationResult.Errors.Count);
    }
    [Fact(DisplayName = "Realizar Pagamento CommandHandler Sucesso")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public async Task PagamentoService_RealizarPagamentoCursoValido_DeveSalvarPagamento()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var command = new RealizarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPagamentoService>()
            .Verify(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Once);
    }
    [Fact(DisplayName = "Realizar Pagamento CommandHandler Falha")]
    [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
    public async Task PagamentoService_RealizarPagamentoCursoInvalido_NaoDeveSalvarPagamento()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoInvalido();

        var command = new RealizarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPagamentoService>()
            .Verify(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Never);
    }
}
