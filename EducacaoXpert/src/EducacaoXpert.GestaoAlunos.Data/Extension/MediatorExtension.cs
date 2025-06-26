﻿using EducacaoXpert.Core.DomainObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.GestaoAlunos.Data.Extension;

public static class MediatorExtension
{
    public static async Task PublishDomainEvents(this IMediator mediator, DbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(x => x.Notificacoes).ToList();

        domainEntities.ForEach(entity => entity.LimparEventos());

        var tasks = domainEvents.Select(async (domainEvent) =>
        {
            await mediator.Publish(domainEvent);
        });

        await Task.WhenAll(tasks);
    }
}
