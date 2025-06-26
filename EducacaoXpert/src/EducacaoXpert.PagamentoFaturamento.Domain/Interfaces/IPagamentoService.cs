using EducacaoXpert.Core.DomainObjects.DTO;

namespace EducacaoXpert.PagamentoFaturamento.Domain.Interfaces;

public interface IPagamentoService
{
    Task<bool> RealizarPagamentoCurso(PagamentoCurso pagamentoCurso);
}
