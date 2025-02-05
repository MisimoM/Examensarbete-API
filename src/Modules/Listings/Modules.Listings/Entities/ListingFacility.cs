namespace Modules.Listings.Entities;

public class ListingFacility
{
    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public int FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;

}
