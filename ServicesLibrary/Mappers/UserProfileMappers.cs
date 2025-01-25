using ModelsLibrary.Models;
using SharedModelsLibrary.UserProfileDTOs;


namespace ServicesLibrary.Mappers
{
    public static class UserProfileMappers
    {
        public static  ProfileViewModel AsProfileViewModel(this UserProfileModel profile, int followingsCount, int followersCount, int recipeCount)
        {
            return new ProfileViewModel
            {
                Email = profile.Email,
                Bio = profile.Bio,
                Followings = followingsCount,
                Followers = followersCount,
                CreatedRecipes = recipeCount
            };
        }
    }
}
