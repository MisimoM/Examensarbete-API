namespace Modules.Listings.Entities;

public class ListingImage
{
    public int Id { get; set; }
    public Guid ListingId { get; set; }
    public string Url { get; set; } = default!;
    public string AltText { get; set; } = default!;

    public Listing Listing { get; set; } = default!;
}
