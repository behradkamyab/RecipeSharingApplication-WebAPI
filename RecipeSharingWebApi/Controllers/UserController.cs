using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RecipeSharingWebApi.Hubs;
using ServicesLibrary.Delegates;
using ServicesLibrary.Interfaces;
using System.Security.Claims;

namespace RecipeSharingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IRecipeManager _recipeManager;
        private readonly INotificationManager _notificationManager;
        public UserController(IUserManager userManager , IRecipeManager recipeManager , INotificationManager notifManager)
        {
            _userManager = userManager;
            _recipeManager = recipeManager;
            _notificationManager = notifManager;
        }


        // created a delegate in my service layer and delegate to fetch the number of recipes created by user to that delegate
        // GET /api/user/profile/
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }


            var result = await _userManager.GetProfileAsync(userId , async (userId) =>
            {
                var numbers = await _recipeManager.GetAllNumbersOfRecipesCreatedByUserAsync (userId);
                return numbers;
            });

            if (result.Success) 
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }



        //POST (PUT) /api/user/profile/bio
        [HttpPost("profile/bio")]
        public async Task<IActionResult> ChangeBio([FromBody] string bio)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User Id not found in token");
                }

                if (string.IsNullOrEmpty(bio))
                {
                    return BadRequest("Bio cannot be null or empty");
                }


                var result = await _userManager.ChangeBioForUserAsync(userId, bio);
                if (result.Success)
                {
                    await _notificationManager.NotifyUserAsync(userId, "You changed your bio!", "Profile");
                    return Ok(result);
                }
                else
                {

                    if (result.Message.Contains("not found"))
                    {
                        return NotFound(result);
                    }

                    return BadRequest(result);

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
           

        }

        // POST /api/user/favorites/add/{recipeId}
        [Authorize]
        [HttpPost("favorites/add/{recipeId:guid}")]
        public async Task<IActionResult> AddRecipeToFavorites([FromRoute] Guid recipeId)
        {
           
                if (recipeId == Guid.Empty)
                {
                    return BadRequest("Please provide recipe id");
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User Id not found in token");
                }
                var result = await _userManager.AddRecipeToUserFavoriteAsync(recipeId, userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
        }

        //DELETE /api/user/favorites/remove/{recipeId}
        [Authorize]
        [HttpDelete("favorites/remove/{recipeId:guid}")]

        public async Task<IActionResult> RemoveRecipeFromFavorites([FromRoute] Guid recipeId)
        {
            if (recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _userManager.RemoveRecipeFromUserFavoriteAsync(recipeId, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //GET /api/user/favorites/

        [Authorize]
        [HttpGet("favorites")]
        public async Task<IActionResult> GetAllFavoriteRecipes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _userManager.GetAllUserFavoritesRecipesAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        // GET /api/user/favorites/{recipeId}
        [Authorize]
        [HttpGet("favorites/{recipeId:guid}")]
        public async Task<IActionResult> GetFavoriteRecipe([FromRoute] Guid recipeId)
        {
            if (recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _userManager.GetUserFavoritesRecipeDetailsAsync(userId, recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // POST /api/user/follow
        [Authorize]
        [HttpPost]
        [Route("follow")]
        public async Task<IActionResult> FollowUser([FromBody]string followedId)
        {
            try
            {
                if (string.IsNullOrEmpty(followedId))
                {
                    return BadRequest("Please provide users Id");
                }
                var followerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var followerName = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(followerId))
                {
                    return Unauthorized("User Id not found in token");
                }
                var result = await _userManager.FollowUserAsync(followerId, followedId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                await _notificationManager.NotifyUserAsync(followedId, $"{followerName} started following you.", "Follow");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
            
        }

        //POST /api/user/unfollow
        [Authorize , HttpPost("unfollow/{followedId}")]
        public async Task<IActionResult> UnfollowUser([FromRoute] string followedId)
        {
            if (string.IsNullOrEmpty(followedId))
            {
                return BadRequest("Please provide users Id");
            }
            var followerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(followerId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _userManager.UnfollowUserAsync(followerId, followedId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

       // GET /api/User/followers? numbers = true
        [Authorize, HttpGet("followers")]
        public async Task<IActionResult> GetAllFollowers([FromQuery] bool numbers)
        {
            var followerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(followerId))
            {
                return Unauthorized("User Id not found in token");
            }
            if (numbers == true)
            {
                var response = await _userManager.GetAllFollowersCountForUserAsync(followerId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);

            }
           
            var result = await _userManager.GetAllFollowersForUserAsync(followerId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

       // GET /api/User/followings
        [Authorize, HttpGet("followings")]
        public async Task<IActionResult> GetAllFollowings([FromQuery] bool numbers)
        {
            var followerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(followerId))
            {
                return Unauthorized("User Id not found in token");
            }

            if (numbers == true)
            {
                var response = await _userManager.GetAllFollowingsCountForUserAsync(followerId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);

            }
            
            var result = await _userManager.GetAllFollowingForUserAsync(followerId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }

}

