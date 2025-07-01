using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.IntegrationEvents;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class MatriculaCommandHandler(IMediator mediator,
                                    IAlunoRepository alunoRepository) : CommandHandler,
                                    IRequestHandler<IncluirMatriculaCommand, bool>,
                                    IRequestHandler<AtivarMatriculaCommand, bool>
{
    public async Task<bool> Handle(IncluirMatriculaCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var aluno = await alunoRepository.ObterPorId(command.AlunoId);
        if (aluno is null)
        {
            await IncluirNotificacao(command.MessageType, "Aluno não encontrado.", cancellationToken);
            return false;
        }

        var matriculaExiste = await alunoRepository.ObterMatriculaPorCursoEAlunoId(command.CursoId, aluno.Id);
        if (matriculaExiste is not null)
        {
            await IncluirNotificacao(command.MessageType, "Matrícula já existente.", cancellationToken);
            return false;
        }

        var matricula = new Matricula(command.AlunoId, command.CursoId);

        aluno.IncluirMatricula(matricula);
        alunoRepository.IncluirMatricula(matricula);

        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AtivarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var matricula = await alunoRepository.ObterMatriculaPorCursoEAlunoId(request.CursoId, request.AlunoId);
        if (matricula is null)
        {
            await IncluirNotificacao(request.MessageType, "Matrícula não encontrada.", cancellationToken);
            return false;
        }
        matricula.Ativar();
        alunoRepository.EditarMatricula(matricula);

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
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}

