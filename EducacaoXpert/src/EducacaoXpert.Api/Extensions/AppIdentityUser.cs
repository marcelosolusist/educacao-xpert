using EducacaoXpert.Core.DomainObjects.Interfaces;
using System.Security.Claims;

namespace EducacaoXpert.Api.Extensions;

public class AppIdentityUser(IHttpContextAccessor accessor) : IAppIdentityUser
{
    public string GetUserId()
    {
        return accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public bool IsAuthenticated()
    {
        return accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
