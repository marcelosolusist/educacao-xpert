using EducacaoXpert.Core.Data;
using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.Messages;
using EducacaoXpert.PagamentoFaturamento.Data.Extension;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EducacaoXpert.PagamentoFaturamento.Data.Context;

public class PagamentoFaturamentoContext(DbContextOptions<PagamentoFaturamentoContext> options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(200)");

        builder.ApplyConfigurationsFromAssembly(typeof(PagamentoFaturamentoContext).Assembly);

        foreach (var relationShip in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        builder.Ignore<Event>();

        base.OnModelCreating(builder);
    }
    public async Task<bool> Commit()
    {
        var sucesso = await SaveChangesAsync() > 0;

        if (sucesso)
            await mediator.PublishDomainEvents(this);

        return sucesso;
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entityEntry in ChangeTracker.Entries<Entity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property("DataCriacao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataAlteracao").IsModified = false;
                entityEntry.Property("DataExclusao").IsModified = false;
            }
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataCriacao").IsModified = false;
                entityEntry.Property("DataExclusao").IsModified = false;
            }
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.State = EntityState.Modified;
                entityEntry.Property("DataExclusao").CurrentValue = DateTime.Now;
                entityEntry.Property("DataCriacao").IsModified = false;
                entityEntry.Property("DataAlteracao").IsModified = false;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
