namespace EducacaoXpert.Core.DomainObjects.Interfaces;

public interface IAppIdentityUser
{
    public string GetUserId();
    bool IsAuthenticated();
}
