using Modules.Users.Common.Enums;
using Modules.Users.Entities;
using Modules.Users.Features.Authentication.Login;
using System.Net.Http.Json;

namespace IntegrationTests.UserTests;

public class LoginTest : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;
    public LoginTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_ShouldSetCookies_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest("test@test.com", "testpassword");

        var user = new User
        (
            "Test User",
            request.Email,
            UserRole.Admin.ToString(),
            PasswordHasher.Hash(request.Password)
        );
        UserDbContext.Users.Add(user);
        await UserDbContext.SaveChangesAsync();

        // Act
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/auth/login", request);

        var cookies = response.Headers.GetValues("Set-Cookie").ToList();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Contains(cookies, cookie => cookie.Contains("accessToken"));
        Assert.Contains(cookies, cookie => cookie.Contains("refreshToken"));
    }
}
