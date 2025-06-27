using EducacaoXpert.Api.Controllers.Base;
using EducacaoXpert.Core.DomainObjects.DTO;
using EducacaoXpert.Core.DomainObjects.Enums;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Queries.ViewModels;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EducacaoXpert.Api.Controllers;

[Route("api/cursos/{cursoId:guid}/aulas")]
public class AulasController(INotificationHandler<DomainNotification> notificacoes,
                            IAppIdentityUser identityUser,
                            IAlunoQueries alunoQueries,
                            IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "ADMIN")]
    [HttpPost("adicionar-aula")]
    public async Task<IActionResult> Adicionar([FromBody] AulaDto aulaDto, Guid cursoId)
    {
        var command = new AdicionarAulaCommand(aulaDto.Nome, aulaDto.Conteudo, cursoId,
                                               aulaDto.NomeMaterial, aulaDto.TipoMaterial);

        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/realizar-aula")]
    public async Task<IActionResult> Realizar(Guid id, Guid cursoId)
    {
        var matricula = await alunoQueries.ObterMatricula(cursoId, UsuarioId);

        ValidarMatricula(matricula);

        if (!OperacaoValida())
            return RespostaPadrao();

        var command = new RealizarAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpPost("{id:guid}/concluir-aula")]
    public async Task<IActionResult> Concluir(Guid id, Guid cursoId)
    {
        var matricula = await alunoQueries.ObterMatricula(cursoId, UsuarioId);

        ValidarMatricula(matricula);
        if (!OperacaoValida())
            return RespostaPadrao();
        var command = new ConcluirAulaCommand(id, UsuarioId, cursoId);
        await _mediator.Send(command);

        return RespostaPadrao(HttpStatusCode.Created);
    }

    private void ValidarMatricula(MatriculaViewModel? matricula)
    {
        if (matricula is null)
        {
            NotificarErro("Matricula", "Matrícula não encontrada.");
            return;
        }

        if (matricula?.Status != EStatusMatricula.Ativa && matricula?.Status != EStatusMatricula.Concluida)
        {
            NotificarErro("Matricula", "Matrícula não está ativa.");
        }
    }
}
