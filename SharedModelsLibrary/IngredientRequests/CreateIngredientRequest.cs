using System.ComponentModel.DataAnnotations;


namespace SharedModelsLibrary.IngredientRequests
{
    public class CreateIngredientRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required int Quantity { get; set; }
    }
}
