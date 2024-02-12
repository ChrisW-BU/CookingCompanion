using Data.Models;

namespace Data.Models.Interfaces
{
    public interface CookCompAPI
    {
        public enum LogActionType
        {
            Read,
            Save,
            Delete
        }

        public enum UserLogs
        {
            StartedRecipe,
            EndedRecipe,
            RestartedRecipe,
            StartedTimer,
            EndedTimer,
            AddFavourite,
            RemovedFavourite,
            AddedShoppingList,
            RemovedShoppingList,
            Error
        }

        ////////////////////////////////
        // IIngredient
        ////////////////////////////////
        Task<List<Ingredient>?> GetIngredientListAsync(int ingredientCount, int index);
        Task<Ingredient?> GetIngredientUniqueAsync(int id);
        Task<int> GetIngredientCountAsync();
        Task<Ingredient?> SaveIngredientAsync(Ingredient editedObj);
        Task DeleteIngredientAsync(int id);



        ////////////////////////////
        // IRecipe
        ////////////////////////////
        Task<List<Recipe>?> GetRecipeListAsync(int recipeCount, int index);
        Task<Recipe?> GetRecipeUniqueAsync(int id);
        Task<int> GetRecipeCountAsync();
        Task<Recipe?> SaveRecipeAsync(Recipe editedObj);
        Task DeleteRecipeAsync(int id);
        Task<List<Recipe>?> GetFavouriteRecipes(int userId);



        ////////////////////////////
        // IRecipeIngredient
        ////////////////////////////
        Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeIngCount, int index);
        Task<RecipeIngredient?> GetRecipeIngredientUniqueAsync(int id);
        Task<int> GetRecipeIngredientCountAsync();
        Task<RecipeIngredient?> SaveRecipeIngredientAsync(RecipeIngredient editedObj);
        Task DeleteRecipeIngredientAsync(int id);
        Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeId);



        ////////////////////////////
        // IRecipeStep
        ////////////////////////////
        Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeStepCount, int index);
        Task<RecipeStep?> GetRecipeStepUniqueAsync(int id);
        Task<int> GetRecipeStepCountAsync();
        Task<RecipeStep?> SaveRecipeStepAsync(RecipeStep editedObj);
        Task DeleteRecipeStepAsync(int id);
        Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeId);



        ////////////////////////////
        // IFavourite
        ////////////////////////////
        Task<List<Favourite>?> GetFavouriteListAsync();
        Task<Favourite?> GetFavouriteUniqueAsync(int id);
        Task<Favourite?> CheckIsFavourite(int recipeId, int userId);
        Task<int> GetFavouriteCountAsync();
        Task<Favourite?> SaveFavouriteAsync(Favourite editedObj);
        Task DeleteFavouriteAsync(int id);



        ////////////////////////////
        // IShoppingList
        ////////////////////////////
        Task<List<ShoppingList>?> GetShoppingListAsync(int userId);
        Task<ShoppingList?> GetShoppingUniqueAsync(int id);
        Task<ShoppingList?> CheckIsPinned(int recipeId, int userId);
        Task<int> GetShoppingCountAsync();
        Task<ShoppingList?> SaveShoppingListAsync(ShoppingList editedObj);
        Task DeleteShoppingListAsync(int id);
        Task ClearShoppingList(int listId);
        Task<bool> IsShoppingListLimit(int userId);


        ////////////////////////////
        // IShoppingListItem
        ////////////////////////////
        Task<List<ShoppingListItem>?> GetShoppingItemListAsync(int listId);
        Task<ShoppingListItem?> GetShoppingItemUniqueAsync(int id);
        Task<int> GetShoppingItemCountAsync();
        Task<ShoppingListItem?> SaveShoppingItemListAsync(ShoppingListItem editedObj);
        Task DeleteShoppingItemAsync(int id);
        Task CheckListHasItems(int listId, int recipeId);



        ////////////////////////////////
        // ICooking
        ////////////////////////////////
        Task<List<Cooking>?> GetCookingListAsync(int cookingCount, int index);
        Task<Cooking?> GetCookingUniqueAsync(int id);
        Task<int> GetCookingCountAsync();
        Task<Cooking?> SaveCookingAsync(Cooking editedObj);
        Task DeleteCookingAsync(int id);
        Task<bool> CheckRecipeStarted(int recipeId, int userId);
        Task<List<CookingStep>?> CheckCookingStatus(int recipeId, int userId);
        Task RestartCookingRecipe(int recipeId, int userId);



        ////////////////////////////////
        // ICookingStep
        ////////////////////////////////
        Task<List<CookingStep>?> GetCookingStepListAsync(int cookingId);
        Task<CookingStep?> GetCookingStepUniqueAsync(int id);
        Task<int> GetCookingStepCountAsync();
        Task<CookingStep?> SaveCookingStepAsync(CookingStep editedObj);
        Task DeleteCookingStepAsync(int id);



        ////////////////////////////
        // IUser
        ////////////////////////////
        Task<List<User>?> GetUserListAsync();
        Task<User?> GetUser(string userName, bool isGuidCheck);
        Task<User?> GetUser(int userId);
        Task<User?> GetUser(Guid userId);
        Task<User?> SaveUserAsync(User editedObj);



        ////////////////////////////
        // ITaskObj
        ////////////////////////////
        Task<List<TaskObj>?> GetTaskListAsync();
        Task<bool> CheckTaskStatus(int taskId, int userId);
        Task<TaskObj?> GetActiveTask(int taskId, int userId);
        Task<TaskObj?> SaveTaskAsync(TaskObj editedObj);



        ////////////////////////////
        // ILogEntry
        ////////////////////////////
        Task<List<LogEntry>?> GetLogEntryListAsync(int userId, string entityType);
        Task SaveManualLogEntry(int userId, int entityId, UserLogs ul, string entityType);
        Task<List<LogEntry>?> GetManualLogEntryList();



        /////////////////
        // Other Helpers
        /////////////////
        Task ClearCacheAsync();
        Task<List<MeasurementType>?>GetMeasurementTypeListAsync();
        Task<MeasurementType?> GetMeasurementTypeUniqueAsync(int id);

        void Output(string s);
    }
}
