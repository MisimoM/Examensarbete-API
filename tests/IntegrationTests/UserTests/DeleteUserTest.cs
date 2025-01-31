using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Enums;
using Modules.Users.Entities;
using Modules.Users.Features.Users.DeleteUser;
using System.Net.Http.Json;

namespace IntegrationTests.UserTests;

public class DeleteUserTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public DeleteUserTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DeleteUser_ShouldReturn200_WhenUserIsDeletedSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        var userToDelete = new User
        (
            "John Delete",
            "john.delete@example.com",
            UserRole.Customer.ToString(),
            "Password123"
        );

        UserDbContext.Users.Add(userToDelete);
        await UserDbContext.SaveChangesAsync();

        // Act
        var response = await client.DeleteAsync($"/users/{userToDelete.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<DeleteUserResponse>();
        Assert.NotNull(result);

        var deletedUser = await UserDbContext.Users.FirstOrDefaultAsync(u => u.Id == userToDelete.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturn404_WhenUserDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        var nonExistingUserId = Guid.NewGuid();

        // Act:
        var response = await client.DeleteAsync($"/users/{nonExistingUserId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
