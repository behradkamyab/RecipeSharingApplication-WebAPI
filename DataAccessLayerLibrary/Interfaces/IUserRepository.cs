using ModelsLibrary.Models;
using SharedModelsLibrary.RecipeDTOs;


namespace DataAccessLayerLibrary.Interfaces
{
    public interface IUserRepository
    {

        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<bool> IsUserValidAsync(string userId);
        Task<bool> IsEmailValidAsync(string email);


        Task CreateProfileAsync(UserProfileModel profile);
        Task DeleteProfileAsync(Guid profileId);
        Task UpdateProfileAsync(UserProfileModel profile);
        Task<UserProfileModel?> GetProfileAsync(string userId);

        Task AddRecipeToUserFavoritesAsync(UserFavoriteRecipe userFavoriteRecipe);
        Task RemoveRecipeFromUserFavoritesAsync( string userId,Guid recipeId);
        Task<IEnumerable<RecipeModel>?> GetAllFavoritesRecipesForUserAsync(string userId);
        Task<RecipeModel?> GetUserFavoritesRecipeDetailsAsync(string userId, Guid recipeId);
        Task<bool> IsRecipeAlreadyInUserFavoritesListAsync(string userId,Guid recipeId);

        Task SaveFollowAsync(Follow follow);
        Task UnfollowAsync(string followerId , string followedId);

        Task<IEnumerable<UserProfileModel>?> GetAllFollowersAsync(string userId); // get all of the followers of the auhtorized user
        Task<IEnumerable<UserProfileModel>?> GetAllFollowingsAsync(string userId); // get all of the followings of the auhtorized user

        Task<int> GetAllFollowersCountAsync(string userId);
        Task<Dictionary<string, int>> GetAllFollowersCountForOtherUsersAsync(List<string> userIds); //get all of the number of followings and followers for all of the auhtorized
                                                                                                    //user followings and followers
        Task<int> GetAllFollowingCountAsync(string userId);
        Task<Dictionary<string, int>> GetAllFollowingsCountForOtherUsersAsync(List<string> userIds);
        Task<bool> IsFollowAvailableAsync(string followerId, string followedId);

    }
}
