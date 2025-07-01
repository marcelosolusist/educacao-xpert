using EducacaoXpert.Api.Controllers.Base;
using EducacaoXpert.Core.DomainObjects.Interfaces;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EducacaoXpert.Api.Controllers;

[Route("api/alunos")]
public class AlunosController(INotificationHandler<DomainNotification> notificacoes,
                                IAlunoQueries alunoQueries,
                                IAppIdentityUser identityUser,
                                IMediator mediator) : MainController(notificacoes, mediator, identityUser)
{

    [Authorize(Roles = "ALUNO")]
    [HttpGet("certificados")]
    public async Task<IActionResult> ListarCertificados()
    {
        var certificados = await alunoQueries.ListarCertificados(UsuarioId);
        if (certificados.Count() == 0)
        {
            return BadRequest();
        }

        return RespostaPadrao(HttpStatusCode.OK, certificados);
    }

    [Authorize(Roles = "ALUNO")]
    [HttpGet("certificados/{id:guid}/download")]
    public async Task<IActionResult> BaixarCertificado(Guid id)
    {
        var certificado = await alunoQueries.ObterCertificado(id, UsuarioId);
        if (certificado?.Arquivo == null || certificado.Arquivo.Length == 0)
        {
            return BadRequest();
        }

        return File(certificado.Arquivo, "application/pdf", $"Certificado_{id}.pdf");
    }
    
}
