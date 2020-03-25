using FluentValidation;
using RicMonitoringAPI.RoomRent.Models;

namespace RicMonitoringAPI.RoomRent.Entities.Validators
{
    public class RenterForCreateDtoValidator : AbstractValidator<RenterForCreateDto>
    {
        public RenterForCreateDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Renter name must not be empty.");
            RuleFor(m => m.AdvanceMonths).NotEmpty().WithMessage("Advance months is required.");
            RuleFor(m => m.AdvancePaidDate).NotEmpty().WithMessage("Please select advance paid date.");
            RuleFor(m => m.StartDate).NotEmpty().WithMessage("Please select start date.");
            RuleFor(m => m.DueDay).NotEmpty().WithMessage("Please select due date.");
            RuleFor(m => m.NoOfPersons).NotEmpty().WithMessage("Number of persons is required.");
            RuleFor(m => m.RoomId).NotEmpty().WithMessage("Please select room.");
        }
    }
}
