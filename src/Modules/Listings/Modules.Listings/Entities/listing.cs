namespace Modules.Listings.Entities;

public class Listing
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; }
    public string AccommodationType { get; set; }
    public string MainLocation { get; set; }
    public string SubLocation { get; set; }
    public decimal Price { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }

    public ICollection<ListingImage> Images { get; set; }
    public ICollection<ListingFacility> ListingFacilities { get; set; }

    public Listing(
        Guid id,
        Guid hostId,
        string title,
        string description,
        string accommodationType,
        string mainLocation,
        string subLocation,
        decimal price,
        DateTime availableFrom,
        DateTime availableUntil)
    {
        Id = id;
        HostId = hostId;
        Title = title;
        Description = description;
        AccommodationType = accommodationType;
        MainLocation = mainLocation;
        SubLocation = subLocation;
        Price = price;
        AvailableFrom = availableFrom;
        AvailableUntil = availableUntil;
        Images = [];
        ListingFacilities = [];
    }
}
