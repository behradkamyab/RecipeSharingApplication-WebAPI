using ModelsLibrary.Models;
using System.ComponentModel.DataAnnotations;


namespace SharedModelsLibrary.RecipeDTOs
{
    public class RecipeFullDetailsViewModel
    {
       
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public ICollection<IngredientRecipe>? Ingredients { get; set; }

        [Required]
        public ICollection<InstructionRecipe>? Instructions { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Cuisine { get; set; }
        [Required]
        public required ICollection<Like> Likes { get; set; }

        [Required]
        public required string CreatorUserId { get; set; }
    }
}
