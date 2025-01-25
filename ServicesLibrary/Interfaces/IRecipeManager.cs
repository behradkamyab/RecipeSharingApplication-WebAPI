using SharedModelsLibrary.RecipeDto;
using ModelsLibrary.Models;
using ServicesLibrary.Responses;
using DataAccessLayerLibrary.Queries;
using SharedModelsLibrary.RecipeDTOs;
using SharedModelsLibrary.RecipeRequests;
using SharedModelsLibrary.IngredientRequests;
using SharedModelsLibrary.InstructionRequests;


namespace ServicesLibrary.Interfaces
{
    public interface IRecipeManager
    {
        Task<RecipeManagerResponse<RecipeViewModel>> CreateRecipeAsync(string userId, CreateRecipeRequest recipe);
        Task<RecipeManagerResponse<RecipeViewModel>> UpdateRecipeAsync(string userId, Guid recipeId,UpdateRecipeRequest recipe);
        Task<RecipeManagerResponse<string>> RemoveRecipeAsync(string userId, Guid recipeId);
        Task<RecipeManagerResponse<RecipeFullDetailsViewModel>> GetRecipeByIdAsync(Guid recipeId);

        
        Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllRecipesFeedAsync(PageNumberQueryParameters query);
        Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllFilterRecipesAsync(RecipeQueryParameters query);
        Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllRecipesCreatedByUserAsync(string userId);
        Task<RecipeManagerResponse<Dictionary<string, int>>> GetAllNumbersOfRecipesCreatedByUserForOtherUsersAsync(List<string> userIds);
        Task<RecipeManagerResponse<int>> GetAllNumbersOfRecipesCreatedByUserAsync(string userId);


        Task<RecipeManagerResponse<IngredientRecipe>> CreateIngredientAsync(CreateIngredientRequest ingredient, Guid recipeId , string userId);
        Task<RecipeManagerResponse<string>> RemoveIngredientAsync(Guid ingredientId, Guid recipeId, string userId);
        Task<RecipeManagerResponse<IngredientRecipe>> UpdateIngredientAsync(UpdateIngredientRequest ingredientModel,Guid recipeId,string userId , Guid ingredientId );
        Task<RecipeManagerResponse<IngredientRecipe>> GetIngredientByIdAsync(Guid ingredientId , Guid recipeId);
        Task<RecipeManagerResponse<IEnumerable<IngredientRecipe>>> GetAllIngredientsOfRecipeAsync(Guid recipeId);


        Task<RecipeManagerResponse<InstructionRecipe>> CreateInstructionAsync(Guid recipeId, string content, string userId);
        Task<RecipeManagerResponse<string>> RemoveInstructionAsync(Guid instructionId,Guid recipeId, string userId);
        Task<RecipeManagerResponse<InstructionRecipe>> UpdateInstructionAsync(UpdateInstructionRequest instructionModel,
            Guid recipeId, string userId);


        Task<RecipeManagerResponse<string>> AddLikeToRecipeAsync(string userId, Guid recipeId);
        Task<RecipeManagerResponse<string>> RemoveLikeFromRecipeAsync(string userId, Guid recipeId);

        Task<RecipeManagerResponse<int>> GetAllLikesAsync(Guid recipeId);
    }
}
