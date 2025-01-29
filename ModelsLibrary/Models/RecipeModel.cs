using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelsLibrary.Models
{
    public class RecipeModel
    {
      
        [Required]
        public Guid Id { get; set; } //PK


        [Required(ErrorMessage ="Please Provide the name!")]
        [MinLength(2)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please Provide the Description!")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please Provide the category!")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Please Provide the cuisine!")]
        public string Cuisine { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; }


        [Required]
        public string UserId { get; set; } //FK => user


        [JsonIgnore]
        public ApplicationUser User { get; set; } //Navigation property
        //public ICollection<UserFavoriteRecipe>? FavoritesByUsers { get; set; } //Navigation property for users who favorited this recipe
        public ICollection<IngredientRecipe>? Ingredients { get; set; } //Navigation property
        public ICollection<InstructionRecipe>? Instructions { get; set; } //Navigation property
        [JsonIgnore]
        public ICollection<Like>? Likes { get; set; }

        public RecipeModel() { }

        public RecipeModel(Guid id ,string name, string description, string category, string cuisine, string userid)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Cuisine = cuisine;
            UserId = userid;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
