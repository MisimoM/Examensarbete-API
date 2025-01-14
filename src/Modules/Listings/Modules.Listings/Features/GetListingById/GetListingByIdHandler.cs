using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Shared.Exceptions;

namespace Modules.Listings.Features.GetListingById;

internal class GetListingByIdHandler(ListingDbContext dbContext, IValidator<GetListingByIdRequest> validator)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IValidator<GetListingByIdRequest> _validator = validator;

    public async Task<GetListingByIdResponse> Handle(GetListingByIdRequest request)
    {
        
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.Id);

        if (listing is null)
            throw new NotFoundException($"No listing with the ID {request.Id} was found.");

        return new GetListingByIdResponse(
            listing.Id,
            listing.Title,
            listing.Description,
            listing.ImageUrl,
            listing.AccommodationType,
            listing.MainLocation,
            listing.SubLocation,
            listing.Price,
            listing.AvailableFrom,
            listing.AvailableUntil
        );
    }
}
