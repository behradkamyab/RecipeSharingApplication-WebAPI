using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;


namespace ModelsLibrary.Models
{

    public class InstructionRecipe
    {
        public InstructionRecipe(Guid id, Guid recipeId, int step, string content)
        {
            Id = id;
            RecipeId = recipeId;
            Step = step;
            Content = content;
        }
        [Required]
        public  Guid Id { get; set; } //PK


        [JsonIgnore]
        public  Guid RecipeId { get; set; } //FK => recipe

        [JsonIgnore]
        public RecipeModel Recipe { get; set; } // navigation property

        
        public  int Step { get; set; }

        [Required(ErrorMessage = "Please Provide the Content!")]
        public  string Content { get; set; }
    }
}
