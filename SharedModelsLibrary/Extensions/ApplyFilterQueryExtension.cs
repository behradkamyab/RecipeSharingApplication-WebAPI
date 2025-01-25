
using DataAccessLayerLibrary.Queries;
using ModelsLibrary.Models;
using SharedModelsLibrary.RecipeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModelsLibrary.Extensions
{
    public static class ApplyFilterQueryExtension
    {
        public static IQueryable<RecipeModel> ApplyFilter(this IQueryable<RecipeModel> query, RecipeQueryParameters filter)
        {


            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(r => r.Name.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Category))
                query = query.Where(r => r.Category == filter.Category);

            if (!string.IsNullOrWhiteSpace(filter.Cuisine))
                query = query.Where(r => r.Cuisine == filter.Cuisine);

            query = query.OrderBy(r => r.Id);

            return query;
        }
    }
}
