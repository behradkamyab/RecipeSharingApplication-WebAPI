

using ModelsLibrary.Models;
using SharedModelsLibrary.UserDTOs;

namespace ServicesLibrary.Mappers
{
    public static class ApplicationUserMapper
    {
        public static UserViewModel AsUserViewModel(this ApplicationUser user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
