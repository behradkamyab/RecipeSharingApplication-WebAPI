
using DataAccessLayerLibrary.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Interfaces;
using ServicesLibrary.Managers;
using SharedModelsLibrary.IngredientRequests;
using SharedModelsLibrary.InstructionRequests;
using SharedModelsLibrary.RecipeRequests;
using System.Security.Claims;




namespace RecipeSharingWebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeManager _recipeManager;
        private readonly INotificationManager _notificationManager;

        public RecipeController(IRecipeManager recipeManager, INotificationManager notificationManager)
        {
            _recipeManager = recipeManager;
            _notificationManager = notificationManager;
        }


        //POST /api/recipe/create
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeRequest recipeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.CreateRecipeAsync(userId, recipeModel);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //DELETE /api/recipe/delete/{id}
        [Authorize]
        [HttpDelete("delete/{recipeId:guid}")]
        public async Task<IActionResult> DeleteRecipe([FromRoute] Guid recipeId)
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
            var result = await _recipeManager.RemoveRecipeAsync(userId, recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        //PUT /api/Recipe/update/{RecipeId}
        [Authorize]
        [HttpPut("update/{recipeId:guid}")]
        public async Task<IActionResult> UpdateRecipe([FromRoute] Guid recipeId,[FromBody] UpdateRecipeRequest recipeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _recipeManager.UpdateRecipeAsync(userId, recipeId,recipeModel);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //GET /api/recipe/{id}
        [Authorize, HttpGet("{recipeId:guid}")]
        public async Task<IActionResult> GetRecipe([FromRoute] Guid recipeId)
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
            var result = await _recipeManager.GetRecipeByIdAsync(recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //GET /api/recipe/all/feed?PageNumber=""&PageSize=""
        [Authorize, HttpGet("all/feed")]
        public async Task<IActionResult> GetAllRecipes([FromQuery] PageNumberQueryParameters queryParameters)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _recipeManager.GetAllRecipesFeedAsync(queryParameters);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //GET /api/recipe/all/filter?name=""?category=""?cuisine=""
        [Authorize]
        [HttpGet("all/filter")]
        public async Task<IActionResult> GetAllFilterRecipes([FromQuery] RecipeQueryParameters query)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _recipeManager.GetAllFilterRecipesAsync(query);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        //GET /api/recipe/all/created-by-user/
        [Authorize, HttpGet("all/created-by-user")]

        public async Task<IActionResult> GetAllRecipesCreatedByuser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            var result = await _recipeManager.GetAllRecipesCreatedByUserAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        //POST /api/recipe/{recipeId}/ingredient/create

        [Authorize, HttpPost("{recipeId:guid}/ingredient/create")]
        public async Task<IActionResult> CreateIngredient([FromRoute] Guid recipeId, [FromBody] CreateIngredientRequest ingredient)
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

            var result = await _recipeManager.CreateIngredientAsync(ingredient, recipeId, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        //DELETE /api/recipe/{recipeId}/ingredient/remove/{ingredientId}
        [Authorize , HttpDelete("{recipeId:guid}/ingredient/remove/{ingredientId:guid}")]
        public async Task<IActionResult> RemoveIngredient([FromRoute] Guid recipeId , [FromRoute] Guid ingredientId)
        {
            if (ingredientId == Guid.Empty || recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or ingredient id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.RemoveIngredientAsync(ingredientId, recipeId , userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //PUT /api/recipe/{recipeId}/ingredient/update/{ingredientId}
        [Authorize, HttpPut("{recipeId:guid}/ingredient/update/{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient([FromRoute] Guid recipeId,Guid ingredientId, [FromBody] UpdateIngredientRequest ingredient)
        {
            if (ingredientId == Guid.Empty || recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or ingredient id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }
            

            var result = await _recipeManager.UpdateIngredientAsync(ingredient,recipeId,userId ,ingredientId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //GET /api/recipe/{recipeId}/ingredient/{id}
        [Authorize,HttpGet("{recipeId:guid}/ingredient/{ingredientId:guid}")]

        public async Task<IActionResult> GetIngredient([FromRoute] Guid recipeId, Guid ingredientId)
        {
            if (ingredientId == Guid.Empty || recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or ingredient id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.GetIngredientByIdAsync(ingredientId,recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //GET /api/recipe/{recipeId}/ingredients
        [Authorize, HttpGet("{recipeId:guid}/ingredients")]

        public async Task<IActionResult> GetAllIngredients([FromRoute] Guid recipeId)
        {
            if ( recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or ingredient id");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.GetAllIngredientsOfRecipeAsync(recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        //POST /api/recipe/{recipeId}/instruction/create

        [Authorize, HttpPost("{recipeId:guid}/instruction/create")]
        public async Task<IActionResult> CreateInstruction([FromRoute] Guid recipeId, [FromBody] string content)
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

            var result = await _recipeManager.CreateInstructionAsync(recipeId,content , userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //DELETE /api/recipe/{recipeId}/instruction/remove/{instructionId}
        [Authorize, HttpDelete("{recipeId:guid}/instruction/remove/{instructionId:guid}")]
        public async Task<IActionResult> RemoveInstruction([FromRoute] Guid recipeId,  Guid instructionId)
        {
            if (instructionId == Guid.Empty || recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or correct instruction step");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.RemoveInstructionAsync(instructionId,recipeId, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //PUT /api/recipe/{recipeId}/instruction/update
        [Authorize, HttpPut("{recipeId:guid}/instruction/update/")]
        public async Task<IActionResult> UpdateIngredient([FromRoute] Guid recipeId, [FromBody] UpdateInstructionRequest instruction)
        {
            if (instruction.InstructionId == Guid.Empty || recipeId == Guid.Empty)
            {
                return BadRequest("Please provide recipe id or correct instruction step");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id not found in token");
            }

            var result = await _recipeManager.UpdateInstructionAsync(instruction,recipeId, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //POST /api/recipe/{recipeId}/like
        [Authorize, HttpPost("{recipeId:guid}/like")]
        public async Task<IActionResult> AddLikeToRecipe([FromRoute] Guid recipeId)
        {
            try
            {
                if (recipeId == Guid.Empty)
                {
                    return BadRequest("Please provide recipe id");
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User Id not found in token");
                }
                var result = await _recipeManager.AddLikeToRecipeAsync(userId, recipeId);
                var recipeResponse = await _recipeManager.GetRecipeByIdAsync(recipeId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                await _notificationManager.NotifyUserAsync(recipeResponse.Data.CreatorUserId, $"{userEmail} Liked your recipe: {recipeResponse.Data.Name}.", "Like");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
            

        }

        //DELETE /api/recipe/{recipeId}/like
        [Authorize , HttpDelete("{recipeId:guid}/like")]
        public async Task<IActionResult> RemoveLikeFromRecipe([FromRoute] Guid recipeId)
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
            var result = await _recipeManager.RemoveLikeFromRecipeAsync(userId, recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        //GET /api/recipe/{recipeId}/likes
        [Authorize, HttpGet("{recipeId:guid}/likes")]
        public async Task<IActionResult> GetAllLikes([FromRoute] Guid recipeId)
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
            var result = await _recipeManager.GetAllLikesAsync(recipeId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
