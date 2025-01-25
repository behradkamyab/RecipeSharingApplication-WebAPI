using ModelsLibrary.Models;
using Microsoft.AspNetCore.Identity;
using ServicesLibrary.Interfaces;
using ServicesLibrary.Mappers;
using ServicesLibrary.Responses;
using SharedModelsLibrary.UserDTOs;
using ServicesLibrary.Generators;
using DataAccessLayerLibrary.Exceptions;
using SharedModelsLibrary.UserRequests;
using ServicesLibrary.Exceptions.UserManagerExceptions;
using DataAccessLayerLibrary.Interfaces;
using SharedModelsLibrary.UserProfileDTOs;
using SharedModelsLibrary.RecipeDto;
using SharedModelsLibrary.RecipeDTOs;
using ServicesLibrary.Delegates;

namespace ServicesLibrary.Managers
{

    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtProvider _provider;
        private readonly IRecipeRepository _recipeRepository;


        public UserManager(IUserRepository userRepo,UserManager<ApplicationUser> userManager,JwtProvider provider,IRecipeRepository recipeRepo)
        {
            _userRepository = userRepo;
            _userManager = userManager;
            _provider = provider;
            _recipeRepository = recipeRepo;
       }



        public async Task<UserManagerResponse<string>> RegisterUserASync(UserRegisterRequest userModel) 
        {
            try
            { 
                if(!await _userRepository.IsEmailValidAsync(userModel.Email))
                {
                    return new UserManagerResponse<string>
                    {
                        Success = false,
                        Message = "Email already exists",
                        Data = null
                    };
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };
                var result = await _userManager.CreateAsync(applicationUser, userModel.Password);


                if (!result.Succeeded)
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }

                await CreateProfileAsync(applicationUser.Email, applicationUser.Id);

                return new UserManagerResponse<string>()
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = userModel.Email
                };
     
            }
            catch (ArgumentNullException)
            {

                return new UserManagerResponse<string>
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
            catch (DataAccessException)
            {
                return new UserManagerResponse<string>
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
         
        }


        public async Task<UserManagerResponse<TokenView>> LoginUserAsync(UserLoginRequest userModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userModel.Email);
                if (user == null)
                {
                    return new UserManagerResponse<TokenView>()
                    {
                        Success = false,
                        Message = "Email not found",
                    };
                }

                var result = await _userManager.CheckPasswordAsync(user, userModel.Password);

                if (!result)
                {
                    return new UserManagerResponse<TokenView>()
                    {
                        Success = false,
                        Message = "Password is wrong!",
                    };
                }


                var tokenView = _provider.Create(user);


                return new UserManagerResponse<TokenView>()
                {
                    Success = true,
                    Message = "User login successfully",
                    Data = tokenView


                };

            }
            catch (TokenProviderException)
            {

                return new UserManagerResponse<TokenView>()
                {
                    Success = false,
                    Message = "Internal Server Error",

                };
            }
            
           
        }

        public async Task<UserManagerResponse<ProfileViewModel>> GetProfileAsync(string userId, GetRecipeCountForOneUserDelegate getRecipesCountAsync)
        {
            try
            {
                var profile = await _userRepository.GetProfileAsync(userId);
                if(profile == null)
                {
                    return new UserManagerResponse<ProfileViewModel>()
                    {
                        Success = false,
                        Message = "Profile not found",
                        Data = null
                    };
                }
               var followingsCount = await _userRepository.GetAllFollowingCountAsync(userId);
                var followersCount = await _userRepository.GetAllFollowersCountAsync(userId);

            
                var numbersoOfCreatedRecipe = await getRecipesCountAsync(userId);
                

                return new UserManagerResponse<ProfileViewModel>()
                {
                    Success = true,
                    Message = "Profile fetched successfully",
                    Data = profile.AsProfileViewModel(followingsCount, followersCount, numbersoOfCreatedRecipe.Data)
                };

            }
            catch (DataAccessException)
            {

                return new UserManagerResponse<ProfileViewModel>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<string>> ChangeBioForUserAsync(string userId, string bio)
        {
            try
            {
                var profile = await _userRepository.GetProfileAsync(userId);
                if (profile == null)
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "Profile not found",
                        Data = null
                    };
                }

                profile.Bio = bio;
                await _userRepository.UpdateProfileAsync(profile);
                return new UserManagerResponse<string>()
                {
                    Success = true,
                    Message = "your bio changed",
                    Data = profile.Bio
                };


            }
            catch (Exception)
            {

                return new UserManagerResponse<string>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }



        public async Task<UserManagerResponse<string>> AddRecipeToUserFavoriteAsync(Guid recipeId, string userId)
        {
            try
            {
               
                if (!await _userRepository.IsUserValidAsync(userId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "user not found",

                    };
                }
                
                if(!await _recipeRepository.IsRecipeAvailable(recipeId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "recipe not found"
                    };
                }

               if(await _userRepository.IsRecipeAlreadyInUserFavoritesListAsync(userId , recipeId))
                {
                    return new UserManagerResponse<string>
                    {
                        Success = false,
                        Message = "This recipe has already added to your favorites",

                    };
                }
                var userFavoriteRecipe = new UserFavoriteRecipe
                {
                    UserId = userId,
                    RecipeId = recipeId,
                };
            
               await _userRepository.AddRecipeToUserFavoritesAsync(userFavoriteRecipe);

                    return new UserManagerResponse<string>
                    {
                        Success = true,
                        Message = "Recipe Added to your favorites"
                    };
            }
            catch (Exception)
            {

                return new UserManagerResponse<string>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }

        }

 
        public async Task<UserManagerResponse<string>> RemoveRecipeFromUserFavoriteAsync(Guid recipeId, string userId)
        {
            try
            {
                var isValid = await _userRepository.IsUserValidAsync(userId);
                if (!isValid)
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "user not found",

                    };
                }

                if (!await _recipeRepository.IsRecipeAvailable(recipeId) || !await _userRepository.IsRecipeAlreadyInUserFavoritesListAsync(userId, recipeId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "recipe not found"
                    };
                }


                await _userRepository.RemoveRecipeFromUserFavoritesAsync(userId , recipeId);

                return new UserManagerResponse<string>
                {
                    Success = true,
                    Message = "Recipe Added to your favorites"
                };
            }
            catch (Exception)
            {

                return new UserManagerResponse<string>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<IEnumerable<RecipeViewModel>>> GetAllUserFavoritesRecipesAsync(string userId)
        {
            try
            {
                var isValid = await _userRepository.IsUserValidAsync(userId);
                if (!isValid)
                {
                    return new UserManagerResponse<IEnumerable<RecipeViewModel>>()
                    {
                        Success = false,
                        Message = "user not found",

                    };
                }

                var recipes = await _userRepository.GetAllFavoritesRecipesForUserAsync(userId);

                if(recipes == null || !recipes.Any())
                {
                    return new UserManagerResponse<IEnumerable<RecipeViewModel>>
                    {
                        Success = false,
                        Message = "No Favorite recipe found"
                    };
                }

                var recipesViewModel = recipes.Select(r => r.AsRecipeViewModel()).ToList();
                return new UserManagerResponse<IEnumerable<RecipeViewModel>>()
                {
                    Success = true,
                    Message = "Favorite recipes fetched",
                    Data = recipesViewModel
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<IEnumerable<RecipeViewModel>>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }


        public async Task<UserManagerResponse<RecipeFullDetailsViewModel>> GetUserFavoritesRecipeDetailsAsync(string userId, Guid recipeId)
        {
            try
            {
                var isValid = await _userRepository.IsUserValidAsync(userId);
                if (!isValid)
                {
                    return new UserManagerResponse<RecipeFullDetailsViewModel>
                    {
                        Success = false,
                        Message = "user not found",

                    };
                }

                var recipe = await _userRepository.GetUserFavoritesRecipeDetailsAsync(userId, recipeId);
                if(recipe == null)
                {
                    return new UserManagerResponse<RecipeFullDetailsViewModel>()
                    {
                        Success = false,
                        Message = "recipe not found",

                    };
                }

                return new UserManagerResponse<RecipeFullDetailsViewModel>()
                {
                    Success = true,
                    Message = "recipe fetched",
                    Data = recipe.AsRecipeFullDetailsViewModel()
                };
            }
            catch (Exception)
            {

                return new UserManagerResponse<RecipeFullDetailsViewModel>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        private async Task CreateProfileAsync(string email, string userId)
        {


            var profile = new UserProfileModel("your bio", email, userId);
            await _userRepository.CreateProfileAsync(profile);


        }

        public async Task<UserManagerResponse<string>> FollowUserAsync(string followerId, string followedId)
        {
            try
            {
                if (!await _userRepository.IsUserValidAsync(followerId) || ! await _userRepository.IsUserValidAsync(followedId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }
                if(await _userRepository.IsFollowAvailableAsync(followerId, followedId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "You already followed this user"
                    };
                }

                var newFollow = new Follow
                {
                    FollowerId = followerId,
                    FollowedId = followedId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _userRepository.SaveFollowAsync(newFollow);
                return new UserManagerResponse<string>()
                {
                    Success = true,
                    Message = "Followed this user"
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<string>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<string>> UnfollowUserAsync(string followerId, string followedId)
        {
            try
            {
                if (!await _userRepository.IsUserValidAsync(followerId) || !await _userRepository.IsUserValidAsync(followerId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }


                if (!await _userRepository.IsFollowAvailableAsync(followerId, followedId))
                {
                    return new UserManagerResponse<string>()
                    {
                        Success = false,
                        Message = "You cant unfollow this user"
                    };
                }
                await _userRepository.UnfollowAsync(followerId, followedId);
                return new UserManagerResponse<string>()
                {
                    Success = true,
                    Message = "You unfollowed this user"
                };

                
            }
            catch (Exception)
            {

                return new UserManagerResponse<string>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<IEnumerable<ProfileViewModel>>> GetAllFollowersForUserAsync(string userId , GetRecipeCountDelegate getRecipeCountAsync)
        {
            try
            {
                var followers = await _userRepository.GetAllFollowersAsync(userId);
                if (followers == null || !followers.Any())
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "You are not following anyone"
                    };
                }
                // get all of the ids of followings
                var followersUsersIds = followers.Select(f => f.UserId).ToList();
                // Get the results from the tasks
                var followersCounts = await _userRepository.GetAllFollowersCountForOtherUsersAsync(followersUsersIds);
                if (followersCounts == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch followers counts."
                    };
                }
                var followingsCounts = await _userRepository.GetAllFollowersCountForOtherUsersAsync(followersUsersIds);
                if (followingsCounts == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch followings counts."
                    };
                }
                var recipeCountsResponse = await getRecipeCountAsync(followersUsersIds);
                if (!recipeCountsResponse.Success || recipeCountsResponse.Data == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch recipe counts."
                    };
                }

                var recipeCounts = recipeCountsResponse.Data; // Extract the dictionary from the response

                var profileViewModels = new List<ProfileViewModel>();

                foreach (var follower in followers)
                {
                    // Look up the pre-fetched data for the user
                    followersCounts.TryGetValue(follower.UserId, out var followersCount);
                    followingsCounts.TryGetValue(follower.UserId, out var followingsCount);
                    recipeCounts.TryGetValue(follower.UserId, out var recipeCount);

                    profileViewModels.Add(follower.AsProfileViewModel(followingsCount, followersCount, recipeCount));
                }


                return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                {
                    Success = true,
                    Message = "List of all of your followers fetched",
                    Data = profileViewModels
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<IEnumerable<ProfileViewModel>>> GetAllFollowingForUserAsync(string userId, GetRecipeCountDelegate getRecipeCountAsync)
        {
            try
            {
                var followings = await _userRepository.GetAllFollowingsAsync(userId);
                if (followings == null || !followings.Any())
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "You are not following anyone"
                    };
                }
                // get all of the ids of followings
                var followingsUsersIds = followings.Select(f => f.UserId).ToList();

                var followersCounts = await _userRepository.GetAllFollowersCountForOtherUsersAsync(followingsUsersIds);
                if (followersCounts == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch followers counts."
                    };
                }
                var followingsCounts = await _userRepository.GetAllFollowingsCountForOtherUsersAsync(followingsUsersIds);
                if (followingsCounts == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch followings counts."
                    };
                }
                var recipeCountsResponse = await getRecipeCountAsync(followingsUsersIds);
                if (!recipeCountsResponse.Success || recipeCountsResponse.Data == null)
                {
                    return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                    {
                        Success = false,
                        Message = "Failed to fetch recipe counts."
                    };
                }

                var recipeCounts = recipeCountsResponse.Data; // Extract the dictionary from the response

                var profileViewModels = new List<ProfileViewModel>();  

                foreach(var following in followings)
                {
                    // Look up the pre-fetched data for the user
                    followersCounts.TryGetValue(following.UserId, out var followersCount);
                    followingsCounts.TryGetValue(following.UserId, out var followingsCount);
                    recipeCounts.TryGetValue(following.UserId, out var recipeCount);

                    profileViewModels.Add(following.AsProfileViewModel(followingsCount, followersCount, recipeCount));
                }
               

                return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                {
                    Success = true,
                    Message = "List of all of your followers fetched",
                    Data = profileViewModels
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<IEnumerable<ProfileViewModel>>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                    Data = null
                };
            }
        }

        public async Task<UserManagerResponse<int>> GetAllFollowersCountForUserAsync(string userId)
        {
            try
            {
                var followersNumber = await _userRepository.GetAllFollowersCountAsync(userId);
                
                return new UserManagerResponse<int>()
                {
                    Success = true,
                    Message = "List of all of your followers fetched",
                    Data = followersNumber
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<int>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                };
            }
        }

        public async Task<UserManagerResponse<int>> GetAllFollowingsCountForUserAsync(string userId)
        {
            try
            {
                var followingNumber = await _userRepository.GetAllFollowingCountAsync(userId);
               
                return new UserManagerResponse<int>()
                {
                    Success = true,
                    Message = "List of all of your followers fetched",
                    Data = followingNumber
                };

            }
            catch (Exception)
            {

                return new UserManagerResponse<int>()
                {
                    Success = false,
                    Message = "Internal Server Error",
                };
            }
        }
    }
}
