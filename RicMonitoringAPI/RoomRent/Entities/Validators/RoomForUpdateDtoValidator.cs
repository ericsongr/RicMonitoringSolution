using FluentValidation;
using RicMonitoringAPI.RoomRent.Models;

namespace RicMonitoringAPI.RoomRent.Entities.Validators
{
    public class RoomForUpdateDtoValidator : AbstractValidator<RoomForUpdateDto>
    {
        public RoomForUpdateDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Room name must not be empty.");
            RuleFor(m => m.Frequency).NotEmpty().WithMessage("Frequency must not be empty.");
            RuleFor(m => m.Price).NotEmpty().WithMessage("Price must not be zero.");
        }
    }
}
