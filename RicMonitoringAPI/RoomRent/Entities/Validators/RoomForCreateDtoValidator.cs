using FluentValidation;
using RicMonitoringAPI.RoomRent.Models;

namespace RicMonitoringAPI.RoomRent.Entities.Validators
{
    public class RoomForCreateDtoValidator : AbstractValidator<RoomForCreateDto>
    {
        public RoomForCreateDtoValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Room name must not be empty.");
            RuleFor(m => m.Frequency).NotEmpty().WithMessage("Frequency must not be empty.");
            RuleFor(m => m.Price).NotEmpty().WithMessage("Price must not be zero.");
        }
    }
}
