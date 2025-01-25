using System.ComponentModel.DataAnnotations;

namespace SharedModelsLibrary.UserDTOs
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
