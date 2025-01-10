namespace Modules.Users.Common.Helpers;

internal interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
