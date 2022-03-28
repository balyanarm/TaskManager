using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DbInitializer
{
    public static class DbInitializer
    {
        public static async Task Initialize(IConfiguration configuration, TaskManagerDbContext context,
            IEncryptService encryptService, ILogger logger)
        {
            logger.Log(LogLevel.Information, "Start DbInitializer process...");
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == configuration["Configs:DefaultRoleName"]);
            if (role == null)
            {
                role = context.Roles.Add(new Role()
                {
                    Name = configuration["Configs:DefaultRoleName"]
                }).Entity;

                var user = context.Users.Add(new User()
                {
                    UserName = configuration["Configs:DefaultUserName"],
                    PasswordHash = encryptService.Encrypt(configuration["Configs:DefaultPassword"],
                        configuration["Configs:Encryptkey"])
                }).Entity;

                context.UserRoles.Add(new UserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });

                await context.SaveChangesAsync();
            }

            logger.Log(LogLevel.Information, "Finish DbInitializer.");
        }
    }
}