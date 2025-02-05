using Modules.Listings.Dtos;

namespace Modules.Listings.Features.SearchListing;

public record SearchListingResponse (
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
