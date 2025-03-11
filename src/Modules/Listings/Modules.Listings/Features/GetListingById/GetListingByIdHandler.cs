using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Modules.Listings.Features.Shared;
using Modules.Users.Communication;
using Shared.Exceptions;

namespace Modules.Listings.Features.GetListingById;

public class GetListingByIdHandler(ListingDbContext dbContext, IUserService userService)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IUserService _userService = userService;

    public async Task<GetListingByIdResponse> Handle(GetListingByIdRequest request, CancellationToken cancellationToken)
    {

        var listing = await _dbContext.Listings
            .AsNoTracking()
            .Where(l => l.Id == request.Id)
            .Include(l => l.Images)
            .Include(l => l.ListingFacilities)
                .ThenInclude(lf => lf.Facility)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        if (listing is null)
            throw new NotFoundException($"No listing with the ID {request.Id} was found.");

        var user = await _userService.GetUserAsync(listing.HostId, cancellationToken);
        if (user is null)
            throw new NotFoundException($"No host with the ID {listing.HostId} was found.");

        var host = new HostDto(user.Name, user.Email, user.ProfileImage);


        return new GetListingByIdResponse(
            listing.Id,
            listing.HostId,
            listing.Title,
            listing.Description,
            listing.AccommodationType,
            listing.MainLocation,
            listing.SubLocation,
            listing.Price,
            listing.AvailableFrom,
            listing.AvailableUntil,
            listing.Images.Select(img => new ListingImageDto(img.Url, img.AltText)).ToList(),
            listing.ListingFacilities.Select(lf => new FacilityDto(lf.Facility.Id, lf.Facility.Name)).ToList(),
            host
        );
    }
}
