using Data.Models;

namespace Data.Models.Interfaces
{
    public interface CookCompAPI
    {
        ////////////////////////////////
        // Ingredient Interface Members
        ////////////////////////////////
        Task<List<Ingredient>?> GetIngredientListAsync(int ingredientCount, int index);

        Task<Ingredient?> GetIngredientUniqueAsync(int id);

        Task<int> GetIngredientCountAsync();

        Task<Ingredient?> SaveIngredientAsync(Ingredient editedObj);

        Task DeleteIngredientAsync(int id);



        ////////////////////////////
        // Recipe Interface Members
        ////////////////////////////
        Task<List<Recipe>?> GetRecipeListAsync(int recipeCount, int index);

        Task<Recipe?> GetRecipeUniqueAsync(int id);

        Task<int> GetRecipeCountAsync();

        Task<Recipe?> SaveRecipeAsync(Recipe editedObj);

        Task DeleteRecipeAsync(int id);



        /////////////////
        // Other Helpers
        /////////////////
        Task ClearCacheAsync();
    }
}
