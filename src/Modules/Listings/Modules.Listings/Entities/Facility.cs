namespace Modules.Listings.Entities;

public class Facility
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ListingFacility> ListingFacilities { get; set; }

    public Facility(string name)
    {
        Name = name;
        ListingFacilities = [];
    }
}
