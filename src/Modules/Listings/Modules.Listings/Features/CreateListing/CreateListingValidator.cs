using FluentValidation;

namespace Modules.Listings.Features.CreateListing;

public class CreateListingValidator : AbstractValidator<CreateListingRequest>
{
    public CreateListingValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.AccommodationType)
            .NotEmpty().WithMessage("Accommodation type is required.")
            .MaximumLength(100).WithMessage("Accommodation type cannot exceed 100 characters.");

        RuleFor(x => x.MainLocation)
            .NotEmpty().WithMessage("Main location is required.")
            .MaximumLength(200).WithMessage("Main location cannot exceed 200 characters.");

        RuleFor(x => x.SubLocation)
            .NotEmpty().WithMessage("Sub location is required.")
            .MaximumLength(200).WithMessage("Sub location cannot exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.AvailableFrom)
            .LessThan(x => x.AvailableUntil).WithMessage("Available from must be before Available until.");

        RuleForEach(x => x.Images).ChildRules(images =>
        {
            images.RuleFor(img => img.Url)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(500).WithMessage("URL cannot exceed 500 characters.");

            images.RuleFor(img => img.AltText)
                .NotEmpty().WithMessage("Alt text is required.")
                .MaximumLength(200).WithMessage("Alt text cannot exceed 200 characters.");
        });
    }
}
