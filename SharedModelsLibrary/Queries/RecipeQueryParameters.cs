using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerLibrary.Queries
{
    public  class RecipeQueryParameters : PageNumberQueryParameters
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Cuisine { get; set; }
    }
}
