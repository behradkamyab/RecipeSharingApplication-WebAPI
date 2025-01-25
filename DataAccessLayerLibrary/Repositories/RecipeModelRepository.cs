using DataAccessLayerLibrary.DataPersistence;
using DataAccessLayerLibrary.Exceptions;
using DataAccessLayerLibrary.Interfaces;
using DataAccessLayerLibrary.Queries;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using SharedModelsLibrary.Extensions;
using SharedModelsLibrary.RecipeDto;
using System.Linq.Expressions;

namespace DataAccessLayerLibrary.Repositories
{
    public class RecipeModelRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRecipeAsync(RecipeModel entity)
        {
            try
            {
                await _context.Recipes.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task RemoveRecipeAsync(Guid recipeId)
        {
            try
            {
               await _context.Recipes.Where(r => r.Id == recipeId).ExecuteDeleteAsync();
               
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException">happen if something</exception>
        public async Task UpdateRecipeAsync(RecipeModel model)
        {
            try
            {
                _context.Recipes.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            
        }

        /// <summary>
        /// Find the recipe Async that satisfy the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Task of the Recipe or null</returns>
        /// <exception cref="ArgumentNullException">If the predicate is null it will throw the null exception<</exception>
        public async Task<IEnumerable<RecipeModel>> FindRecipeAsync(Expression<Func<RecipeModel, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                throw new ArgumentNullException();

                return await _context.Recipes.Where(predicate).ToListAsync();
                
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task<RecipeModel?> GetRecipeAsync(Guid id)
        {
            return await _context.Recipes.FindAsync(id);
        }


        public async Task<RecipeModel?> GetFullDetailsRecipeAsync(Guid id)
        {
            try
            {
                return await _context.Recipes.Where(r => r.Id == id).Include(r => r.Ingredients).Include(r => r.Instructions).Include(r => r.Likes).FirstOrDefaultAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }


        }
        public async Task<IEnumerable<RecipeModel>?> GetAllRecipesRandomlyAsync(PageNumberQueryParameters query)
        {
            try
            {
                return await _context.Recipes.FromSqlRaw("SELECT * FROM public.\"Recipes\" ORDER BY RANDOM()").Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task<IEnumerable<RecipeModel>?> GetAllFilterRecipesAsync(RecipeQueryParameters query)
        {
            try
            {
                return await _context.Recipes.ApplyFilter(query).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
                
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
           
 
        }


        public async Task<IEnumerable<RecipeModel>?> GetAllRecipesCreatedByUser(string userId)
        {
            try
            {
                return await _context.Recipes.Where(r => r.UserId == userId).ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }
       public async Task<Dictionary<string, int>> GetAllNumbersOfRecipesCreatedByUserForOtherUsersAsync(List<string> userIds)
       {
            try
            {
                // Initialize the dictionary with default values (0)
                var recipeCounts = userIds.ToDictionary(id => id, _ => 0);

                // Fetch the number of recipes created by each user in the list
                var counts = await _context.Recipes
                    .Where(r => userIds.Contains(r.UserId)) // Filter by the list of user IDs
                    .GroupBy(r => r.UserId) // Group by user ID
                    .Select(g => new
                    {
                        UserId = g.Key,
                        RecipeCount = g.Count() // Count the number of recipes for each user
                    })
                    .ToListAsync();

                // Update the dictionary with the fetched counts
                foreach (var count in counts)
                {
                    recipeCounts[count.UserId] = count.RecipeCount;
                }

                return recipeCounts;
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task<int> GetAllNumbersOfRecipesCreatedByUserAsync(string userId)
        {
            try
            {
                return await _context.Recipes.CountAsync(r => r.UserId == userId);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task AddIngredientAsync(IngredientRecipe ingredientRecipe)
        {
                
            try
            {
               await _context.Ingredients.AddAsync(ingredientRecipe);
                await _context.SaveChangesAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            

        }


        public async Task RemoveIngredientAsync(Guid ingredientRecipeId)
        {
            try
            {
                await _context.Ingredients.Where(i => i.Id == ingredientRecipeId).ExecuteDeleteAsync();
            }  
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task UpdateIngredientAsync(IngredientRecipe ingredientRecipe)
        {
            try
            {
               _context.Ingredients.Update(ingredientRecipe);
                await _context.SaveChangesAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }


        public async Task<IngredientRecipe?> GetIngredientByIdAsync(Guid ingredientId)
        {
            return await _context.Ingredients.FindAsync(ingredientId);

        }

       public async Task<bool> IsIngredientAvailable(Guid ingredientId, Guid recipeId)
        {
            try
            {
                return await _context.Ingredients.AnyAsync(i => i.Id == ingredientId && i.RecipeId == recipeId);
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }



       public async Task<IEnumerable<IngredientRecipe>?> GetAllIgredientsOfRecipe(Guid recipeId)
        {
            try
            {
                return await _context.Ingredients.Where(i => i.RecipeId == recipeId).ToListAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task AddInstructionAsync(InstructionRecipe instructionRecipe)
        {
            try
            {
                await _context.Instructions.AddAsync(instructionRecipe);
                await _context.SaveChangesAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }





        public async Task RemoveInstructionAsync(Guid instructionRecipeId)
        {
            try
            {                
                await _context.Ingredients.Where(i => i.Id == instructionRecipeId).ExecuteDeleteAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task UpdateInstructionAsync(InstructionRecipe instructionRecipe)
        {
            try
            {
                _context.Instructions.Update(instructionRecipe);
                await _context.SaveChangesAsync();
            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }
        

        public async Task<IEnumerable<InstructionRecipe>?> FindInstructionAsync(Expression<Func<InstructionRecipe, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                    throw new ArgumentNullException();

                return await _context.Instructions.Where(predicate).ToListAsync();

            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task<int> GetLastInstructionStep(Guid recipeId)
        {
            try
            {
                var ins =  await _context.Instructions.Where(i =>i.RecipeId == recipeId).OrderBy(i => i.Step).LastOrDefaultAsync();
                if(ins == null)
                {
                    return 0;
                }
                else
                {
                    return ins.Step;
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }

        }

        public async Task<InstructionRecipe?> GetInstructionByIdAsync(Guid instructionId)
        {
            try
            {
                return await _context.Instructions.FirstOrDefaultAsync(i => i.Id == instructionId);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            
        }



        public async Task AddLikeToRecipeAsync(Like like)
        {
            try
            {
                await _context.Likes.AddAsync(like);
                await _context.SaveChangesAsync();

            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }


        public async Task RemoveLikeFromRecipeAsync(Like like)
        {
            try
            {
                 _context.Likes.Remove(like);
                await _context.SaveChangesAsync();

            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }
        public async Task<Like?> GetLikeForRecipeAsync(string userId, Guid recipeId)
        {
            try
            {
                return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.RecipeId == recipeId);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }

        public async Task<int> GetAllLikesForRecipeAsync(Guid recipeId)
        {
            try
            {
               return await _context.Likes.CountAsync(l => l.RecipeId == recipeId);

            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }


        public async Task<bool> IsRecipeOwnedByUserASync(string userId, Guid recipeId)
        {
            try
            {
                var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
                return recipe != null && recipe.UserId == userId;
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);


            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }

        }

        public async Task<bool> IsRecipeAvailable(Guid recipeId)
        {
            try
            {
                return await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataAccessException("Arguments cannot be null", ex);

            }
            catch (OperationCanceledException ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }
        }
    }
}
