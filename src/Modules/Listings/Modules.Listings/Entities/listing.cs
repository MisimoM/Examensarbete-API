namespace Modules.Listings.Entities;

//Kommentar
internal class Listing
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string AccommodationType { get; set; } = default!;
    public string MainLocation { get; set; } = default!;
    public string SubLocation { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }
}
