using System.Collections.Generic;
using FluentValidation;
using RicMonitoringAPI.RicXplorer.ViewModels;

namespace RicMonitoringAPI.RicXplorer.Validators
{
    public class GuestBookingDetailDtoValidator : AbstractValidator<GuestBookingDetailDto>
    {
        public GuestBookingDetailDtoValidator()
        {
            RuleFor(m => m.ArrivalDate).NotEmpty().WithMessage("Please select arrival date.");
            RuleFor(m => m.DepartureDate).NotEmpty().WithMessage("Please select departure date.");
            RuleFor(m => m.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(m => m.LanguagesSpoken).NotEmpty().WithMessage("Languages spoken is required.");
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(m => m.Contact).NotEmpty().WithMessage("Contact is required.");

            //for ICollection validator
            RuleForEach(m => m.GuestBookings).SetValidator(new GuestBookingDtoValidator());
        }
    }
}
