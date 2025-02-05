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
        (
            Guid.NewGuid(),
            hostId,
            request.Title,
            request.Description,
            request.AccommodationType,
            request.MainLocation,
            request.SubLocation,
            request.Price,
            request.AvailableFrom,
            request.AvailableUntil
        );

        listing.Images = request.Images.Select(img => new ListingImage
        (
            listing.Id,
            img.Url,
            img.AltText
        )).ToList();

        listing.ListingFacilities = request.Facilities.Select(facility => new ListingFacility
        {
            ListingId = listing.Id,
            FacilityId = facility.Id
        }).ToList();

        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateListingResponse(listing.Id);
    }
}
