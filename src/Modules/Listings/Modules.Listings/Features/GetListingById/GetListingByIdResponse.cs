namespace Modules.Listings.Features.GetListingById;

public record GetListingByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string AccommodationType,
    string MainLocation,
    string SubLocation,
    decimal Price,
    DateTime AvailableFrom,
    DateTime AvailableUntil,
    List<ListingImageResponse> Images);

public record ListingImageResponse(string Url, string AltText);
