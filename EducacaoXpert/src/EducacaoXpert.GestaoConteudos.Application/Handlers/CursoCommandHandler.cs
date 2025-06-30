using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoConteudos.Application.Commands;
using EducacaoXpert.GestaoConteudos.Domain.Entities;
using EducacaoXpert.GestaoConteudos.Domain.Interfaces;
using MediatR;

namespace EducacaoXpert.GestaoConteudos.Application.Handlers;

public class CursoCommandHandler(ICursoRepository cursoRepository,
                                 IProgressoCursoRepository progressoCursoRepository,
                                 IMediator mediator) : CommandHandler,
                                                    IRequestHandler<IncluirCursoCommand, bool>,
                                                    IRequestHandler<EditarCursoCommand, bool>,
                                                    IRequestHandler<ExcluirCursoCommand, bool>
{
    public async Task<bool> Handle(IncluirCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command)) return false;

        var curso = new Curso(command.Nome, command.Conteudo, command.UsuarioCriacaoId, command.Preco);
        cursoRepository.Incluir(curso);

        return await cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(EditarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var curso = await cursoRepository.ObterPorId(command.CursoId);
        if (curso is null)
        {
            await IncluirNotificacao(command.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        curso.EditarNome(command.Nome);
        curso.EditarConteudo(command.Conteudo);
        curso.EditarPreco(command.Preco);

        cursoRepository.Editar(curso);
        return await cursoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(ExcluirCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command))
            return false;

        var curso = await cursoRepository.ObterCursoComAulas(command.CursoId);
        if (curso is null)
        {
            await IncluirNotificacao(command.MessageType, "Curso não encontrado.", cancellationToken);
            return false;
        }

        if (curso.Aulas.Any())
        {
            await IncluirNotificacao(command.MessageType, "Curso não pode ser excluído pois possui aulas associadas.", cancellationToken);
            return false;
        }

        cursoRepository.Remover(curso);
        return await cursoRepository.UnitOfWork.Commit();
    }

    protected override async Task IncluirNotificacao(string messageType, string descricao, CancellationToken cancellationToken)
    {
        await mediator.Publish(new DomainNotification(messageType, descricao), cancellationToken);
    }
    private bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}
