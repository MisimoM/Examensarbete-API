﻿namespace Modules.Listings.Features.SearchListing;

internal record SearchListingResponse (
    Guid Id,
    string Title,
    string ImageUrl,
    string AccommodationType,
    string MainLocation,
    string SubLocation,
    decimal Price,
    DateTime AvailableFrom,
    DateTime AvailableUntil
    );
