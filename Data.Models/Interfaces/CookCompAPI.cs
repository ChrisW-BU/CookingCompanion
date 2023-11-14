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
        // Recipe Interface Members
        ////////////////////////////
        Task<List<Favourite>?> GetFavouriteListAsync();

        Task<Favourite?> GetFavouriteUniqueAsync(int id);

        Task<Favourite?> CheckIsFavourite(int recipeId, int userId);

        Task<int> GetFavouriteCountAsync();

        Task<Favourite?> SaveFavouriteAsync(Favourite editedObj);

        Task DeleteFavouriteAsync(int id);



        ////////////////////////////
        // User Interface Members
        ////////////////////////////
        Task<List<User>?> GetUserListAsync();
        Task<User?> GetUser(string userName);


        /////////////////
        // Other Helpers
        /////////////////
        Task ClearCacheAsync();
        Task<List<MeasurementType?>>GetMeasurementTypeListAsync();

        Task<MeasurementType?> GetMeasurementTypeUniqueAsync(int id);
    }
}
