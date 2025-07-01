using EducacaoXpert.Api.Controllers.Base;
using EducacaoXpert.Api.ViewModels;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;
using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using EducacaoXpert.PagamentoFaturamento.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EducacaoXpert.Api.Controllers;

[Route("api/cursos")]
public class CursosController(INotificationHandler<DomainNotification> notificacoes,
                                IMediator mediator,
                                IAppIdentityUser identityUser,
                                IAlunoQueries alunoQueries,
                                IProgressoCursoRepository progressoCursoRepository,
                                ICursoQueries cursoQueries) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CursoViewModel>>> ListarTodos()
    {
        var cursos = await cursoQueries.ListarTodos();
        return RespostaPadrao(HttpStatusCode.OK, cursos);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CursoViewModel>> ObterPorId(Guid id)
    {
        var curso = await cursoQueries.ObterPorId(id);
        return RespostaPadrao(HttpStatusCode.OK, curso);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Incluir([FromBody] CursoViewModel curso)
    {
        var command = new IncluirCursoCommand(curso.Nome, curso.Conteudo, UsuarioId, curso.Preco);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] CursoViewModel curso)
    {
        if (id != curso.Id)
        {
            NotificarErro("Curso", "O ID do curso não pode ser diferente do ID informado na URL.");
            return RespostaPadrao();
        }
        var command = new EditarCursoCommand(curso.Id, curso.Nome, curso.Conteudo, curso.Preco);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var command = new ExcluirCursoCommand(id);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id:guid}/aulas")]
    public async Task<ActionResult<IEnumerable<CursoViewModel>>> ListarTodasAulasPorCursoId(Guid id)
    {
        var aulas = await cursoQueries.ListarTodasAulasPorCursoId(id);
        var aulasResumidas = new List<AulaResumidaViewModel>();
        foreach (var aula in aulas)
        {
            aulasResumidas.Add(new AulaResumidaViewModel()
            {
                Id = aula.Id,
                Nome = aula.Nome,
                Conteudo = aula.Conteudo,
            });
        }
        return RespostaPadrao(HttpStatusCode.OK, aulasResumidas);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("{id:guid}/aulas")]
    public async Task<IActionResult> IncluirAula(Guid id, [FromBody] AulaViewModel aulaDto)
    {
        var command = new IncluirAulaCommand(aulaDto.Nome, aulaDto.Conteudo, id,
                                               aulaDto.NomeMaterial, aulaDto.TipoMaterial);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:guid}/aulas/{idAula:guid}")]
    public async Task<IActionResult> EditarAula(Guid id, Guid idAula, [FromBody] AulaViewModel aula)
    {
        if (idAula != aula.Id)
        {
            NotificarErro("Aula", "O ID da aula não pode ser diferente do ID informado na URL.");
            return RespostaPadrao();
        }
        var command = new EditarAulaCommand((Guid)aula.Id, aula.Nome, aula.Conteudo, id, aula.NomeMaterial, aula.TipoMaterial);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}/aulas/{idAula:guid}")]
    public async Task<IActionResult> ExcluirAula(Guid id, Guid idAula)
    {
        var command = new ExcluirAulaCommand(idAula);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/matricular")]
    public async Task<IActionResult> Matricular(Guid id)
    {
        var curso = await cursoQueries.ObterPorId(id);
        if (curso is null)
        {
            NotificarErro("Curso", "Curso não encontrado.");
            return RespostaPadrao();
        }
        var command = new IncluirMatriculaCommand(UsuarioId, id);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/pagar-matricula")]
    public async Task<IActionResult> PagarMatricula(Guid id, [FromBody] DadosPagamentoViewModel dadosPagamento)
    {
        var curso = await cursoQueries.ObterPorId(id);
        await ValidarCursoMatricula(curso);
        if (!OperacaoValida()) return RespostaPadrao();
        var command = new RealizarPagamentoCursoCommand(UsuarioId, id, dadosPagamento.CvvCartao, dadosPagamento.ExpiracaoCartao, dadosPagamento.NomeCartao, dadosPagamento.NumeroCartao, curso.Preco);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpGet("{id:guid}/aulas/{idAula:guid}/iniciar")]
    public async Task<IActionResult> IniciarAula(Guid id, Guid idAula)
    {
        //TODO: Iniciar a aula

        return RespostaPadrao(HttpStatusCode.OK);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPut("{id:guid}/aulas/{idAula:guid}/finalizar")]
    public async Task<IActionResult> FinalizarAula(Guid id, Guid idAula)
    {
        //TODO: Finalizar a aula

        return RespostaPadrao(HttpStatusCode.OK);
    }

    private async Task ValidarCursoMatricula(CursoDto? curso)
    {
        if (curso is null)
        {
            NotificarErro("Curso", "Curso não encontrado.");
            return;
        }
        var matricula = await alunoQueries.ObterMatricula(curso.Id, UsuarioId);
        if (matricula is not { Status: StatusMatricula.EmPagamento })
        {
            NotificarErro("Matricula", "A matrícula deve estar com status 'Em Pagamento' para realizar o pagamento.");
        }
    }

    private async Task ValidarConclusaoCurso(CursoDto? curso)
    {
        if (curso is null)
        {
            NotificarErro("Curso", "Curso não encontrado.");
            return;
        }
        var progressoCurso = await progressoCursoRepository.Obter(curso.Id, UsuarioId);
        if (progressoCurso is null || !progressoCurso.CursoConcluido)
        {
            NotificarErro("Curso", "Todas as aulas deste curso precisam estar concluídas.");
        }
    }
}
