using ServicesLibrary.Responses;

namespace ServicesLibrary.Delegates
{
    public delegate Task<RecipeManagerResponse<Dictionary<string, int>>> GetRecipeCountDelegate(List<string> userId);
}
