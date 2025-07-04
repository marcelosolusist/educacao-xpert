using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.GestaoAlunos.Domain.Entities;

namespace EducacaoXpert.GestaoAlunos.Domain.Tests;

public class EntityMatriculaTests
{

    [Fact(DisplayName = "EntityMatricula - Incluir Matricula")]
    [Trait("Categoria", "GestaoAlunos - IncluirMatricula")]
    public void EntityMatricula_IncluirMatriculaInvalida_DeveLancarDomainException()
    {
        // Arrange && Act && Assert
        Assert.Throws<DomainException>(() => new Matricula(Guid.Empty, Guid.Empty));
    }
}
