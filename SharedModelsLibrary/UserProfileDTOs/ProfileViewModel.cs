using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModelsLibrary.UserProfileDTOs
{
    public class ProfileViewModel
    {
       
        [Required]
        public string Email { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public int Followings { get; set; }
        [Required]
        public int Followers { get; set; }
        [Required]
        public int CreatedRecipes { get; set; }
    }
}
