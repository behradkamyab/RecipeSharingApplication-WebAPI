using ModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModelsLibrary.RecipeRequests
{
    public class UpdateRecipeRequest
    {

        [MinLength(2)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<IngredientRecipe>? Ingredients { get; set; } = new List<IngredientRecipe>();

        public IEnumerable<InstructionRecipe>? Instructions { get; set; } = new List<InstructionRecipe>();


        public string? Category { get; set; }

        public string? Cuisine { get; set; }
    }
}
