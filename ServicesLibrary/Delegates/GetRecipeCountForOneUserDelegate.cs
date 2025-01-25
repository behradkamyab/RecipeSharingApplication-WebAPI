using ServicesLibrary.Responses;

namespace ServicesLibrary.Delegates
{
    public delegate Task<RecipeManagerResponse<int>> GetRecipeCountForOneUserDelegate( string userId);
}
