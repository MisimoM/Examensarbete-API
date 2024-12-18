using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;

namespace Modules.Listings.Features.GetListingById;

internal class GetListingByIdHandler(ListingDbContext dbContext, IValidator<GetListingByIdRequest> validator)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IValidator<GetListingByIdRequest> _validator = validator;

    public async Task<IResult> Handle(GetListingByIdRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.Id);

        if (listing is null)
            return Results.NotFound(new {error = $"No listing with the ID {request.Id} was found" });

        return Results.Ok(
            new GetListingByIdResponse(
            listing.Id,
            listing.Title,
            listing.Description,
            listing.ImageUrl,
            listing.AccommodationType,
            listing.MainLocation,
            listing.SubLocation,
            listing.Price,
            listing.AvailableFrom,
            listing.AvailableUntil)
        );
    }
}
