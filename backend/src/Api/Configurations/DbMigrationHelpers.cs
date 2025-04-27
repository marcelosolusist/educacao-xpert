using Business.Entities;
using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations
{
    public static class DbMigrationHelpers
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            EnsureSeedData(app).Wait();
        }

        public static async Task EnsureSeedData(WebApplication application)
        {
            var service = application.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(service);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();
            await InserirDadosIniciais(context);
        }

        private static async Task InserirDadosIniciais(AppDbContext context)
        {
            if (context.Users.Any() || context.Set<Aluno>().Any()) return;
           
            var userIdentity = new IdentityUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "alunoadm@teste.com",
                EmailConfirmed = true,
                NormalizedEmail = "ALUNOADM@TESTE.COM",
                UserName = "alunoadm@teste.com",
                AccessFailedCount = 0,
                PasswordHash = "AQAAAAIAAYagAAAAEBsWFitsFK8YbDMAiXBQuFFCMZ0VNbH4W7qUWPCeBYcWMCp1FpCM1tXUkMCjDEYtOQ==",
                NormalizedUserName = "ALUNOADM@TESTE.COM",
            };

            var aluno = new Aluno()
            {
                Id = userIdentity.Id,
                Nome = "Aluno Administrador"
            };

            await context.Users.AddAsync(userIdentity);

            await context.Set<Aluno>().AddAsync(aluno);

            await context.SaveChangesAsync();
        }
    }
}
