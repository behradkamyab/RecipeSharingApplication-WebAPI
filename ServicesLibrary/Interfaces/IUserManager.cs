using ModelsLibrary.Models;
using ServicesLibrary.Delegates;
using ServicesLibrary.Responses;
using SharedModelsLibrary.RecipeDto;
using SharedModelsLibrary.RecipeDTOs;
using SharedModelsLibrary.UserDTOs;
using SharedModelsLibrary.UserProfileDTOs;
using SharedModelsLibrary.UserRequests;

namespace ServicesLibrary.Interfaces
{
    public interface IUserManager
    {
        // Todo change password (Authorize) => user controller
        // Todo change email   (Authorize) => user controller
        // Todo enable two-factor (Auhtorize) => user controller
        // Todo Get one favorite recipe details

        // User Profile will be created automatically when the user register
        Task<UserManagerResponse<string>> RegisterUserASync(UserRegisterRequest user); //Auth controller
        Task<UserManagerResponse<TokenView>> LoginUserAsync(UserLoginRequest user); // Auth controller


        Task<UserManagerResponse<ProfileViewModel>> GetProfileAsync(string userId, GetRecipeCountForOneUserDelegate getRecipesCountAsync); // user controller
        Task<UserManagerResponse<string>> ChangeBioForUserAsync(string userId, string bio);  // user controller

        Task<UserManagerResponse<string>> AddRecipeToUserFavoriteAsync(Guid recipeId, string userId); // user controller
        Task<UserManagerResponse<string>> RemoveRecipeFromUserFavoriteAsync(Guid recipeId, string userId); // user controller
        Task<UserManagerResponse<IEnumerable<RecipeViewModel>>> GetAllUserFavoritesRecipesAsync(string userId); // user controller
        Task<UserManagerResponse<RecipeFullDetailsViewModel>> GetUserFavoritesRecipeDetailsAsync(string userId, Guid recipeId);

        Task<UserManagerResponse<string>> FollowUserAsync(string followerId, string followedId);
        Task<UserManagerResponse<string>> UnfollowUserAsync(string followerId, string followedId);
        Task<UserManagerResponse<IEnumerable<ProfileViewModel>>> GetAllFollowersForUserAsync(string userId, GetRecipeCountDelegate getRecipeCountAsync);
        Task<UserManagerResponse<IEnumerable<ProfileViewModel>>> GetAllFollowingForUserAsync(string userId, GetRecipeCountDelegate getRecipeCountAsync);
        Task<UserManagerResponse<int>> GetAllFollowersCountForUserAsync(string userId);
        Task<UserManagerResponse<int>> GetAllFollowingsCountForUserAsync(string userId);



    }
}
