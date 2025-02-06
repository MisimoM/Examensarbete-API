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

            var user1 = new User
            (
                Guid.Parse("d3b07384-d9a9-4e6b-a0b9-2f084bc16961"),
                "Conny Svensson",
                "conny@email.com",
                UserRole.Host.ToString(),
                passwordHasher.Hash("Conny123")
            );

           var user2 = new User
           (
               Guid.Parse("f2a7e6b3-8c1d-4c5d-a1f6-9e3d5e7b8a2c"),
               "Lars Andersson",
               "lars@email.com",
               UserRole.Host.ToString(),
               passwordHasher.Hash("Lars123")
           );

           var user3 = new User
           (
               Guid.Parse("d9f4b8e2-3a7c-4d5f-8b1e-6c2a3e9d7f5b"),
               "Lena Larsson",
               "lena@email.com",
               UserRole.Host.ToString(),
               passwordHasher.Hash("Lena123")
           );

           var user4 = new User
           (
               Guid.Parse("a1c5e7d3-9b2f-4d8c-6f3a-7e5b1d2f9c8a"),
               "Karin Persson",
               "karin@email.com",
               UserRole.Host.ToString(),
               passwordHasher.Hash("Karin123")
           );

            dbContext.Users.AddRange(adminUser, user1, user2, user3, user4);
            dbContext.SaveChanges();
        }
    }
}
