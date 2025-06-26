﻿using EducacaoXpert.GestaoAlunos.Application.Queries.ViewModels;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;

namespace EducacaoXpert.GestaoAlunos.Application.Queries;

public class AlunoQueries(IAlunoRepository alunoRepository) : IAlunoQueries
{
    public async Task<MatriculaViewModel?> ObterMatricula(Guid cursoId, Guid alunoId)
    {
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(cursoId, alunoId);

        if (matricula is null)
            return null;

        return new MatriculaViewModel
        {
            Id = matricula.Id,
            AlunoId = matricula.AlunoId,
            CursoId = matricula.CursoId,
            Status = matricula.Status,
            DataMatricula = matricula.DataMatricula
        };
    }

    public async Task<IEnumerable<MatriculaViewModel>> ObterMatriculasPendentePagamento(Guid alunoId)
    {
        var matriculas = await alunoRepository.ObterMatriculasPendentePagamento(alunoId);

        return matriculas.Select(m => new MatriculaViewModel
        {
            Id = m.Id,
            AlunoId = m.AlunoId,
            CursoId = m.CursoId,
            Status = m.Status,
            DataMatricula = m.DataMatricula
        }).ToList();
    }

    public async Task<CertificadoViewModel> ObterCertificado(Guid certificadoId, Guid alunoId)
    {
        var certificado = await alunoRepository.ObterCertificadoPorId(certificadoId, alunoId);

        return new CertificadoViewModel
        {
            Arquivo = certificado?.Arquivo ?? []
        };
    }
}
