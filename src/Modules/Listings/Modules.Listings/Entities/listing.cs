namespace Modules.Listings.Entities;

public class Listing
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string AccommodationType { get; set; } = default!;
    public string MainLocation { get; set; } = default!;
    public string SubLocation { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }


    public ICollection<ListingImage> Images { get; set; } = [];
}
