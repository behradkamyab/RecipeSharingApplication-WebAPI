
using System.ComponentModel.DataAnnotations;
using ModelsLibrary.Models;


namespace SharedModelsLibrary.RecipeDto
{
    public class RecipeViewModel
    {
        [Required]
        public required Guid RecipeId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Cuisine { get; set; }
    }
}
