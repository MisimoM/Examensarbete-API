using FluentValidation;

namespace Modules.Listings.Features.GetListingById;

public class GetListingByIdValidator : AbstractValidator<GetListingByIdRequest>
{
    public GetListingByIdValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Id must be a valid GUID.");
    }
}
