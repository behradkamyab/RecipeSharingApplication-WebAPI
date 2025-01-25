using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ModelsLibrary.Models
{
    public class IngredientRecipe
    {
        [Required]
        public required Guid Id { get; set; } //PK

        [Required]
        [JsonIgnore]
        public required Guid RecipeId { get; set; } //FK => recipe


        [JsonIgnore]
        public RecipeModel Recipe { get; set; } // Navigation property

        [Required(ErrorMessage = "Please Provide the Name!")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Please Provide the Quantity!")]
        public required int Quantity { get; set; }
    }
}
