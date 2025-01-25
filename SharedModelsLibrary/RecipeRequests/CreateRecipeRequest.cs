using ModelsLibrary.Models;
using System.ComponentModel.DataAnnotations;


namespace SharedModelsLibrary.RecipeRequests
{
    public class CreateRecipeRequest
    {
        [Required]
        [MinLength(2)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        public IEnumerable<IngredientRecipe>? Ingredients { get; set; } = new List<IngredientRecipe>();

        public IEnumerable<InstructionRecipe>? Instructions { get; set; } = new List<InstructionRecipe>();

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Cuisine { get; set; }
    }
}
