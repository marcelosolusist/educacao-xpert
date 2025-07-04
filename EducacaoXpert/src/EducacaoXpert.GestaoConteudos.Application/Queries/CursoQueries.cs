using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;

namespace EducacaoXpert.GestaoConteudos.Application.Queries;

public class CursoQueries(ICursoRepository cursoRepository) : ICursoQueries
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
}
