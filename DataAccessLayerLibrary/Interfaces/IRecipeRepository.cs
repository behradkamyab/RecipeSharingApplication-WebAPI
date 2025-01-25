using System.Linq.Expressions;
using DataAccessLayerLibrary.Exceptions;
using DataAccessLayerLibrary.Queries;
using ModelsLibrary.Models;


namespace DataAccessLayerLibrary.Interfaces
{
    public interface IRecipeRepository
    {
        Task AddRecipeAsync(RecipeModel recipe);
        Task RemoveRecipeAsync(Guid recipeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        /// /// <exception cref="DataAccessException">happen if something</exception>
        Task UpdateRecipeAsync(RecipeModel recipe);
        Task<IEnumerable<RecipeModel>> FindRecipeAsync(Expression<Func<RecipeModel, bool>> predicate);
        Task<RecipeModel?> GetRecipeAsync(Guid id);
        Task<RecipeModel?> GetFullDetailsRecipeAsync(Guid id);

        Task<IEnumerable<RecipeModel>?> GetAllRecipesRandomlyAsync(PageNumberQueryParameters query);
        Task<IEnumerable<RecipeModel>?> GetAllFilterRecipesAsync(RecipeQueryParameters query);
        Task<IEnumerable<RecipeModel>?> GetAllRecipesCreatedByUser(string userId);
        Task<int> GetAllNumbersOfRecipesCreatedByUserAsync(string userId);
        Task<Dictionary<string, int>> GetAllNumbersOfRecipesCreatedByUserForOtherUsersAsync(List<string> userIds);


        Task AddIngredientAsync(IngredientRecipe ingredientRecipe);
        Task RemoveIngredientAsync(Guid ingredientRecipeId);
        Task UpdateIngredientAsync(IngredientRecipe ingredientRecipe);
        Task<IngredientRecipe?> GetIngredientByIdAsync(Guid ingredientId);
        Task<IEnumerable<IngredientRecipe>?> GetAllIgredientsOfRecipe(Guid recipeId);
        Task<bool> IsIngredientAvailable(Guid ingredientId , Guid recipeId);



        Task AddInstructionAsync(InstructionRecipe instructionRecipe);
        Task RemoveInstructionAsync(Guid instructionId);
        Task UpdateInstructionAsync(InstructionRecipe instructionRecipe);
        Task<int> GetLastInstructionStep(Guid recipeId);
        Task<IEnumerable<InstructionRecipe>?> FindInstructionAsync(Expression<Func<InstructionRecipe, bool>> predicate);
        Task<InstructionRecipe?> GetInstructionByIdAsync(Guid instructionId);

        Task AddLikeToRecipeAsync(Like like);
        Task RemoveLikeFromRecipeAsync(Like like);
        Task<Like?> GetLikeForRecipeAsync(string userId, Guid recipeId);
        Task<int> GetAllLikesForRecipeAsync(Guid recipeId);

        Task<bool> IsRecipeOwnedByUserASync(string userId, Guid recipeId);
        Task<bool> IsRecipeAvailable(Guid recipeId);


    }
}
