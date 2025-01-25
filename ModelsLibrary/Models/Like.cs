using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class Like
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required Guid RecipeId { get; set; }

        public required DateTime LikedAt { get; set; }
    }
}
