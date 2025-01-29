using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class UserProfileModel
    {
        [Required]
        public Guid Id { get; set; } //PK

        [Required]
        public string  Bio { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        

        [Required]
        public string UserId { get; set; } //FK => user
        public ApplicationUser User { get; set; } // navigation property

        public UserProfileModel( Guid id,string bio , string email , string userId)
        {
            Id = id;
            Bio = bio;
            Email = email;
            UserId = userId;   
        }
    }
}
