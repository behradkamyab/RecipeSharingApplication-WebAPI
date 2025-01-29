using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ModelsLibrary.Models
{
    public class IngredientRecipe
    {

        public IngredientRecipe(Guid id,Guid recipeId , string name , int quantity)
        {
            Id = id;
            RecipeId = recipeId;
            Name = name;
            Quantity = quantity;
        }


        public  Guid Id { get; set; } //PK

        [JsonIgnore]
        public  Guid RecipeId { get; set; } //FK => recipe


        [JsonIgnore]
        public RecipeModel Recipe { get; set; } // Navigation property

        [Required(ErrorMessage = "Please Provide the Name!")]
        public  string Name { get; set; }
        [Required(ErrorMessage = "Please Provide the Quantity!")]
        public  int Quantity { get; set; }
    }
}
