using EducacaoXpert.GestaoAlunos.Application.Queries.DTO;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using Org.BouncyCastle.Ocsp;

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

    public async Task<IEnumerable<MatriculaDto>> ObterMatriculasAPagar(Guid alunoId)
    {
        var matriculas = await alunoRepository.ObterMatriculasAPagar(alunoId);

        return matriculas.Select(m => new MatriculaDto
        {
            Id = m.Id,
            AlunoId = m.AlunoId,
            CursoId = m.CursoId,
            Status = m.Status,
            DataMatricula = m.DataMatricula
        }).ToList();
    }

    public async Task<ArquivoCertificadoDto> ObterCertificado(Guid certificadoId, Guid alunoId)
    {
        var certificado = await alunoRepository.ObterCertificadoPorId(certificadoId, alunoId);

        return new ArquivoCertificadoDto
        {
            Arquivo = certificado?.Arquivo ?? []
        };
    }

    public async Task<IEnumerable<CertificadoDto>> ListarCertificados(Guid alunoId)
    {
        var certificados = await alunoRepository.ListarCertificadosPorAlunoId(alunoId);
        var certificadosDto = new List<CertificadoDto>();
        foreach (var certificado in certificados)
        {
            certificadosDto.Add(new CertificadoDto()
            {
                Id = certificado.Id,
                NomeAluno = certificado.NomeAluno,
                NomeCurso = certificado.NomeCurso,
                DataEmissao = certificado.DataEmissao,
            });
        }
        return certificadosDto;
    }
}
