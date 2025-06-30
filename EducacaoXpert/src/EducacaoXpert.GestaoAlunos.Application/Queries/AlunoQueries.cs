using EducacaoXpert.GestaoAlunos.Application.Queries.DTO;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;

namespace EducacaoXpert.GestaoAlunos.Application.Queries;

public class AlunoQueries(IAlunoRepository alunoRepository) : IAlunoQueries
{
    public async Task<MatriculaDto?> ObterMatricula(Guid cursoId, Guid alunoId)
    {
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(cursoId, alunoId);

        if (matricula is null)
            return null;

        return new MatriculaDto
        {
            Id = matricula.Id,
            AlunoId = matricula.AlunoId,
            CursoId = matricula.CursoId,
            Status = matricula.Status,
            DataMatricula = matricula.DataMatricula
        };
    }

    public async Task<IEnumerable<MatriculaDto>> ObterMatriculasEmPagamento(Guid alunoId)
    {
        var matriculas = await alunoRepository.ObterMatriculasEmPagamento(alunoId);

        return matriculas.Select(m => new MatriculaDto
        {
            Id = m.Id,
            AlunoId = m.AlunoId,
            CursoId = m.CursoId,
            Status = m.Status,
            DataMatricula = m.DataMatricula
        }).ToList();
    }

    public async Task<CertificadoDto> ObterCertificado(Guid certificadoId, Guid alunoId)
    {
        var certificado = await alunoRepository.ObterCertificadoPorId(certificadoId, alunoId);

        return new CertificadoDto
        {
            Arquivo = certificado?.Arquivo ?? []
        };
    }
}
