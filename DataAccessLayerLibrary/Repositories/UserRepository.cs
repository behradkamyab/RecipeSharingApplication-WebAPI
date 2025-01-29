using DataAccessLayerLibrary.DataPersistence;
using DataAccessLayerLibrary.Exceptions;
using DataAccessLayerLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using SharedModelsLibrary.RecipeDTOs;

namespace DataAccessLayerLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }

        }


        public async Task<bool> IsUserValidAsync(string userId)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Id == userId);

            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }


        public async Task<bool> IsEmailValidAsync(string email)
        {
            try
            {
                return !await _context.Users.AnyAsync(u => u.Email == email);

            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task CreateProfileAsync(UserProfileModel profile)
        {
            try
            {
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }

        }

        public async Task DeleteProfileAsync(Guid profileId)
        {

            try
            {
                await _context.UserProfiles.Where(p => p.Id == profileId).ExecuteDeleteAsync();

            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }


        public async Task UpdateProfileAsync(UserProfileModel profile)
        {
            try
            {

                _context.UserProfiles.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);
            }

        }

        public async Task<UserProfileModel?> GetProfileAsync(string userId) //check for user null in caller
        {
            try
            {
               return await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);

            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task AddRecipeToUserFavoritesAsync(UserFavoriteRecipe userFavoriteRecipe)
        {
            try
            {
                _context.UserFavoriteRecipes.Add(userFavoriteRecipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task RemoveRecipeFromUserFavoritesAsync(string userId, Guid recipeId)
        {
            try
            {
                await _context.UserFavoriteRecipes.Where(ufr => ufr.RecipeId == recipeId && ufr.UserId == userId).ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<IEnumerable<RecipeModel>?> GetAllFavoritesRecipesForUserAsync(string userId)
        {
            try
            {
                return await _context.UserFavoriteRecipes.Where(uf => uf.UserId == userId).Include(uf => uf.Recipe).Select(uf => uf.Recipe).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<RecipeModel?> GetUserFavoritesRecipeDetailsAsync(string userId, Guid recipeId)
        {
            try
            {
                return await _context.UserFavoriteRecipes
                                        .Where(ufr => ufr.UserId == userId && ufr.RecipeId == recipeId)
                                        .Include(ufr => ufr.Recipe)
                                             .ThenInclude(r => r.Ingredients)
                                        .Include(ufr => ufr.Recipe)
                                              .ThenInclude(r => r.Instructions)
                                        .Select(ufr => ufr.Recipe)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
        public async Task<bool> IsRecipeAlreadyInUserFavoritesListAsync(string userId, Guid recipeId)
        {
            try
            {
                return await _context.UserFavoriteRecipes.AnyAsync(ufr => ufr.UserId == userId && ufr.RecipeId == recipeId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task SaveFollowAsync(Follow follow)
        {
            try
            {
                await _context.Follows.AddAsync(follow);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task UnfollowAsync(string followerId , string followedId)
        {
            try
            {
               await _context.Follows.Where(f => f.FollowerId == followerId && f.FollowedId == followedId).ExecuteDeleteAsync();
                
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
        

        public async Task<IEnumerable<ApplicationUser>?> GetAllFollowersAsync(string userId)
        {
            try
            {
              return  await _context.Follows.Where(u => u.FollowedId == userId).Include(u => u.FollowerUsers).Select(u => u.FollowerUsers).ToListAsync();  
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<IEnumerable<ApplicationUser>?> GetAllFollowingsAsync(string userId)
        {
            try
            {
                return await _context.Follows.Where(f => f.FollowerId == userId).Include(u => u.FollowedUsers).Select(u => u.FollowedUsers).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<int> GetAllFollowersCountAsync(string userId)
        {
            try
            {
                return await _context.Follows.CountAsync(f => f.FollowedId == userId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }


        public async Task<Dictionary<string, int>> GetAllFollowersCountForOtherUsersAsync(List<string> userIds)
        {
            try
            {
                return await _context.Follows
                .Where(f => userIds.Contains(f.FollowedId))
                .GroupBy(f => f.FollowedId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

       
        public async Task<int> GetAllFollowingCountAsync(string userId)
        {
            try
            {
                return await _context.Follows.CountAsync(f => f.FollowerId == userId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
        public async Task<Dictionary<string, int>> GetAllFollowingsCountForOtherUsersAsync(List<string> userIds)
        {
            try
            {
                return await _context.Follows
                .Where(f => userIds.Contains(f.FollowerId))
                .GroupBy(f => f.FollowerId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<bool> IsFollowAvailableAsync(string followerId, string followedId)
        {
            try
            {
                return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
    }
}
