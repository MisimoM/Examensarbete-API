namespace IntegrationTests.UserTests;

using Microsoft.EntityFrameworkCore;
using Modules.Users.Common.Enums;
using Modules.Users.Data;
using Modules.Users.Entities;
using Modules.Users.Features.Users.UpdateUser;
using System.Net.Http.Json;

public class UpdateUserTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public UpdateUserTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn200_WhenUserIsUpdatedSuccessfully()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "updated.alice@example.com", "NewPassword123");

        // Act
        var response = await client.PutAsJsonAsync($"/users/{existingUser.Id}", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var updatedUser = await UserDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == existingUser.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated Name", updatedUser.Name);
        Assert.Equal("updated.alice@example.com", updatedUser.Email);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn404_WhenUserNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "updated.bob@example.com", "NewPassword123");

        // Act
        var response = await client.PutAsJsonAsync($"/users/{Guid.NewGuid()}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn409_WhenEmailAlreadyExists()
    {
        // Arrange
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Charlie Brown",
            Email = "charlie.brown@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "Daisy Smith",
            Email = "daisy.smith@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.AddRange(user1, user2);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "daisy.smith@example.com", "NewPassword123");

        // Act
        var response = await client.PutAsJsonAsync($"/users/{user1.Id}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn400_WhenInvalidEmailFormat()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Eve Adams",
            Email = "eve.adams@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "invalidemail", "NewPassword123");

        // Act
        var response = await client.PutAsJsonAsync($"/users/{existingUser.Id}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid email format", result);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn400_WhenPasswordTooShort()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Frank Miller",
            Email = "frank.miller@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "updated.frank@example.com", "short");

        // Act
        var response = await client.PutAsJsonAsync($"/users/{existingUser.Id}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must be at least 8 characters long", result);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn400_WhenPasswordMissingUppercase()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Emily Davis",
            Email = "emily.davis@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "updated.emily@example.com", "password123"); // Missing uppercase

        // Act
        var response = await client.PutAsJsonAsync($"/users/{existingUser.Id}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must contain at least one uppercase letter", result);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturn400_WhenPasswordMissingDigit()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "Password123",
            Role = UserRole.Customer.ToString()
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();
        var request = new UpdateUserRequest("Updated Name", "updated.email@example.com", "Password"); // Missing digit

        // Act
        var response = await client.PutAsJsonAsync($"/users/{existingUser.Id}", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must contain at least one digit", result);
    }
}
