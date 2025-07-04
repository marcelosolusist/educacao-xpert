using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.GestaoConteudos.Domain.Entities;

namespace EducacaoXpert.GestaoConteudos.Domain.Tests;

public class EntityCursoTests
{
    private readonly string NOME_CURSO = "Nome do Curso de Testes";
    private readonly string CONTEUDO_CURSO = "Conteúdo do Curso de Testes";
    private readonly Guid USUARIO_CRIACAO_ID = Guid.NewGuid();
    private readonly int PRECO_CURSO = 100;
    private readonly string NOME_AULA = "Nome da aula";
    private readonly string CONTEUDO_AULA = "Conteúdo da aula";
    private readonly Guid CURSO_ID = Guid.NewGuid();
    private readonly Guid ALUNO_ID = Guid.NewGuid();
    private readonly int TOTAL_AULAS = 3;
    private readonly Guid AULA_ID = Guid.NewGuid();

    [Fact(DisplayName = "Incluir Curso Dados Válidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirCursoDadosValidos_DevePassarNaValidacao()
    {
        // Arrange && Act
        var curso = new Curso(NOME_CURSO, CONTEUDO_CURSO, USUARIO_CRIACAO_ID, PRECO_CURSO);

        // Assert
        Assert.True(curso.Nome == NOME_CURSO);
        Assert.True(curso.Conteudo == CONTEUDO_CURSO);
        Assert.True(curso.UsuarioCriacaoId == USUARIO_CRIACAO_ID);
        Assert.True(curso.Preco == PRECO_CURSO);
    }
    [Fact(DisplayName = "Incluir Curso Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirCursoDadosInvalidos_DeveLancarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Curso(string.Empty, string.Empty, Guid.Empty, 0));
    }
    [Fact(DisplayName = "Incluir Aula ao Curso")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirAulaNoCursoDadosValidos_DevePassarNaValidacao()
    {
        // Arrange
        var curso = new Curso(NOME_CURSO, CONTEUDO_CURSO, USUARIO_CRIACAO_ID, PRECO_CURSO);
        var aula = new Aula(NOME_AULA, CONTEUDO_AULA);

        // Act
        curso.IncluirAula(aula);

        // Assert
        Assert.True(curso.Nome == NOME_CURSO);
        Assert.True(curso.Conteudo == CONTEUDO_CURSO);
        Assert.True(curso.UsuarioCriacaoId == USUARIO_CRIACAO_ID);
        Assert.True(aula.Nome == NOME_AULA);
        Assert.True(aula.Conteudo == CONTEUDO_AULA);
        Assert.Equal(1, curso.Aulas.Count(a => a.CursoId == curso.Id));
    }

    [Fact(DisplayName = "Incluir Aula Duplicada ao Curso")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirAulaDuplicadaNoCurso_DeveLancarDomainException()
    {
        // Arrange
        var curso = new Curso(NOME_CURSO, CONTEUDO_CURSO, USUARIO_CRIACAO_ID, PRECO_CURSO);
        var aula = new Aula(NOME_AULA, CONTEUDO_AULA);

        // Act
        curso.IncluirAula(aula);

        // Assert
        Assert.Throws<DomainException>(() => curso.IncluirAula(aula));
        Assert.Equal(1, curso.Aulas.Count(a => a.CursoId == curso.Id));
    }
    [Fact(DisplayName = "Incluir ProgressoCurso Dados Válidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirProgressoCursoDadosValidos_DevePassarNaValidacao()
    {
        // Arrange
        var progressoCurso = new ProgressoCurso(CURSO_ID, ALUNO_ID, TOTAL_AULAS);

        // Assert
        Assert.True(progressoCurso.CursoId == CURSO_ID);
        Assert.True(progressoCurso.AlunoId == ALUNO_ID);
        Assert.True(progressoCurso.TotalAulas == TOTAL_AULAS);
    }
    [Fact(DisplayName = "Incluir ProgressoCurso Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirProgressoAulaDadosInvalidos_NaoDevePassarNaValidacao()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new ProgressoAula(Guid.Empty));
    }
    [Fact(DisplayName = "Incluir ProgressoAula Dados Válidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirProgressoAulaDadosValidos_DevePassarNaValidacao()
    {
        // Arrange
        var progressoAula = new ProgressoAula(AULA_ID);

        // Assert
        Assert.True(progressoAula.AulaId == AULA_ID);
    }
    [Fact(DisplayName = "Incluir ProgressoAula Dados Inválidos")]
    [Trait("Categoria", "GestaoConteudos - Aula")]
    public void EntityAula_IncluirProgressoAulaInvalido_DeveLancarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new ProgressoAula(Guid.Empty));
    }
    [Fact(DisplayName = "Incluir ProgressoAula em ProgressoCurso Dados Válidos")]
    [Trait("Categoria", "GestaoConteudos - Curso")]
    public void EntityCurso_IncluirProgressoAulaEmProgressoCursoDadosValidos_DevePassarNaValidacao()
    {
        // Arrange
        var progressoCurso = new ProgressoCurso(CURSO_ID, ALUNO_ID, TOTAL_AULAS);
        var progressoAula = new ProgressoAula(AULA_ID);
        
        //Act
        progressoCurso.IncluirProgressoAula(progressoAula);

        // Assert
        Assert.True(progressoCurso.CursoId == CURSO_ID);
        Assert.True(progressoCurso.AlunoId == ALUNO_ID);
        Assert.True(progressoCurso.TotalAulas == TOTAL_AULAS);
        Assert.True(progressoAula.AulaId == AULA_ID);
        Assert.True(progressoCurso.ProgressoAulas.Select(pa => pa.Id == progressoAula.Id).Count() == 1);
    }
}
