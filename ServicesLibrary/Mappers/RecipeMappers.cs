using ModelsLibrary.Models;
using SharedModelsLibrary.RecipeDto;
using SharedModelsLibrary.RecipeDTOs;

namespace ServicesLibrary.Mappers
{
    public static class RecipeMappers
    {
        public static RecipeViewModel AsRecipeViewModel(this RecipeModel recipe)
        {
            return new RecipeViewModel
            {
                RecipeId = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Category = recipe.Category,
                Cuisine = recipe.Cuisine
            };
        }

        public static RecipeFullDetailsViewModel AsRecipeFullDetailsViewModel(this RecipeModel recipe)
        {
            return new RecipeFullDetailsViewModel()
            {

                Name = recipe.Name,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions,
                Category = recipe.Category,
                Cuisine = recipe.Cuisine,
                Likes = recipe.Likes,
                CreatorUserId = recipe.UserId,

            };
        }
    }
}
