using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.Core.DomainObjects;

public class Usuario : Entity, IAggregateRoot
{
    public Usuario(Guid Id) : base(Id) { }

}
