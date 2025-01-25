using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace ModelsLibrary.Models
{

    public class InstructionRecipe
    {
        [Required]
        public required Guid Id { get; set; } //PK


        [Required]
        [JsonIgnore]
        public required Guid RecipeId { get; set; } //FK => recipe

        [JsonIgnore]
        public RecipeModel Recipe { get; set; } // navigation property

        
        public required int Step { get; set; }

        [Required(ErrorMessage = "Please Provide the Content!")]
        public required string Content { get; set; }
    }
}
