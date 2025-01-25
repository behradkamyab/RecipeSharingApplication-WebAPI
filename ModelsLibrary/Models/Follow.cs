using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class Follow
    {
        [Required]
        public required string FollowerId { get; set; } // A person who follows another person
        [JsonIgnore]
        public ApplicationUser FollowerUsers { get; set; }
        [Required]
        public required string  FollowedId { get; set; } // A person who followed by another person
        [JsonIgnore]
        public ApplicationUser FollowedUsers { get; set; }
        public required DateTime CreatedAt { get; set; }


    }
}
