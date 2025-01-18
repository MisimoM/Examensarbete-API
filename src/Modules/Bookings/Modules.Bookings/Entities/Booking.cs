namespace Modules.Bookings.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ListingId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Status { get; private set; } = "Pending";

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private Booking(Guid userId, Guid listingId, DateTime startDate, DateTime endDate, decimal totalPrice)
    {
        if (startDate >= endDate)
            throw new InvalidOperationException("Start date must be earlier than end date.");

        UserId = userId;
        ListingId = listingId;
        StartDate = startDate;
        EndDate = endDate;
        TotalPrice = totalPrice;
    }

    public static Booking Create(Guid userId, Guid listingId, DateTime startDate, DateTime endDate, decimal pricePerNight)
    {
        var totalPrice = CalculateTotalPrice(startDate, endDate, pricePerNight);
        return new Booking(userId, listingId, startDate, endDate, totalPrice);
    }

    private static decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerNight)
    {
        var totalDays = (endDate - startDate).Days;
        if (totalDays <= 0)
            throw new InvalidOperationException("End date must be later than start date.");

        return totalDays * pricePerNight;
    }

    public void Confirm()
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Only pending bookings can be confirmed.");

        Status = "Confirmed";
        UpdatedAt = DateTime.UtcNow;
    }
}