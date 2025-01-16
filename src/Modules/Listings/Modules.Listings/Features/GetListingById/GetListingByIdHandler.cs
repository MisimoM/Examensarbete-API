using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Shared.Exceptions;

namespace Modules.Listings.Features.GetListingById;

internal class GetListingByIdHandler(ListingDbContext dbContext)
{
    private readonly ListingDbContext _dbContext = dbContext;

    public async Task<GetListingByIdResponse> Handle(GetListingByIdRequest request)
    {
        
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .Where(l => l.Id == request.Id)
            .Include(l => l.Images)
            .FirstOrDefaultAsync();

        if (listing is null)
            throw new NotFoundException($"No listing with the ID {request.Id} was found.");

        return new GetListingByIdResponse(
            listing.Id,
            listing.Title,
            listing.Description,
            listing.AccommodationType,
            listing.MainLocation,
            listing.SubLocation,
            listing.Price,
            listing.AvailableFrom,
            listing.AvailableUntil,
            listing.Images.Select(img => new ListingImageResponse(img.Url, img.AltText)).ToList()
        );
    }
}
