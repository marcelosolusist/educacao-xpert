using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducacaoXpert.Api.Context;

public class ApiContext(DbContextOptions<ApiContext> options) : IdentityDbContext(options)
{
}