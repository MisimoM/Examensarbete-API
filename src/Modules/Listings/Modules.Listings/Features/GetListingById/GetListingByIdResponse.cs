﻿using Modules.Listings.Features.Shared;

namespace Modules.Listings.Features.GetListingById;

public record GetListingByIdResponse(
    Guid Id,
    Guid HostId,
    string Title,
    string Description,
    string AccommodationType,
    string MainLocation,
    string SubLocation,
    decimal Price,
    DateTime AvailableFrom,
    DateTime AvailableUntil,
    List<ListingImageDto> Images,
    List<FacilityDto> Facilities,
    HostDto Host
);
