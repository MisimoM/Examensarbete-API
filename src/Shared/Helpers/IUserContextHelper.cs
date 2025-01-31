namespace Shared.Helpers;

public interface IUserContextHelper
{
    Guid GetUserIdFromClaims();
}
