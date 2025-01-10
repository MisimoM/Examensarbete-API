using FluentValidation;
using System;
namespace Modules.Bookings.Application.Bookings.CreateBooking;

public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingValidator()
    {
        RuleFor(request => request.ListingId)
            .NotEmpty()
            .WithMessage("ListingId is required.");

        RuleFor(request => request.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("StartDate must be today or a future date.");

        RuleFor(request => request.EndDate)
            .GreaterThan(request => request.StartDate)
            .WithMessage("EndDate must be later than StartDate.");
    }
}
