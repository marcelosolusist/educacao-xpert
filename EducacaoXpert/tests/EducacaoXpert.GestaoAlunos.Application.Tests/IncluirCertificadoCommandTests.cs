using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Handlers;
using EducacaoXpert.GestaoAlunos.Application.Services;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class IncluirCertificadoCommandTests
{
    private readonly AutoMocker _mocker;
    private readonly CertificadoCommandHandler _handler;
    private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
    private readonly Mock<ICertificadoPdfService> _certificadoPdfServiceMock;
    private readonly Aluno _aluno;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;
    private readonly Guid _matriculaId;
    private readonly string _nomeCurso;
    public IncluirCertificadoCommandTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CertificadoCommandHandler>();
        _alunoRepositoryMock = _mocker.GetMock<IAlunoRepository>();
        _certificadoPdfServiceMock = _mocker.GetMock<ICertificadoPdfService>();
        _aluno = new Aluno(Guid.NewGuid(), "Aluno de Testes");
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid();
        _nomeCurso = "Curso de testes";
    }

    [Fact(DisplayName = "Incluir Certificado Com Sucesso")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificadoCommand")]
    public async Task IncluirCertificadoCommand_IncluirCertificado_DeveIncluirComSucesso()
    {
        // Arrange
        var command = new IncluirCertificadoCommand(_alunoId, _cursoId, _nomeCurso);
        var matricula = new Matricula(_alunoId, _cursoId);
        matricula.Ativar();

        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
        _certificadoPdfServiceMock.Setup(r => r.GerarPdf(It.IsAny<Certificado>())).Returns(new byte[10]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.IncluirCertificado(It.IsAny<Certificado>()), Times.Once);
    }

    [Fact(DisplayName = "Criar Certificado - Aluno Inexistente")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificadoCommand")]
    public async Task IncluirCertificadoCommand_AlunoInexistente_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCertificadoCommand(_alunoId, _cursoId, _nomeCurso);
        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync((Aluno?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.IncluirCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Incluir Certificado - PDF não encontrado")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificadoCommand")]
    public async Task IncluirCertificadoCommand_IncluirCertificadoPdfNaoEncontrado_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCertificadoCommand(_alunoId, _cursoId, _nomeCurso);
        var matricula = new Matricula(_alunoId, _cursoId);

        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync(matricula);
        _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
        _certificadoPdfServiceMock.Setup(r => r.GerarPdf(It.IsAny<Certificado>())).Returns(new byte[0]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.IncluirCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Gerar PDF")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificadoCommand")]
    public void IncluirCertificadoCommand_GerarPdfValido_DevePassarNaValidacao()
    {
        // Arrange
        var certificado = new Certificado("Aluno de Testes", _nomeCurso, _alunoId);

        // Act
        var result = _mocker.CreateInstance<CertificadoPdfService>().GerarPdf(certificado);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Length > 0);
    }
}
