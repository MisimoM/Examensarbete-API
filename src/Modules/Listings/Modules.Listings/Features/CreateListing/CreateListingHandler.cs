using FluentValidation;
using Modules.Listings.Data;
using Modules.Listings.Entities;
using Modules.Users.Communication;
using Shared.Exceptions;
using Shared.Helpers;

namespace Modules.Listings.Features.CreateListing;

public class CreateListingHandler(ListingDbContext dbContext, IValidator<CreateListingRequest> validator, IUserContextHelper userContextHelper, IUserService userService)
{
    private readonly ListingDbContext _dbContext = dbContext;
    private readonly IValidator<CreateListingRequest> _validator = validator;
    private readonly IUserContextHelper _userContextHelper = userContextHelper;
    private readonly IUserService _userService = userService;
    public async Task<CreateListingResponse> Handle(CreateListingRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
        }

        var userId = _userContextHelper.GetUserIdFromClaims();
        var hostId = await _userService.GetUserIdAsync(userId, cancellationToken);

        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            HostId = hostId,
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
