using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class CertificadoCommandHandler(ICertificadoPdfService certificadoPdfService,
                                        IMediator mediator,
                                        IAlunoRepository alunoRepository) : CommandHandler,
                                        IRequestHandler<IncluirCertificadoCommand, bool>
{
    public async Task<bool> Handle(IncluirCertificadoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var aluno = await alunoRepository.ObterPorId(command.AlunoId);
        if (aluno is null)
        {
            await IncluirNotificacao(command.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }

        var certificado = new Certificado(aluno.Nome, command.NomeCurso, aluno.Id);

        var pdf = certificadoPdfService.GerarPdf(certificado);

        if (!pdf.Any())
        {
            await IncluirNotificacao(command.MessageType, "Erro ao gerar o PDF do certificado.", cancellationToken);
            return false;
        }

        certificado.IncluirArquivo(pdf);

        aluno.IncluirCertificado(certificado);
        alunoRepository.IncluirCertificado(certificado);

        return await alunoRepository.UnitOfWork.Commit();
    }
    protected override async Task IncluirNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
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
