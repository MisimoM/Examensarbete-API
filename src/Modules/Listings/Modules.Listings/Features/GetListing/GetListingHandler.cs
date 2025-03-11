using Microsoft.EntityFrameworkCore;
using Modules.Listings.Data;
using Modules.Listings.Features.Shared;

namespace Modules.Listings.Features.GetListing;

public class GetListingHandler(ListingDbContext dbContext)
{
    private readonly ListingDbContext _dbContext = dbContext;

    public async Task <List<GetListingResponse>> Handle(GetListingRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Listings.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.MainLocation))
            query = query.Where(l => l.MainLocation == request.MainLocation);

        if (!string.IsNullOrWhiteSpace(request.SubLocation))
            query = query.Where(l => l.SubLocation == request.SubLocation);

        if (!string.IsNullOrWhiteSpace(request.AccommodationType))
            query = query.Where(l => l.AccommodationType == request.AccommodationType);

        var results = await query
            .Select(l => new GetListingResponse
            (
                l.Id,
                l.HostId,
                l.Title,
                l.AccommodationType,
                l.MainLocation,
                l.SubLocation,
                l.Price,
                l.AvailableFrom,
                l.AvailableUntil,
                l.Images.Select(img => new ListingImageDto(img.Url, img.AltText)).ToList()
            ))
            .ToListAsync(cancellationToken);

        return results;
    }
}
