using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using Moq;
using Moq.AutoMock;

namespace EducacaoXpert.GestaoAlunos.Application.Tests;

public class AlunoRepositoryTests
{
    private readonly AutoMocker _mocker;
    private readonly AlunoQueries _query;
    private readonly Guid _cursoId;
    private readonly Guid _alunoId;

    public AlunoRepositoryTests()
    {
        _mocker = new AutoMocker();
        _query = _mocker.CreateInstance<AlunoQueries>();
        _cursoId = Guid.NewGuid();
        _alunoId = Guid.NewGuid();
    }

    [Fact(DisplayName = "Obter Matricula")]
    [Trait("Categoria", "GestaoAlunos - AlunoRepository")]
    public async Task AlunoRepository_ObterMatriculaPorCursoEAlunoId_DeveRetornarAlunoECursoId()
    {
        // Arrange
        var matricula = new Matricula(_alunoId, _cursoId);
        matricula.AguardarPagamento();
        _mocker.GetMock<IAlunoRepository>()
            .Setup(q => q.ObterMatriculaPorCursoEAlunoId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(matricula);

        // Act
        var result = await _query.ObterMatricula(_cursoId, _alunoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_alunoId, result.AlunoId);
        Assert.Equal(_cursoId, result.CursoId);
    }

    [Fact(DisplayName = "Obter Matriculas APagar")]
    [Trait("Categoria", "GestaoAlunos - AlunoRepository")]
    public async Task AlunoRepository_ObterMatriculasAPagar_DeveRetornarMatriculasAPagar()
    {
        // Arrange
        var matriculas = new List<Matricula>()
        {
            new(_alunoId, _cursoId)
        };
        matriculas[0].AguardarPagamento();

        _mocker.GetMock<IAlunoRepository>()
            .Setup(q => q.ObterMatriculasAPagar(It.IsAny<Guid>()))
            .ReturnsAsync(matriculas);

        // Act
        var result = await _query.ObterMatriculasAPagar(_alunoId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Collection(result, (item) =>
        {
            Assert.Equal(item.AlunoId, _alunoId);
            Assert.Equal(item.CursoId, _cursoId);
            Assert.Equal(StatusMatricula.APagar, item.Status);
        });
    }
}
