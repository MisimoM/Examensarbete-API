namespace Modules.Bookings.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ListingId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal PricePerNight { get; private set; }
    public int NumberOfNights { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Status { get; private set; } = "Pending";

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private Booking(Guid userId, Guid listingId, DateTime startDate, DateTime endDate, decimal pricePerNight)
    {
        if (startDate >= endDate)
            throw new InvalidOperationException("Start date must be earlier than end date.");

        if (pricePerNight <= 0)
            throw new InvalidOperationException("Price per night must be greater than zero.");

        UserId = userId;
        ListingId = listingId;
        StartDate = startDate;
        EndDate = endDate;
        PricePerNight = pricePerNight;
        NumberOfNights = (endDate - startDate).Days;
        TotalPrice = NumberOfNights * PricePerNight;
    }

    public static Booking Create(Guid userId, Guid listingId, DateTime startDate, DateTime endDate, decimal pricePerNight)
    {
        return new Booking(userId, listingId, startDate, endDate, pricePerNight);
    }

    public void ConfirmPayment()
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Only pending bookings can be marked as paid.");

        Status = "Confirmed";
        UpdatedAt = DateTime.UtcNow;
    }
}