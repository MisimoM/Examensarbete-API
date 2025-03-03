using FluentValidation;
namespace Modules.Bookings.Features.Bookings.CreateBooking;

public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingValidator()
    {
        RuleFor(request => request.ListingId)
            .NotEmpty()
            .WithMessage("ListingId is required.");

        RuleFor(request => request.StartDate)
            .NotEmpty()
            .WithMessage("StartDate is required")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("StartDate must be today or a future date.");

        RuleFor(request => request.EndDate)
            .NotEmpty()
            .WithMessage("EndDate is required")
            .GreaterThan(request => request.StartDate)
            .WithMessage("EndDate must be later than StartDate.");
    }
}
