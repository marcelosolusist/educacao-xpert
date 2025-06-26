﻿using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class CertificadoCommandHandler(ICertificadoPdfService certificadoPdfService,
                                        IMediator mediator,
                                       IAlunoRepository alunoRepository) : CommandHandler,
                                        IRequestHandler<AdicionarCertificadoCommand, bool>
{
    public async Task<bool> Handle(AdicionarCertificadoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = await alunoRepository.ObterPorId(request.AlunoId);
        if (aluno is null)
        {
            await AdicionarNotificacao(request.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }
        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula is null)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }

        if (!matricula.DataConclusao.HasValue)
        {
            await AdicionarNotificacao(request.MessageType, "Matrícula não está concluída.", cancellationToken);
            return false;
        }

        var certificado = new Certificado(aluno.Nome, request.NomeCurso, matricula.Id, aluno.Id, matricula.DataConclusao);

        var pdf = certificadoPdfService.GerarPdf(certificado);

        if (!pdf.Any())
        {
            await AdicionarNotificacao(request.MessageType, "Erro ao gerar o PDF do certificado.", cancellationToken);
            return false;
        }

        certificado.AdicionarArquivo(pdf);

        aluno.AdicionarCertificado(certificado);
        alunoRepository.AdicionarCertificado(certificado);

        return await alunoRepository.UnitOfWork.Commit();
    }
    protected override async Task AdicionarNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
    {
        await mediator.Publish(new DomainNotification(messageType, descricao), cancellationToken);
    }
    private bool ValidarComando(Command command)
    {
        if (command.EhValido())
            return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage), CancellationToken.None);
        }
        return false;
    }
}
