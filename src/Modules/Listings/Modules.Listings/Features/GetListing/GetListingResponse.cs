using Modules.Listings.Features.Shared;

namespace Modules.Listings.Features.GetListing;

public record GetListingResponse (
    Guid Id,
    Guid HostId,
    string Title,
    string AccommodationType,
    string MainLocation,
    string SubLocation,
    decimal Price,
    DateTime AvailableFrom,
    DateTime AvailableUntil,
    List<ListingImageDto> Images);
