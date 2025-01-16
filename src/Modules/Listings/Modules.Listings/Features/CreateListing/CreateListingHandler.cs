using FluentValidation;
using Modules.Listings.Data;
using Modules.Listings.Entities;
using Shared.Exceptions;

namespace Modules.Listings.Features.CreateListing;

internal class CreateListingHandler(ListingDbContext dbContext, IValidator<CreateListingRequest> validator)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IValidator<CreateListingRequest> _validator = validator;
    public async Task<CreateListingResponse> Handle(CreateListingRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            AccommodationType = request.AccommodationType,
            MainLocation = request.MainLocation,
            SubLocation = request.SubLocation,
            Price = request.Price,
            AvailableFrom = request.AvailableFrom,
            AvailableUntil = request.AvailableUntil,
            Images = request.Images.Select(img => new ListingImage
            {
                Url = img.Url,
                AltText = img.AltText
            }).ToList()
        };
        
        foreach (var image in listing.Images)
        {
            image.ListingId = listing.Id;
        }

        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateListingResponse(listing.Id);

    }
}
