﻿using EducacaoXpert.Core.DomainObjects.Interfaces;

namespace EducacaoXpert.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
