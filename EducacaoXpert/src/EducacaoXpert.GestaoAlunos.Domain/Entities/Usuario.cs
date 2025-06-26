using EducacaoXpert.Core.DomainObjects;
using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.GestaoAlunos.Domain.Entities;

public class Usuario : Entity, IAggregateRoot
{
    public Usuario(Guid Id) : base(Id) { }
}
