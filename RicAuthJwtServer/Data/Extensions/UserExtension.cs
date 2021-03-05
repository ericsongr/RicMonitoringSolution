using RicAuthJwtServer.ViewModels;

namespace RicAuthJwtServer.Data.Extensions
{
    public static class UserExtension
    {
        public static UserViewModel Projection(this ApplicationUser user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
