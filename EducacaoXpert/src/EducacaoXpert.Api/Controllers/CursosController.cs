using EducacaoXpert.Api.Controllers.Base;
using EducacaoXpert.Api.ViewModels;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using EducacaoXpert.GestaoConteudos.Application.Queries.DTO;
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
                            ICursoRepository cursoRepository,
                            ICursoQueries cursoQueries) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CursoViewModel>>> ObterTodos()
    {
        var cursos = await cursoQueries.ObterTodos();
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
    public async Task<IActionResult> Adicionar([FromBody] CursoViewModel curso)
    {
        var command = new AdicionarCursoCommand(curso.Nome, curso.Conteudo, UsuarioId, curso.Preco);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CursoViewModel curso)
    {
        if (id != curso.Id)
        {
            NotificarErro("Curso", "O ID do curso não pode ser diferente do ID informado na URL.");
            return RespostaPadrao();
        }
        var command = new AtualizarCursoCommand(curso.Id, curso.Nome, curso.Conteudo, curso.Preco);

        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/concluir-curso")]
    public async Task<IActionResult> ConcluirCurso(Guid id)
    {
        var curso = await cursoQueries.ObterPorId(id);

        await ValidarConclusaoCurso(curso);

        if (!OperacaoValida())
            return RespostaPadrao();

        var command = new ConcluirMatriculaCommand(UsuarioId, id, curso.Nome);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{cursoId:guid}/realizar-pagamento")]
    public async Task<IActionResult> RealizarPagamento(Guid cursoId, [FromBody] DadosPagamentoViewModel dadosPagamento)
    {
        var curso = await cursoQueries.ObterPorId(cursoId);

        await ValidarCursoMatricula(curso);

        if (!OperacaoValida())
            return RespostaPadrao();

        var command = new RealizarPagamentoCursoCommand(UsuarioId, cursoId, dadosPagamento.CvvCartao, dadosPagamento.ExpiracaoCartao, dadosPagamento.NomeCartao, dadosPagamento.NumeroCartao, curso.Preco);

        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var command = new DeletarCursoCommand(id);
        await _mediator.Send(command);
        return RespostaPadrao(HttpStatusCode.NoContent);
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
        var progressoCurso = await cursoRepository.ObterProgressoCurso(curso.Id, UsuarioId);

        if (progressoCurso is null || !progressoCurso.CursoConcluido)
        {
            NotificarErro("Curso", "Todas as aulas deste curso precisam estar concluídas.");
        }
    }
}
