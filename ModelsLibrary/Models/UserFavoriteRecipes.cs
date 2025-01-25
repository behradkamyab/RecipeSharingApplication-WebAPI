
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelsLibrary.Models
{
    public class UserFavoriteRecipe
    {
        //Composite key => {userId & recipeId}

        [Required]
        public required string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; } // Navigation property

        [Required]
        public required Guid RecipeId { get; set; }
        [JsonIgnore]
        public RecipeModel Recipe { get; set; } // navigation property
    }
}
