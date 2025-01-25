
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;


namespace ModelsLibrary.Models
{
    /// <summary>
    /// User model with id, UserName, Email, PhoneNumber
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public Guid ProfileId { get; set; } //FK => profile
        public UserProfileModel Profile { get; set; } //Navigation property

        [JsonIgnore]
        public ICollection<UserFavoriteRecipe>? FavoriteRecipes { get; set; } //Navigation Property
        [JsonIgnore]
        public ICollection<RecipeModel> CreatedRecipe { get; set; } // navigation property for the recipes that user has created!
        [JsonIgnore]
        public ICollection<Follow> Followings { get; set; } // Navigation property for List of followings
        [JsonIgnore]
        public ICollection<Follow> Followers { get; set; } //Navigation property for List of followers
        public ApplicationUser(string userName) : base(userName)
        {

        }

        public ApplicationUser()
        {

        }
    }

    
}
