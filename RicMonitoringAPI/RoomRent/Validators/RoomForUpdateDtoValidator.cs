using FluentValidation;
using RicModel.RoomRent.Dtos;

namespace RicMonitoringAPI.RoomRent.Validators
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
