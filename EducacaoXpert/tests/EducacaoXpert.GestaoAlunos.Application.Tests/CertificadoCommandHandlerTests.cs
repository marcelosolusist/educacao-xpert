using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Handlers;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using MediatR;
using Moq.AutoMock;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Services;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class CertificadoCommandHandlerTests
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
    public CertificadoCommandHandlerTests()
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
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task CertificadoCommandHandler_IncluirCertificado_DeveIncluirComSucesso()
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

    [Fact(DisplayName = "Incluir Certificado Command inválido")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task CertificadoCommandHandler_ComandoEstaInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCertificadoCommand(Guid.Empty, Guid.Empty, string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Never);
        _alunoRepositoryMock.Verify(r => r.IncluirCertificado(It.IsAny<Certificado>()), Times.Never);
        Assert.Contains(GerarCertificadoCommandValidator.CursoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(GerarCertificadoCommandValidator.AlunoIdErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Contains(GerarCertificadoCommandValidator.NomeCursoErro,
            command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        Assert.Equal(4, command.ValidationResult.Errors.Count);
    }

    [Fact(DisplayName = "Criar Certificado - Aluno Inexistente")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task CertificadoCommandHandler_AlunoInexistente_NaoDevePassarNaValidacao()
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

    [Fact(DisplayName = "Criar Certificado - Matricula Inexistente")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task CertificadoCommandHandler_MatriculaInexistente_NaoDevePassarNaValidacao()
    {
        // Arrange
        var command = new IncluirCertificadoCommand(_alunoId, _cursoId, _nomeCurso);
        _alunoRepositoryMock.Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync(_aluno);
        _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId)).ReturnsAsync((Matricula?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        _alunoRepositoryMock.Verify(r => r.ObterPorId(command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.ObterMatriculaPorCursoEAlunoId(command.CursoId, command.AlunoId), Times.Once);
        _alunoRepositoryMock.Verify(r => r.IncluirCertificado(It.IsAny<Certificado>()), Times.Never);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Incluir Certificado - PDF não encontrado")]
    [Trait("Categoria", "GestaoAlunos - CertificadoCommandHandler")]
    public async Task CertificadoCommandHandler_IncluirCertificadoPdfNaoEncontrado_NaoDevePassarNaValidacao()
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
    [Trait("Categoria", "GestaoAlunos - CertificadoPdfService")]
    public void CertificadoService_GerarPdfInvalido_NaoDevePassarNaValidacao()
    {
        // Arrange
        var certificado = new Certificado("Aluno de Testes", _nomeCurso, _alunoId);

        // Act
        var result = _mocker.CreateInstance<CertificadoPdfService>().GerarPdf(certificado);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Length > 100, "O PDF gerado é muito pequeno e pode estar inválido.");

        var filePath = Path.Combine("C:\\Temp", "certificado_teste.pdf");
        File.WriteAllBytes(filePath, result);

        Assert.True(File.Exists(filePath));
    }
}
