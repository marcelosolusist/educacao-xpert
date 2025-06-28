﻿using EducacaoXpert.Core.Messages;
using EducacaoXpert.Core.Messages.Notifications;
using EducacaoXpert.GestaoAlunos.Application.Commands;
using EducacaoXpert.GestaoAlunos.Domain.Interfaces;
using EducacaoXpert.GestaoAlunos.Domain.Entities;
using MediatR;

namespace EducacaoXpert.GestaoAlunos.Application.Handlers;

public class UsuarioCommandHandler(IMediator mediator,
                                   IAlunoRepository alunoRepository,
                                   IUsuarioRepository usuarioRepository) : CommandHandler,
                                    IRequestHandler<AdicionarAlunoCommand, bool>,
                                    IRequestHandler<AdicionarAdminCommand, bool>
{
    public async Task<bool> Handle(AdicionarAlunoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = new Aluno(Guid.Parse(request.UsuarioId), request.Nome);

        alunoRepository.Adicionar(aluno);
        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AdicionarAdminCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var usuario = new Usuario(Guid.Parse(request.UsuarioId));

        usuarioRepository.Adicionar(usuario);
        return await usuarioRepository.UnitOfWork.Commit();
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
