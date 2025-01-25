using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModelsLibrary.IngredientRequests
{
    public class UpdateIngredientRequest
    {

        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Quantity { get; set; }
    }
}
