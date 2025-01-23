using Modules.Users.Common.Enums;
using Modules.Users.Data;
using Modules.Users.Entities;
using Modules.Users.Features.Users.CreateUser;
using System.Net.Http.Json;

namespace IntegrationTests.UserTests;

public class CreateUserTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public CreateUserTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateUser_ShouldReturn201_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        var request = new CreateUserRequest("John Doe", "john.doe@example.com", "Password123");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn409_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "existinguser@example.com",
            Role = UserRole.Customer.ToString(),
            Password = "Password123"
        };

        UserDbContext.Users.Add(existingUser);
        await UserDbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        var request = new CreateUserRequest("John Doe", "existinguser@example.com", "Password123");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn400_WhenInvalidEmailFormat()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("John Doe", "invalidemail", "Password123!");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid email format", result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn400_WhenPasswordTooShort()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("John Doe", "john.doe@example.com", "Short1");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must be at least 8 characters long", result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn400_WhenPasswordMissingUppercase()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("John Doe", "john.doe@example.com", "password123");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must contain at least one uppercase letter", result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn400_WhenPasswordMissingDigit()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("John Doe", "john.doe@example.com", "Password");

        // Act
        var response = await client.PostAsJsonAsync("/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password must contain at least one digit", result);
    }
}
