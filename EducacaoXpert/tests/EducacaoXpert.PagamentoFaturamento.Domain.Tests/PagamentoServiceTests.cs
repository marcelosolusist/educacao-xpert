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
    [Fact(DisplayName = "Efetuar Pagamento com Sucesso")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public async Task PagamentoService_EfetuarPagamentoCursoValido_DeveSalvarDados()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var pagamentoService = _mocker.CreateInstance<PagamentoService>();
        _mocker.GetMock<IPagamentoService>().Setup(p => p.EfetuarPagamentoCurso(It.IsAny<PagamentoCurso>()))
            .ReturnsAsync(true);
        _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
            .Setup(p => p.EfetuarPagamento(It.IsAny<Pedido>(), It.IsAny<PagamentoCurso>()))
            .Returns(new Transacao { StatusTransacao = StatusTransacao.Autorizado });
        _mocker.GetMock<IPagamentoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        var result = await pagamentoService.EfetuarPagamentoCurso(pagamentoCurso);

        // Assert
        Assert.True(result);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Incluir(It.IsAny<Pagamento>()));
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.IncluirTransacao(It.IsAny<Transacao>()));
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit());
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
            Times.Never);
    }
    [Fact(DisplayName = "Efetuar Pagamento com Falha")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public async Task PagamentoService_EfetuarPagamentoCursoInvalido_NaoDeveSalvarDados()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var pagamentoService = _mocker.CreateInstance<PagamentoService>();
        _mocker.GetMock<IPagamentoService>().Setup(p => p.EfetuarPagamentoCurso(It.IsAny<PagamentoCurso>()))
            .ReturnsAsync(false);
        _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
            .Setup(p => p.EfetuarPagamento(It.IsAny<Pedido>(), It.IsAny<PagamentoCurso>()))
            .Returns(new Transacao { StatusTransacao = StatusTransacao.Negado });

        // Act
        var result = await pagamentoService.EfetuarPagamentoCurso(pagamentoCurso);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Incluir(It.IsAny<Pagamento>()), Times.Never);
        _mocker.GetMock<IPagamentoRepository>()
            .Verify(r => r.IncluirTransacao(It.IsAny<Transacao>()), Times.Never);
        _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
            Times.Once);
    }
    [Fact(DisplayName = "Efetuar Pagamento Command Válido")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public void PagamentoService_EfetuarPagamentoCursoCommandValido_DevePassarNaValidacao()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var command = new EfetuarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.True(result);
    }
    [Fact(DisplayName = "Efetuar Pagamento Command Inválido")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public void PagamentoService_EfetuarPagamentoCursoCommandInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoInvalido();

        var command = new EfetuarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        var result = command.EhValido();

        // Assert
        Assert.False(result);
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.NomeCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.NumeroCartaoInvalidoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.ExpiracaoCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.CvvCartaoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(EfetuarPagamentoCursoCommandValidation.ValorErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(7, command.ValidationResult.Errors.Count);
    }
    [Fact(DisplayName = "Efetuar Pagamento CommandHandler Sucesso")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public async Task PagamentoService_EfetuarPagamentoCursoValido_DeveSalvarPagamento()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoValido();

        var command = new EfetuarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPagamentoService>()
            .Verify(p => p.EfetuarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Once);
    }
    [Fact(DisplayName = "Efetuar Pagamento CommandHandler Falha")]
    [Trait("Categoria", "Pagamentos - EfetuarPagamentoCurso")]
    public async Task PagamentoService_EfetuarPagamentoCursoInvalido_NaoDeveSalvarPagamento()
    {
        // Arrange
        var pagamentoCurso = _pagamentoCursoFixture.GerarPagamentoCursoInvalido();

        var command = new EfetuarPagamentoCursoCommand(pagamentoCurso.AlunoId, pagamentoCurso.CursoId,
            pagamentoCurso.CvvCartao, pagamentoCurso.ExpiracaoCartao, pagamentoCurso.NomeCartao,
            pagamentoCurso.NumeroCartao, pagamentoCurso.Valor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mocker.GetMock<IPagamentoService>()
            .Verify(p => p.EfetuarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Never);
    }
}
