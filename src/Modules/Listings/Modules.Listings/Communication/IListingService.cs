using Modules.Listings.Entities;

namespace Modules.Listings.Communication;

public interface IListingService
{
    Task<Listing> GetByIdAsync(Guid Id);
}
