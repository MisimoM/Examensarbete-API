using Modules.Users.Common.Enums;
using Modules.Users.Common.Helpers;
using Modules.Users.Entities;

namespace Modules.Users.Data.Seed;

internal class UserSeeder
{
    public static void Seed(UserDbContext dbContext, IPasswordHasher passwordHasher)
    {
        if (!dbContext.Users.Any())
        {
            var adminUser = new User
            (
                "Admin User",
                "admin@admin.com",
                UserRole.Admin.ToString(),
                passwordHasher.Hash("Admin123")
            );

            var hostUser = new User
            (
                Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                "Host User",
                "host@host.com",
                UserRole.Host.ToString(),
                passwordHasher.Hash("Host123")
            );

            dbContext.Users.AddRange(adminUser, hostUser);
            dbContext.SaveChanges();
        }
    }
}
