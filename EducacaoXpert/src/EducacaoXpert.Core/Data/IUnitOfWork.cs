namespace EducacaoXpert.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
