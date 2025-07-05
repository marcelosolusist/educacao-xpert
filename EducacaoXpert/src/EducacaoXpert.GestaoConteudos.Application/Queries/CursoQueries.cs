using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Application.Queries;

public class CursoQueries(ICursoRepository cursoRepository,
                            IProgressoCursoRepository progressoCursoRepository) : ICursoQueries
{
    public async Task<CursoDto?> ObterPorId(Guid cursoId)
    {
        var curso = await cursoRepository.ObterPorId(cursoId);

        if (curso is null)
            return null;

        return new CursoDto
        {
            Id = curso.Id,
            Nome = curso.Nome,
            Conteudo = curso.Conteudo,
            Preco = curso.Preco,
            Aulas = curso.Aulas?.Select(a => new AulaDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList() ?? []
        };
    }

    

    public async Task<IEnumerable<CursoDto>> ListarTodos()
    {
        var cursos = await cursoRepository.ListarTodos();

        return cursos.Select(c => new CursoDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Conteudo = c.Conteudo,
            Preco = c.Preco,
            Aulas = c.Aulas?.Select(a => new AulaDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Conteudo = a.Conteudo
            }).ToList() ?? []
        }).ToList();
    }

    public async Task<IEnumerable<AulaDto>> ListarTodasAulasPorCursoId(Guid cursoId)
    {
        var aulas = await cursoRepository.ListarTodasAulasPorCursoId(cursoId);

        return aulas.Select(a => new AulaDto
        {
            Id = a.Id,
            Nome = a.Nome,
            Conteudo = a.Conteudo,
        }).ToList();
    }

    public async Task<ProgressoCursoDto?> ObterProgressoCurso(Guid cursoId, Guid alunoId)
    {
        var progressoCurso = await progressoCursoRepository.Obter(cursoId, alunoId);

        if (progressoCurso is null)
            return null;

        return new ProgressoCursoDto
        {
            Id = progressoCurso.Id,
            AlunoId = progressoCurso.AlunoId,
            CursoId = progressoCurso.CursoId,
            AulasFinalizadas = progressoCurso.AulasFinalizadas,
            TotalAulas = progressoCurso.TotalAulas,
            PercentualConcluido = progressoCurso.PercentualConcluido,
            CertificadoGerado = progressoCurso.CertificadoGerado,
            ProgressoAulas = progressoCurso.ProgressoAulas?.Select(a => new ProgressoAulaDto
            {
                Id = a.Id,
                Assistindo = a.Assistindo,
                AulaId = a.AulaId,
                ProgressoCursoId = a.ProgressoCursoId,
                Status = a.Status
            }).ToList() ?? []
        };
    }

    public async Task<ProgressoAulaDto?> ObterProgressoAula(Guid aulaId, Guid alunoId)
    {
        var progressoAula = await progressoCursoRepository.ObterProgressoAula(aulaId, alunoId);

        if (progressoAula is null)
            return null;

        return new ProgressoAulaDto
        {
            Id = progressoAula.Id,
            AulaId = progressoAula.AulaId,
            ProgressoCursoId =  progressoAula.ProgressoCursoId,
            Status = progressoAula.Status,
            Assistindo = progressoAula.Assistindo
        };
    }

    public async Task<AulaDto?> ObterAulaPorId(Guid aulaId)
    {
        var aula = await cursoRepository.ObterAulaPorId(aulaId);

        if (aula is null)
            return null;

        return new AulaDto
        {
            Id = aula.Id,
            Nome = aula.Nome,
            Conteudo = aula.Conteudo,
            Materiais = aula.Materiais?.Select(a => new MaterialDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Tipo = a.Tipo
            }).ToList() ?? []
        };
    }
}
