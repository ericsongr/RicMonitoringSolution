using FluentValidation;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicMonitoringAPI.RicXplorer.Validators
{
    public class GuestBookingDtoValidator : AbstractValidator<GuestBookingDto>
    {
        public GuestBookingDtoValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last name spoken is required.");
            RuleFor(m => m.Gender).NotEmpty().WithMessage("Gender is required.");
            RuleFor(m => m.Birthday).NotEmpty().WithMessage("Birthday is required.");
        }
    }
}
