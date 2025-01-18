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
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin User",
                Email = "admin@admin.com",
                Password = passwordHasher.Hash("Admin123"),
                Role = UserRole.Admin.ToString()
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
    }
}
