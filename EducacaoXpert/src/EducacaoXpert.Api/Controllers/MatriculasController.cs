using EducacaoXpert.Api.Controllers.Base;
using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoConteudos.Application.Queries.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EducacaoXpert.Api.Controllers;

[Authorize(Roles = "ADMIN,ALUNO")]
[Route("api/matriculas")]
public class MatriculasController(INotificationHandler<DomainNotification> notificacoes,
                                 IAlunoQueries alunoQueries,
                                 IAppIdentityUser identityUser,
                                 ICursoQueries cursoQueries,
                                 IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("em-pagamento")]
    public async Task<ActionResult<IEnumerable<MatriculaDto>>> ObterMatriculasEmPagamento()
    {
        var matriculas = await alunoQueries.ObterMatriculasEmPagamento(UsuarioId);
        return RespostaPadrao(HttpStatusCode.OK, matriculas);
    }

    [HttpPost("{cursoId:guid}")]
    public async Task<IActionResult> Adicionar(Guid cursoId)
    {
        var curso = await cursoQueries.ObterPorId(cursoId);

        if (curso is null)
        {
            NotificarErro("Curso", "Curso não encontrado.");
            return RespostaPadrao();
        }
        var command = new AdicionarMatriculaCommand(UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }
}
