using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.GestaoAlunos.Domain.Entities;

namespace EducacaoXpert.GestaoAlunos.Domain.Tests;

public class EntityAlunoTests
{
    private readonly string NOME_ALUNO = "Aluno de Testes";
    private readonly string NOME_CURSO = "Curso de Testes";

    [Fact(DisplayName = "EntityAluno - Incluir Matricula")]
    [Trait("Categoria", "GestaoAlunos - IncluirMatricula")]
    public void EntityAluno_IncluirMatriculaAPagar_DevePassarNaValidacao()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), NOME_ALUNO);
        var cursoId = Guid.NewGuid();
        var matricula = new Matricula(aluno.Id, cursoId);

        // Act
        aluno.IncluirMatricula(matricula);

        // Assert
        Assert.Single(aluno.Matriculas);
        Assert.Equal(StatusMatricula.APagar, matricula.Status);
    }
    [Fact(DisplayName = "EntityAluno - Incluir Matricula Existente")]
    [Trait("Categoria", "GestaoAlunos - IncluirMatricula")]
    public void EntityAluno_IncluirMatriculaDuplicada_DeveLancarDomainException()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), NOME_ALUNO); ;
        var cursoId = Guid.NewGuid();
        var matricula = new Matricula(aluno.Id, cursoId);

        // Act
        aluno.IncluirMatricula(matricula);

        // Assert
        Assert.Throws<DomainException>(() => aluno.IncluirMatricula(matricula));
        Assert.Single(aluno.Matriculas);
    }

    [Fact(DisplayName = "EntityAluno - Incluir Certificado")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificado")]
    public void EntityAluno_IncluirCertificadoValido_DeveIncluirNormalmente()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), NOME_ALUNO);
        var certificado = new Certificado(aluno.Nome, NOME_CURSO, aluno.Id);

        // Act
        aluno.IncluirCertificado(certificado);

        // Assert
        Assert.Single(aluno.Certificados);
    }

    [Fact(DisplayName = "EntityAluno - Gerar Descrição do Certificado")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificado")]
    public void EntityAluno_IncluirCertificadoValido_DeveGerarDescricao()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(),NOME_ALUNO);
        var certificado = new Certificado(aluno.Nome, NOME_CURSO, aluno.Id);

        // Act
        aluno.IncluirCertificado(certificado);

        // Assert
        Assert.Contains(aluno.Nome, certificado.Descricao);
        Assert.Contains(NOME_CURSO, certificado.Descricao);
    }

    [Fact(DisplayName = "EntityAluno - Incluir Certificado Invalido")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificado")]
    public void EntityAluno_IncluirCertificadoInvalido_DeveGerarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Certificado("", "", Guid.Empty));
    }

    [Fact(DisplayName = "EntityAluno - Incluir Certificado Existente")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificado")]
    public void EntityAluno_IncluirCertificadoDuplicado_DeveGerarDomainException()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), NOME_ALUNO);
        var certificado = new Certificado(aluno.Nome, NOME_CURSO, aluno.Id);

        // Act
        aluno.IncluirCertificado(certificado);

        // Assert
        Assert.Throws<DomainException>(() => aluno.IncluirCertificado(certificado));
        Assert.Single(aluno.Certificados);
    }

    [Fact(DisplayName = "EntityAluno - Incluir Certificado Sem Arquivo")]
    [Trait("Categoria", "GestaoAlunos - IncluirCertificado")]
    public void EntityAluno_IncluirCertificadoSemArquivo_DeveGerarDomainException()
    {
        // Arrange
        var aluno = new Aluno(Guid.NewGuid(), NOME_ALUNO);
        var certificado = new Certificado(aluno.Nome, NOME_CURSO, aluno.Id);

        // Act && Assert
        Assert.Throws<DomainException>(() => certificado.IncluirArquivo(null));
    }
}
