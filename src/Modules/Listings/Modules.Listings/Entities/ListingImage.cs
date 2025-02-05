namespace Modules.Listings.Entities;

public class ListingImage
{
    public int Id { get; set; }
    public Guid ListingId { get; set; }
    public string Url { get; set; }
    public string AltText { get; set; }
    public Listing Listing { get; set; } = null!;

    public ListingImage(Guid listingId, string url, string altText)
    {
        ListingId = listingId;
        Url = url;
        AltText = altText;
    }
}
