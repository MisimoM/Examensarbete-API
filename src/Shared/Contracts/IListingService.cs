using Shared.Dtos;

namespace Shared.Contracts;

public interface IListingService
{
    Task<ListingDto> GetByIdAsync(Guid Id);
}
