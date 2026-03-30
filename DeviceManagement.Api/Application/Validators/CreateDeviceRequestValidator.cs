namespace DeviceManagementApi.Application.Validators
{
    using DeviceManagementApi.Application.DTOs;
    using FluentValidation;

    public class CreateDeviceRequestValidator : AbstractValidator<CreateDeviceRequest>
    {
        public CreateDeviceRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Name cannot be whitespace.")
                .MaximumLength(100);

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Brand cannot be whitespace.")
                .MaximumLength(100);
        }
    }

}
