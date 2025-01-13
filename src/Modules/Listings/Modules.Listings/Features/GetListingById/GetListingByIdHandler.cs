using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Listings.Data;

namespace Modules.Listings.Features.GetListingById;

internal class GetListingByIdHandler(ListingDbContext dbContext, IValidator<GetListingByIdRequest> validator, ILogger<GetListingByIdHandler> logger)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IValidator<GetListingByIdRequest> _validator = validator;
    private readonly ILogger _logger = logger;

    public async Task<IResult> Handle(GetListingByIdRequest request)
    {
        _logger.LogInformation("Received request to get listing by ID: {ListingId}", request.Id);
 
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for listing ID: {ListingId}. Errors: {Errors}", request.Id, validationResult.Errors.Select(e => e.ErrorMessage));
            return Results.BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }
        
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.Id);

        if (listing is null)
        {
            _logger.LogWarning("No listing found with ID: {ListingId}", request.Id);
            return Results.NotFound(new { error = $"No listing with the ID {request.Id} was found" });
        }

        _logger.LogInformation("Listing with ID: {ListingId} fetched successfully. Title: {Title}", listing.Id, listing.Title);

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
