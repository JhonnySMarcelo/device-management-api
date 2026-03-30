namespace DeviceManagementApi.Application.Validators
{
    using DeviceManagementApi.Application.DTOs;
    using FluentValidation;

    public class UpdateDeviceRequestValidator : AbstractValidator<UpdateDeviceRequest>
    {
        public UpdateDeviceRequestValidator()
        {
            RuleFor(x => x.Name)
                .Must(s => string.IsNullOrWhiteSpace(s) || !string.IsNullOrWhiteSpace(s))
                .WithMessage("Name cannot be whitespace.")
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Brand)
                .Must(s => string.IsNullOrWhiteSpace(s) || !string.IsNullOrWhiteSpace(s))
                .WithMessage("Brand cannot be whitespace.")
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Brand));

            RuleFor(x => x.State)
                .IsInEnum()
                .When(x => x.State.HasValue);
        }
    }

}
