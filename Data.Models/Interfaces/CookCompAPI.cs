using RPCC.Data.Models;

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
        

        /////////////////
        // Other Helpers
        /////////////////
        Task ClearCacheAsync();
    }
}
