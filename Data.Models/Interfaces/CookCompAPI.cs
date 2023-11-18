﻿using Data.Models;

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

        Task<List<Recipe>?> GetFavouriteRecipes(int userId);



        ////////////////////////////
        // Recipe Ingredient Interface Members
        ////////////////////////////
        Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeIngCount, int index);

        Task<RecipeIngredient?> GetRecipeIngredientUniqueAsync(int id);

        Task<int> GetRecipeIngredientCountAsync();

        Task<RecipeIngredient?> SaveRecipeIngredientAsync(RecipeIngredient editedObj);

        Task DeleteRecipeIngredientAsync(int id);

        Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeId);



        ////////////////////////////
        // Recipe Steps Interface Members
        ////////////////////////////
        Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeStepCount, int index);

        Task<RecipeStep?> GetRecipeStepUniqueAsync(int id);

        Task<int> GetRecipeStepCountAsync();

        Task<RecipeStep?> SaveRecipeStepAsync(RecipeStep editedObj);

        Task DeleteRecipeStepAsync(int id);

        Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeId);



        ////////////////////////////
        // Favourites Interface Members
        ////////////////////////////
        Task<List<Favourite>?> GetFavouriteListAsync();

        Task<Favourite?> GetFavouriteUniqueAsync(int id);

        Task<Favourite?> CheckIsFavourite(int recipeId, int userId);

        Task<int> GetFavouriteCountAsync();

        Task<Favourite?> SaveFavouriteAsync(Favourite editedObj);

        Task DeleteFavouriteAsync(int id);



        ////////////////////////////
        // Shopping Interface Members
        ////////////////////////////
        Task<List<ShoppingList>?> GetShoppingListAsync(int userId);

        Task<ShoppingList?> GetShoppingUniqueAsync(int id);

        Task<ShoppingList?> CheckIsPinned(int recipeId, int userId);

        Task<int> GetShoppingCountAsync();

        Task<ShoppingList?> SaveShoppingListAsync(ShoppingList editedObj);

        Task DeleteShoppingListAsync(int id);



        ////////////////////////////
        // ShoppingListItem Interface Members
        ////////////////////////////
        Task<List<ShoppingListItem>?> GetShoppingItemListAsync(int listId);

        Task<ShoppingListItem?> GetShoppingItemUniqueAsync(int id);

        Task<int> GetShoppingItemCountAsync();

        Task<ShoppingListItem?> SaveShoppingItemListAsync(ShoppingListItem editedObj);

        Task DeleteShoppingItemAsync(int id);

        Task CheckListHasItems(int listId, int recipeId);



        ////////////////////////////
        // User Interface Members
        ////////////////////////////
        Task<List<User>?> GetUserListAsync();
        Task<User?> GetUser(string userName);

        Task<User?> GetUser(int userId);


        /////////////////
        // Other Helpers
        /////////////////
        Task ClearCacheAsync();
        Task<List<MeasurementType?>>GetMeasurementTypeListAsync();

        Task<MeasurementType?> GetMeasurementTypeUniqueAsync(int id);
    }
}
