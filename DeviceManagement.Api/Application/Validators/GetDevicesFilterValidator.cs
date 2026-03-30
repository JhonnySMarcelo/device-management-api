using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain;
using FluentValidation;

namespace DeviceManagementApi.Application.Validators
{
    public class GetDevicesFilterValidator : AbstractValidator<GetDevicesFilter>
    {
        public GetDevicesFilterValidator()
        {
            RuleFor(x => x.Brand)
                        .MaximumLength(100).WithMessage("Brand must be at most 100 characters.")
                        .Must(s => string.IsNullOrEmpty(s) || !string.IsNullOrWhiteSpace(s))
                        .WithMessage("Brand cannot be only whitespace.");

            RuleFor(x => x.State)
                .IsInEnum().When(x => x.State.HasValue)
                .WithMessage("State must be a valid enum value.");
        }
    }

}
