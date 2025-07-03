using EducacaoXpert.Core.DomainObjects.DTO;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Tests;

public class PagamentoCursoFixture
{
    public PagamentoCurso GerarPagamentoCursoValido()
    {
        return new PagamentoCurso()
        {
            AlunoId = Guid.NewGuid(),
            CursoId = Guid.NewGuid(),
            NomeCartao = "Aluno de Testes",
            NumeroCartao = "5460717501008669",
            ExpiracaoCartao = "08/32",
            CvvCartao = "987",
            Valor = 100
        };
    }

    public PagamentoCurso GerarPagamentoCursoInvalido()
    {
        return new PagamentoCurso()
        {
            AlunoId = Guid.Empty,
            CursoId = Guid.Empty,
            NomeCartao = string.Empty,
            NumeroCartao = "9865487983",
            ExpiracaoCartao = string.Empty,
            CvvCartao = string.Empty,
            Valor = 0
        };
    }
}
