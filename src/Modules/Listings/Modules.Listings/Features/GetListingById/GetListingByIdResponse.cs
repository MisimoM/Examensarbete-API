namespace Modules.Listings.Features.GetListingById;

//Kommentar
internal record GetListingByIdResponse(
    Guid Id,
    string Title,
    string Description,
    string ImageUrl,
    string AccommodationType,
    string MainLocation,
    string SubLocation,
    decimal Price,
    DateTime AvailableFrom,
    DateTime AvailableUntil);
