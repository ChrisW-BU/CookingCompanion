﻿using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Data.Models;
using System.Xml.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Data
{

    public class CookCompApiJsonAccess : CookCompAPI
    {
        CookCompApiJsonAccessSetting _settings;
        private List<Recipe>? _recipes;
        private List<RecipeStep>? _recipe_steps;
        private List<RecipeIngredient>? _recipe_ingredients;
        private List<Ingredient>? _ingredients;
        private List<User>? _users;
        private List<ShoppingList>? _shopping_list;
        private List<ShoppingListItem>? _shopping_list_item;
        private List<Favourite>? _favourites;
        private List<MeasurementType>? _measurements;
        private List<Cooking>? _cooking;
        private List<CookingStep>? _cooking_steps;
        private List<LogEntry>? _logs;
        private List<TaskObj>? _tasks;
        private List<QuestionnaireObj>? _questions;
        private List<ConsentObj>? _consent_forms;

        /// <summary>
        /// This allows the injection of IOptions and creates a settings structure for the data folders.
        /// </summary>
        /// <param name="option"></param>
        public CookCompApiJsonAccess(IOptions<CookCompApiJsonAccessSetting> option)
        {
            // Check if we have the correct directories, and create them if not
            _settings = option.Value;

            if (!Directory.Exists(_settings.DataRootPath))
            {
                Directory.CreateDirectory(_settings.DataRootPath);
            }
            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.RecipeFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.RecipeFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.RecipeStepFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.RecipeStepFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.RecipeIngredientFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.RecipeIngredientFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.IngredientFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.IngredientFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.UserFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.UserFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.ShoppingListItemFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.ShoppingListItemFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.ShoppingListFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.ShoppingListFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.FavouriteFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.FavouriteFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.MeasurementTypeFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.MeasurementTypeFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.CookingFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.CookingFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.CookingStepFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.CookingStepFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.LogEntryFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.LogEntryFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.TasksFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.TasksFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.QuestionnaireFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.QuestionnaireFolder}");
            }

            if (!Directory.Exists($@"{_settings.DataRootPath}\{_settings.ConsentObjectFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataRootPath}\{_settings.ConsentObjectFolder}");
            }
        }

        //////////////////////////
        // Generic Helper Methods
        //////////////////////////

        /// <summary>
        /// Generic Load Function. Allows the passing of any entity to load JSON files.
        /// It will only load data when none is present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="folder"></param>
        private void Load<T>(ref List<T>? list, string folder)
        {
            if (list == null)
            {
                // Initialise a new instance of the provided class
                list = new();

                // Retrieve the full path for this entity
                var path = $@"{_settings.DataRootPath}\{folder}";

                // Deserialise any JSONs located in this path and add to the list
                foreach (var f in Directory.GetFiles(path))
                {
                    var getJson = File.ReadAllText(f);
                    var convertedObject = JsonSerializer.Deserialize<T>(getJson);
                    if (convertedObject != null)
                    {
                        list.Add(convertedObject);
                    }
                }
            }
        }

        /// <summary>
        /// Generic method that allows Saving of any entity type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task SaveAsync<T>(List<T>? list, string folder, string filename, T item)
        {
            // Retrieve the file path
            var path = $@"{_settings.DataRootPath}\{folder}\{filename}";

            // Create new file/modify existing - and save.
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize<T>(item));

            if (list == null)
            {
                list = new();
            }

            // Add our new/modified item to existing list
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Generic method that allows the Deletion of any entity type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="folder"></param>
        /// <param name="id"></param>
        private void DeleteAsync<T>(List<T>? list, string folder, int id)
        {
            // Find our file path
            var path = $@"{_settings.DataRootPath}\{folder}\{id}.json";
            try
            {
                // Delete if this file exists
                File.Delete(path);
            }
            catch { }
        }

        public Task RefreshRepository()
        {
            ClearCacheAsync();

            int recipeCount = GetRecipeCountAsync().Result;
            GetRecipeListAsync(recipeCount, 0);

            int ingredientCount = GetIngredientCountAsync().Result;
            GetIngredientListAsync(ingredientCount, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Clear all JSON list data.
        /// </summary>
        /// <returns></returns>
        public Task ClearCacheAsync()
        {
            _recipes = null;
            _recipe_steps = null;
            _recipe_ingredients = null;
            _ingredients = null;
            _shopping_list = null;
            _shopping_list_item = null;
            _favourites = null;
            _measurements = null;
            //_users = null;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Load measurement types JSON data.
        /// </summary>
        /// <returns></returns>
        private Task LoadMeasurementTypesAsync()
        {
            Load<MeasurementType>(ref _measurements, _settings.MeasurementTypeFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Return a list of measurement types.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MeasurementType>?> GetMeasurementTypeListAsync()
        {
            await LoadMeasurementTypesAsync();
            return _measurements ?? new();
        }

        /// <summary>
        /// Return a single unique measurement type using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<MeasurementType?> GetMeasurementTypeUniqueAsync(int id)
        {
            await LoadMeasurementTypesAsync();
            if (_measurements == null)
            {
                throw new Exception("No measurements have been found");
            }
            return _measurements.FirstOrDefault(b => b.Id == id);
        }



        /////////////////////
        // Ingredients
        /////////////////////

        /// <summary>
        /// Load a list of all ingredients.
        /// </summary>
        /// <returns></returns>
        private Task LoadIngredientAsync()
        {
            Load<Ingredient>(ref _ingredients, _settings.IngredientFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of ingredients and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<Ingredient>?> GetIngredientListAsync(int ingredientCount, int index)
        {
            await LoadIngredientAsync();
            return _ingredients ?? new();
        }

        /// <summary>
        /// Load a single unique ingredient and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Ingredient?> GetIngredientUniqueAsync(int id)
        {
            await LoadIngredientAsync();
            if (_ingredients == null)
            {
                throw new Exception("No ingredients have been found");
            }
            return _ingredients.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all ingredients.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetIngredientCountAsync()
        {
            await LoadIngredientAsync();
            if (_ingredients == null)
            {
                return 0;
            }
            else
            {
                return _ingredients.Count();
            }
        }

        /// <summary>
        /// Save changes made to an ingredient object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<Ingredient?> SaveIngredientAsync(Ingredient editedObj)
        {
            if (_ingredients == null)
            {
                LoadIngredientAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_ingredients.Count > 0)
                {
                    editedObj.Id = ((int)_ingredients.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<Ingredient>(_ingredients, _settings.IngredientFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete an ingredient using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteIngredientAsync(int id)
        {
            DeleteAsync(_ingredients, _settings.IngredientFolder, id);
            if (_ingredients != null)
            {
                var editedObj = _ingredients.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _ingredients.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }




        /////////////////////
        // Recipes
        /////////////////////

        /// <summary>
        /// Load a list of all recipes.
        /// </summary>
        /// <returns></returns>
        private Task LoadRecipesAsync()
        {
            Load<Recipe>(ref _recipes, _settings.RecipeFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of recipes and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<Recipe>?> GetRecipeListAsync(int recipeCount, int index)
        {
            await LoadRecipesAsync();
            return _recipes ?? new();
        }

        /// <summary>
        /// Load a single unique recipe and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Recipe?> GetRecipeUniqueAsync(int id)
        {
            await LoadRecipesAsync();
            if (_recipes == null)
            {
                throw new Exception("No recipes have been found");
            }
            return _recipes.FirstOrDefault(b => b.Id == id);
        }

        public async Task<List<Recipe>?> GetRecipeBySearchAsync(string s)
        {
            await LoadRecipesAsync();
            if(_recipes == null)
            {
                throw new Exception("No recipes have been found");
            }

            List<Recipe> returnCollection = new();
            foreach (Recipe recipe in _recipes)
            {
                if (recipe.Name.ToLower().Contains(s.ToLower()))
                {
                    returnCollection.Add(recipe);
                }
            }
            return returnCollection;
        }

        /// <summary>
        /// Return the count of all ingredients.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecipeCountAsync()
        {
            await LoadRecipesAsync();
            if (_recipes == null)
                return 0;
            else
                return _recipes.Count();
        }

        /// <summary>
        /// Save changes made to a recipe object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<Recipe?> SaveRecipeAsync(Recipe editedObj)
        {
            if (_recipes == null)
            {
                LoadRecipesAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_recipes.Count > 0)
                {
                    editedObj.Id = ((int)_recipes.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<Recipe>(_recipes, _settings.RecipeFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete a recipe using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteRecipeAsync(int id)
        {
            DeleteAsync(_recipes, _settings.RecipeFolder, id);
            if (_recipes != null)
            {
                var editedObj = _recipes.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _recipes.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of favourites recipes for stated user and return the list.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Recipe>?> GetFavouriteRecipes(int userId)
        {
            await LoadRecipesAsync();
            await LoadFavouritesAsync();

            List<Recipe> returnList = new();
            List<Favourite> favList = await GetFavouriteListAsync();

            if (favList != null && favList.Count > 0)
            {
                foreach (Favourite f in favList)
                {
                    if (f.UserId == userId)
                    {
                        Recipe r = await GetRecipeUniqueAsync(f.RecipeId);

                        if (r != null && r.Id > 0)
                        {
                            returnList.Add(r);
                        }
                    }
                }
            }
            return returnList ?? new();
        }



        /////////////////////
        // Recipe Ingredients
        /////////////////////

        /// <summary>
        /// Load a list of all recipe ingredients.
        /// </summary>
        /// <returns></returns>
        private Task LoadRecipeIngredientsAsync()
        {
            Load<RecipeIngredient>(ref _recipe_ingredients, _settings.RecipeIngredientFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of recipe ingredients and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeIngCount, int index)
        {
            await LoadRecipeIngredientsAsync();
            return _recipe_ingredients ?? new();
        }

        /// <summary>
        /// Load a single unique recipe ingredient and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RecipeIngredient?> GetRecipeIngredientUniqueAsync(int id)
        {
            await LoadRecipeIngredientsAsync();
            if (_recipe_ingredients == null)
            {
                throw new Exception("No recipe ingredients have been found");
            }
            return _recipe_ingredients.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all ingredients.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecipeIngredientCountAsync()
        {
            await LoadRecipeIngredientsAsync();
            if (_recipe_ingredients == null)
                return 0;
            else
                return _recipe_ingredients.Count();
        }

        /// <summary>
        /// Save changes made to a recipe ingredient object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<RecipeIngredient?> SaveRecipeIngredientAsync(RecipeIngredient editedObj)
        {
            if (_recipe_ingredients == null)
            {
                LoadRecipeIngredientsAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_recipe_ingredients.Count > 0)
                {
                    editedObj.Id = ((int)_recipe_ingredients.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<RecipeIngredient>(_recipe_ingredients, _settings.RecipeIngredientFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete a recipe ingredient using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteRecipeIngredientAsync(int id)
        {
            DeleteAsync(_recipe_ingredients, _settings.RecipeIngredientFolder, id);
            if (_recipe_ingredients != null)
            {
                var editedObj = _recipe_ingredients.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _recipe_ingredients.Remove(editedObj);
                }

            }
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of recipe ingredients with the specified recipeId and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<RecipeIngredient>?> GetRecipeIngredientListAsync(int recipeId)
        {
            await LoadRecipeIngredientsAsync();

            return _recipe_ingredients.Where(b => b.RecipeId == recipeId).ToList() ?? new();

            //return _recipe_ingredients ?? new();
        }


        /////////////////////
        // Recipe Steps
        /////////////////////

        /// <summary>
        /// Load a list of all recipe steps.
        /// </summary>
        /// <returns></returns>
        private Task LoadRecipeStepsAsync()
        {
            Load<RecipeStep>(ref _recipe_steps, _settings.RecipeStepFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of recipe steps and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeStepCount, int index)
        {
            await LoadRecipeStepsAsync();
            return _recipe_steps ?? new();
        }

        /// <summary>
        /// Load a single unique recipe step and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RecipeStep?> GetRecipeStepUniqueAsync(int id)
        {
            await LoadRecipeStepsAsync();
            if (_recipe_steps == null)
            {
                throw new Exception("No recipe steps have been found");
            }
            return _recipe_steps.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all recipe steps.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecipeStepCountAsync()
        {
            await LoadRecipeStepsAsync();
            if (_recipe_steps == null)
                return 0;
            else
                return _recipe_steps.Count();
        }

        /// <summary>
        /// Save changes made to a recipe step object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<RecipeStep?> SaveRecipeStepAsync(RecipeStep editedObj)
        {
            if (_recipe_steps == null)
            {
                LoadRecipeStepsAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_recipe_steps.Count > 0)
                {
                    editedObj.Id = ((int)_recipe_steps.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<RecipeStep>(_recipe_steps, _settings.RecipeStepFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete a recipe ingredient using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteRecipeStepAsync(int id)
        {
            DeleteAsync(_recipe_steps, _settings.RecipeStepFolder, id);
            if (_recipe_steps != null)
            {
                var editedObj = _recipe_steps.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _recipe_steps.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of recipe steps with the specified recipeId and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<RecipeStep>?> GetRecipeStepListAsync(int recipeId)
        {
            await LoadRecipeStepsAsync();

            return _recipe_steps.Where(b => b.RecipeId == recipeId).ToList() ?? new();

            //return _recipe_ingredients ?? new();
        }


        /////////////////////
        // Favourites
        /////////////////////

        /// <summary>
        /// Load a list of all favourites.
        /// </summary>
        /// <returns></returns>
        private Task LoadFavouritesAsync()
        {
            Load<Favourite>(ref _favourites, _settings.FavouriteFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of favourites and return the list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Favourite>?> GetFavouriteListAsync()
        {
            await LoadFavouritesAsync();
            return _favourites ?? new();
        }

        /// <summary>
        /// Load a single unique favourite and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Favourite?> GetFavouriteUniqueAsync(int id)
        {
            await LoadFavouritesAsync();
            if (_favourites == null)
            {
                throw new Exception("No favourites have been found");
            }
            return _favourites.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Check if the logged in user has set a favourites record for this recipe.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<Favourite?> CheckIsFavourite(int recipeId, int userId)
        {
            if (_favourites == null)
            {
                await LoadFavouritesAsync();
            }

            foreach (Favourite f in _favourites)
            {
                if (f.RecipeId == recipeId && f.UserId == userId)
                {
                    return f;
                }
            }

            return new Favourite();
        }

        /// <summary>
        /// Return the count of all favourites.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetFavouriteCountAsync()
        {
            await LoadFavouritesAsync();
            if (_favourites == null)
                return 0;
            else
                return _favourites.Count();
        }

        /// <summary>
        /// Save changes made to a favourites object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<Favourite?> SaveFavouriteAsync(Favourite editedObj)
        {
            if (_favourites == null)
            {
                LoadFavouritesAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_favourites.Count > 0)
                {
                    editedObj.Id = ((int)_favourites.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<Favourite>(_favourites, _settings.FavouriteFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete a favourite using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteFavouriteAsync(int id)
        {
            DeleteAsync(_favourites, _settings.FavouriteFolder, id);
            if (_favourites != null)
            {
                var editedObj = _favourites.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _favourites.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }


        /////////////////////
        // Shopping List
        /////////////////////

        /// <summary>
        /// Load all shopping lists.
        /// </summary>
        /// <returns></returns>
        private Task LoadShoppingListsAsync()
        {
            Load<ShoppingList>(ref _shopping_list, _settings.ShoppingListFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method will return any shopping lists associated with the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingList>?> GetShoppingListAsync(int userId)
        {
            await LoadShoppingListsAsync();
            List<ShoppingList> returnList = new();

            foreach (ShoppingList s in _shopping_list)
            {
                if (s.UserId == userId)
                {
                    returnList.Add(s);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Load a single unique shopping list and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ShoppingList?> GetShoppingUniqueAsync(int id)
        {
            await LoadShoppingListsAsync();
            if (_shopping_list == null)
            {
                throw new Exception("No shopping lists have been found");
            }
            return _shopping_list.FirstOrDefault(b => b.Id == id);
        }

        public async Task<ShoppingList?> CheckIsPinned(int recipeId, int userId)
        {
            if (_shopping_list == null)
            {
                await LoadShoppingListsAsync();
            }

            foreach (ShoppingList s in _shopping_list)
            {
                if (s.RecipeId == recipeId && s.UserId == userId)
                {
                    return s;
                }
            }

            return new ShoppingList();
        }

        /// <summary>
        /// Return the count of all shopping lists.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetShoppingCountAsync()
        {
            await LoadShoppingListsAsync();
            if (_shopping_list == null)
                return 0;
            else
                return _shopping_list.Count();
        }

        /// <summary>
        /// Save changes made to a shopping list object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<ShoppingList?> SaveShoppingListAsync(ShoppingList editedObj)
        {
            if (_shopping_list == null)
            {
                LoadShoppingListsAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_shopping_list.Count > 0)
                {
                    editedObj.Id = ((int)_shopping_list.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<ShoppingList>(_shopping_list, _settings.ShoppingListFolder, $"{editedObj.Id}.json", editedObj);

            // Save any list items that are attached
            if (editedObj.ShoppingListItems != null)
            {
                foreach (ShoppingListItem sItem in editedObj.ShoppingListItems)
                {
                    await SaveShoppingItemListAsync(sItem);
                }
            }

            return editedObj;
        }

        /// <summary>
        /// Delete a shopping list using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteShoppingListAsync(int id)
        {
            DeleteAsync(_shopping_list, _settings.ShoppingListFolder, id);
            if (_shopping_list != null)
            {
                var editedObj = _shopping_list.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _shopping_list.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Delete a shopping list using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ClearShoppingList(int listId)
        {
            if (_shopping_list != null)
            {
                foreach (ShoppingList s in _shopping_list)
                {
                    if (s.Id == listId)
                    {
                        List<ShoppingListItem> getList = await GetShoppingItemListAsync(s.Id);

                        if (getList != null && getList.Count > 0)
                        {
                            foreach (ShoppingListItem sl in getList)
                            {
                                sl.HasObtained = false;
                                SaveShoppingItemListAsync(sl);
                            }
                        }
                    }
                }
            }
        }

        public async Task<bool> IsShoppingListLimit(int userId)
        {
            await LoadShoppingListsAsync();

            if (_shopping_list != null)
            {
                List<ShoppingList> userList = new();

                foreach (ShoppingList s in _shopping_list)
                {
                    if (s.UserId == userId)
                    {
                        userList.Add(s);
                    }
                }

                if (userList.Count == 3)
                {
                    return true;
                }
            }

            return false;
        }


        ////////////////////////
        // Shopping List Items
        ////////////////////////

        /// <summary>
        /// Load all shopping list items.
        /// </summary>
        /// <returns></returns>
        private Task LoadShoppingItemListsAsync()
        {
            Load<ShoppingListItem>(ref _shopping_list_item, _settings.ShoppingListItemFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method will return any shopping items associated with the specified list.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ShoppingListItem>?> GetShoppingItemListAsync(int listId)
        {
            await LoadShoppingItemListsAsync();
            List<ShoppingListItem> returnList = new();

            foreach (ShoppingListItem s in _shopping_list_item)
            {
                if (s.ListId == listId)
                {
                    returnList.Add(s);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Load a single unique shopping list item and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ShoppingListItem?> GetShoppingItemUniqueAsync(int id)
        {
            await LoadShoppingItemListsAsync();
            if (_shopping_list_item == null)
            {
                throw new Exception("No shopping items have been found");
            }
            return _shopping_list_item.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all shopping list items.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetShoppingItemCountAsync()
        {
            await LoadShoppingItemListsAsync();
            if (_shopping_list_item == null)
                return 0;
            else
                return _shopping_list_item.Count();
        }

        /// <summary>
        /// Save changes made to a shopping item object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<ShoppingListItem?> SaveShoppingItemListAsync(ShoppingListItem editedObj)
        {
            if (_shopping_list_item == null)
            {
                LoadShoppingItemListsAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_shopping_list_item.Count > 0)
                {
                    editedObj.Id = ((int)_shopping_list_item.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<ShoppingListItem>(_shopping_list_item, _settings.ShoppingListItemFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete a shopping list item using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteShoppingItemAsync(int id)
        {
            DeleteAsync(_shopping_list_item, _settings.ShoppingListItemFolder, id);
            if (_shopping_list_item != null)
            {
                var editedObj = _shopping_list_item.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _shopping_list_item.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }

        public async Task CheckListHasItems(int listId, int recipeId)
        {
            if (_shopping_list_item == null)
            {
                LoadShoppingItemListsAsync();
            }
            bool hasItems = false;

            foreach (ShoppingListItem s in _shopping_list_item)
            {
                if (s.ListId == listId)
                {
                    hasItems = true;
                    break;
                }
            }

            if (!hasItems)
            {
                List<RecipeIngredient> ingList = await GetRecipeIngredientListAsync(recipeId);

                foreach (RecipeIngredient ing in ingList)
                {
                    ShoppingListItem listItem = new();
                    listItem.ListId = listId;
                    listItem.RecipeIngId = ing.Id;


                    Ingredient ingItem = await GetIngredientUniqueAsync(ing.IngredientId);
                    ing.IngredientName = ingItem.Name;

                    MeasurementType mType = await GetMeasurementTypeUniqueAsync(ing.MeasurementTypeId);
                    ing.MeasurementName = mType.Name;

                    listItem.Details = ing;
                    await SaveShoppingItemListAsync(listItem);
                }
            }

            //System.Diagnostics.Debug.WriteLine("Hold here.....");
        }



        /////////////////////
        // Cooking
        /////////////////////

        /// <summary>
        /// Load a list of all cooking objects.
        /// </summary>
        /// <returns></returns>
        private Task LoadCookingAsync()
        {
            Load<Cooking>(ref _cooking, _settings.CookingFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of cooking objects and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<Cooking>?> GetCookingListAsync(int cookingCount, int index)
        {
            await LoadCookingAsync();
            return _cooking ?? new();
        }

        /// <summary>
        /// Load a single unique cooking object and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Cooking?> GetCookingUniqueAsync(int id)
        {
            await LoadCookingAsync();
            if (_cooking == null)
            {
                throw new Exception("No cooking objects have been found");
            }
            return _cooking.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all cooking objects.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCookingCountAsync()
        {
            await LoadCookingAsync();
            if (_cooking == null)
            {
                return 0;
            }
            else
            {
                return _cooking.Count();
            }
        }

        /// <summary>
        /// Save changes made to a cooking object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<Cooking?> SaveCookingAsync(Cooking editedObj)
        {
            if (_cooking == null)
            {
                LoadCookingAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_cooking.Count > 0)
                {
                    editedObj.Id = ((int)_cooking.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<Cooking>(_cooking, _settings.CookingFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete an cooking object using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteCookingAsync(int id)
        {
            DeleteAsync(_cooking, _settings.CookingFolder, id);
            if (_cooking != null)
            {
                var editedObj = _cooking.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _cooking.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<bool> CheckRecipeStarted(int recipeId, int userId)
        {
            if (_cooking == null)
            {
                await LoadCookingAsync();
            }

            foreach (Cooking c in _cooking)
            {
                if (c.RecipeId == recipeId && c.UserId == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<CookingStep>?> CheckCookingStatus(int recipeId, int userId)
        {
            if (_cooking == null)
            {
                await LoadCookingAsync();
            }
            bool recipeStarted = false;

            Cooking cookObj = new();

            foreach (Cooking c in _cooking)
            {
                if (c.RecipeId == recipeId && c.UserId == userId)
                {
                    cookObj = c;
                    List<CookingStep> cSteps = await GetCookingStepListAsync(c.Id);

                    if (cSteps.Count > 0)
                    {
                        recipeStarted = true;
                    }
                    break;
                }
            }

            // Create our cooking obj and cooking steps
            if (!recipeStarted)
            {
                if (cookObj.Id == 0)
                {
                    cookObj.RecipeId = recipeId;
                    cookObj.UserId = userId;
                    cookObj.TimeStarted = DateTime.Now;
                    await SaveCookingAsync(cookObj);
                }

                List<RecipeStep> recipeSteps = await GetRecipeStepListAsync(recipeId);

                foreach (RecipeStep rs in recipeSteps)
                {
                    CookingStep cookStep = new();
                    cookStep.CookingId = cookObj.Id;
                    cookStep.StepId = rs.Id;
                    cookStep.StepNumber = rs.StepNumber;
                    await SaveCookingStepAsync(cookStep);
                }
            }

            List<CookingStep> stepList = await GetCookingStepListAsync(cookObj.Id);

            return stepList ?? new();
        }

        public async Task RestartCookingRecipe(int recipeId, int userId)
        {
            if (_cooking == null)
            {
                await LoadCookingAsync();
            }

            if (_cooking_steps == null)
            {
                await LoadCookingStepAsync();
            }

            List<CookingStep> deleteList = new();
            int cookId = 0;

            foreach (Cooking c in _cooking)
            {
                if (c.RecipeId == recipeId && c.UserId == userId)
                {
                    foreach (CookingStep cs in _cooking_steps)
                    {
                        if (cs.CookingId == c.Id)
                        {
                            deleteList.Add(cs);
                        }
                    }
                    cookId = c.Id;
                }
            }

            if (cookId > 0)
            {
                foreach (CookingStep cs in deleteList)
                {
                    await DeleteCookingStepAsync(cs.Id);
                }

                await DeleteCookingAsync(cookId);
            }
        }



        /////////////////////
        // Cooking Step
        /////////////////////

        /// <summary>
        /// Load a list of all cooking step objects.
        /// </summary>
        /// <returns></returns>
        private Task LoadCookingStepAsync()
        {
            Load<CookingStep>(ref _cooking_steps, _settings.CookingStepFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of cooking steps and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="ingredientCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<CookingStep>?> GetCookingStepListAsync(int cookingId)
        {
            await LoadCookingStepAsync();
            List<CookingStep> stepList = new();

            foreach (CookingStep cs in _cooking_steps)
            {
                if (cs.CookingId == cookingId)
                {
                    stepList.Add(cs);
                }
            }

            return stepList ?? new();
        }

        /// <summary>
        /// Load a single unique cooking step and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CookingStep?> GetCookingStepUniqueAsync(int id)
        {
            await LoadCookingStepAsync();
            if (_cooking_steps == null)
            {
                throw new Exception("No cooking steps have been found");
            }
            return _cooking_steps.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all cooking steps.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCookingStepCountAsync()
        {
            await LoadCookingStepAsync();
            if (_cooking_steps == null)
            {
                return 0;
            }
            else
            {
                return _cooking_steps.Count();
            }
        }

        /// <summary>
        /// Save changes made to a cooking step.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<CookingStep?> SaveCookingStepAsync(CookingStep editedObj)
        {
            if (_cooking_steps == null)
            {
                LoadCookingStepAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_cooking_steps.Count > 0)
                {
                    editedObj.Id = ((int)_cooking_steps.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<CookingStep>(_cooking_steps, _settings.CookingStepFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Delete an cooking object using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteCookingStepAsync(int id)
        {
            DeleteAsync(_cooking_steps, _settings.CookingStepFolder, id);
            //SaveLogEntryAsync(null, id, id, LogActionType.Read, "Token Renewed");

            if (_cooking_steps != null)
            {
                var editedObj = _cooking_steps.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _cooking_steps.Remove(editedObj);
                }
            }
            return Task.CompletedTask;
        }



        /////////////////////
        // Users
        /////////////////////

        /// <summary>
        /// Load a list of all users.
        /// </summary>
        /// <returns></returns>
        private Task LoadUsersAsync()
        {
            Load<User>(ref _users, _settings.UserFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of users and return the list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>?> GetUserListAsync()
        {
            await LoadUsersAsync();
            return _users ?? new();
        }

        public async Task<User?> GetUser(string userName, bool isGuidCheck)
        {
            await GetUserListAsync();

            if (_users != null)
            {
                foreach (User u in _users)
                {
                    if (isGuidCheck)
                    {
                        // Think this is redundant now, as is the isGuidCheck
                        if (u.UserToken.ToString() == userName)
                        {
                            await SaveLogEntryAsync(u, u.Id, u.Id, Models.Interfaces.CookCompAPI.LogActionType.Read, "Token Validated");
                            u.LastLoggedIn = DateTime.Now;
                            await SaveUserAsync(u);
                            return u;
                        }
                    }
                    else
                    {
                        if (u.Name.ToUpper() == userName.Trim().ToUpper())
                        {
                            await SaveLogEntryAsync(u, u.Id, u.Id, Models.Interfaces.CookCompAPI.LogActionType.Read, "Logged In");
                            u.UserToken = Guid.NewGuid();
                            u.LastLoggedIn = DateTime.Now;
                            await SaveUserAsync(u);
                            return u;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<User?> GetUser(int userId)
        {
            await GetUserListAsync();

            if (_users != null)
            {
                foreach (User u in _users)
                {
                    if (u.Id == userId)
                    {
                        return u;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// This will check if an existing token matches a user, and return that user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User?> GetUser(Guid userId)
        {
            await GetUserListAsync();

            if (_users != null)
            {
                foreach (User u in _users)
                {
                    if (u.UserToken == userId)
                    {
                        await SaveLogEntryAsync(u, u.Id, u.Id, Models.Interfaces.CookCompAPI.LogActionType.Read, "Token Renewed");
                        return u;
                    }
                }
            }

            return null;
        }

        public async Task<User?> SaveUserAsync(User editedObj)
        {
            if (_users == null)
            {
                await LoadUsersAsync();
            }

            if (editedObj.Id == 0)
            {
                int userId = 0;

                if (_users?.Count == 0)
                {
                    userId = 1;
                }
                else
                {
                    userId = ((int)_users.Max(b => b.Id)) + 1;
                }

                if (editedObj.Id == 0)
                {
                    editedObj.Id = userId;
                }
            }

            await SaveAsync<User>(_users, _settings.UserFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        public async Task<User?> GetUserUniqueAsync(int id)
        {
            await LoadUsersAsync();
            if (_users == null)
            {
                throw new Exception("No users have been found");
            }
            return _users.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all users.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetUserCountAsync()
        {
            await LoadUsersAsync();
            if (_users == null)
                return 0;
            else
                return _users.Count();
        }

        /// <summary>
        /// Delete a user using the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteUserAsync(int id)
        {
            DeleteAsync(_users, _settings.UserFolder, id);
            if (_users != null)
            {
                var editedObj = _users.FirstOrDefault(b => b.Id == id);
                if (editedObj != null)
                {
                    _users.Remove(editedObj);
                }

            }
            return Task.CompletedTask;
        }


        /////////////////////
        // Questionnaire Obj
        /////////////////////

        /// <summary>
        /// Load a list of all questionnaires.
        /// </summary>
        /// <returns></returns>
        private Task LoadQuestionnairesAsync()
        {
            Load<QuestionnaireObj>(ref _questions, _settings.QuestionnaireFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Load a list of all questionnaires and return the list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionnaireObj>?> GetQuestionnaireObjListAsync()
        {
            await LoadQuestionnairesAsync();
            return _questions ?? new();
        }

        public async Task<QuestionnaireObj?> CheckUserQuestionnaire(int userId)
        {
            if (_questions == null)
            {
                await LoadQuestionnairesAsync();
            }
            
            foreach(QuestionnaireObj question in _questions)
            {
                if (question.UserId == userId)
                {
                    return question;
                }
            }

            QuestionnaireObj newQuestionnaire = new();

            if (_questions.Count > 0)
            {
                newQuestionnaire.Id = _questions.Max(x => x.Id) + 1;
            }
            else
            {
                newQuestionnaire.Id = 1;
            }
            newQuestionnaire.UserId = userId;
            newQuestionnaire.TimeStarted = DateTime.Now;

            return newQuestionnaire;
        }

        public async Task<QuestionnaireObj?> SaveQuestionnaireAsync(QuestionnaireObj editedObj)
        {
            if (_questions == null)
            {
                LoadQuestionnairesAsync();
            }

            editedObj.IsCompleted = true;
            editedObj.TimeEnded = DateTime.Now;

            await SaveAsync<QuestionnaireObj>(_questions, _settings.QuestionnaireFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }


        /////////////////////
        // Tasks
        /////////////////////
        private Task LoadTasksAsync()
        {
            Load<TaskObj>(ref _tasks, _settings.TasksFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Loads a list of all tasks if there are any.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskObj>?> GetTaskListAsync()
        {
            await LoadTasksAsync();
            return _tasks ?? new();
        }

        /// <summary>
        /// Checks to see if the provided user and task have a match. 
        /// If so the user has completed this task (true)_
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckTaskStatus(int taskId, int userId)
        {
            if (_tasks == null)
            {
                await LoadTasksAsync();
            }

            foreach (TaskObj task in _tasks)
            {
                if (task.TaskId == taskId && task.UserId == userId)
                {
                    if (task.TaskFailed || task.TimeEnded.HasValue)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<TaskObj?> GetActiveTask(int taskId, int userId)
        {
            if (_tasks == null)
            {
                await LoadTasksAsync();
            }

            foreach (TaskObj task in _tasks)
            {
                if (task.TaskId == taskId && task.UserId == userId)
                {
                    return task;
                }
            }

            return new TaskObj();
        }

        public async Task<TaskObj?> SaveTaskAsync(TaskObj editedObj)
        {
            if (_users == null)
            {
                await LoadTasksAsync();
            }

            if (editedObj.Id == 0)
            {
                int taskId = 0;

                if (_tasks?.Count == 0)
                {
                    taskId = 1;
                }
                else
                {
                    taskId = ((int)_tasks.Max(b => b.Id)) + 1;
                }

                if (editedObj.Id == 0)
                {
                    editedObj.Id = taskId;
                }
            }

            await SaveAsync<TaskObj>(_tasks, _settings.TasksFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        public async Task<List<TaskObj>?> GetUserTaskList(int userId)
        {
            if (_users == null)
            {
                await LoadUsersAsync();
            }

            if (_tasks == null)
            {
                await LoadTasksAsync();
            }

            List<TaskObj> returnCollection = new();

            foreach(TaskObj task in _tasks)
            {
                if(task.UserId == userId)
                {
                    returnCollection.Add(task);
                }
            }

            return returnCollection;
        }



        /////////////////////
        // Ingredients
        /////////////////////

        /// <summary>
        /// Load a list of all consent objects.
        /// </summary>
        /// <returns></returns>
        private Task LoadConsentObjectsAsync()
        {
            Load<ConsentObj>(ref _consent_forms, _settings.ConsentObjectFolder);
            return Task.CompletedTask;
        }

        ///
        /// <summary>
        /// Load a list of consent objects and return the list. The count and index will determine the range and amount returned.
        /// </summary>
        /// <param name="consentCount"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<List<ConsentObj>?> GetConsentListAsync(int consentCount, int index)
        {
            await LoadConsentObjectsAsync();
            return _consent_forms ?? new();
        }

        /// <summary>
        /// Load a single unique consent object and return it. Requires a correct unique ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ConsentObj?> GetConsentUniqueAsync(int id)
        {
            await LoadConsentObjectsAsync();
            if (_consent_forms == null)
            {
                throw new Exception("No consent objects have been found");
            }
            return _consent_forms.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Return the count of all ingredients.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetConsentCountAsync()
        {
            await LoadConsentObjectsAsync();
            if (_consent_forms == null)
            {
                return 0;
            }
            else
            {
                return _consent_forms.Count();
            }
        }

        /// <summary>
        /// Save changes made to an ingredient object.
        /// </summary>
        /// <param name="editedObj"></param>
        /// <returns></returns>
        public async Task<ConsentObj?> SaveConsentObjAsync(ConsentObj editedObj)
        {
            if (_consent_forms == null)
            {
                LoadConsentObjectsAsync();
            }

            if (editedObj.Id == 0)
            {
                if (_consent_forms.Count > 0)
                {
                    editedObj.Id = ((int)_consent_forms.Max(b => b.Id)) + 1;
                }
                else
                {
                    editedObj.Id = 1;
                }
            }
            await SaveAsync<ConsentObj>(_consent_forms, _settings.ConsentObjectFolder, $"{editedObj.Id}.json", editedObj);
            return editedObj;
        }

        /// <summary>
        /// Check if a user has already completed the consent form.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckConsentStatus(int userId)
        {
            if (_consent_forms == null)
            {
                await LoadConsentObjectsAsync();
            }

            foreach (ConsentObj obj in _consent_forms)
            {
                if(obj.UserId == userId && obj.ConsentCompleted)
                {
                    return true;
                }
            }

            return false;
        }



        /////////////////////
        // Logs
        /////////////////////

        /// <summary>
        /// Load the log entry repository.
        /// </summary>
        /// <returns></returns>
        private Task LoadLogEntriesAsync()
        {
            Load<LogEntry>(ref _logs, _settings.LogEntryFolder);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Save a log entry using the specified parameters.
        /// </summary>
        /// <param name="dataObj"></param>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <param name="lat"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SaveLogEntryAsync(object dataObj, int userId, int entityId,
            Models.Interfaces.CookCompAPI.LogActionType lat, string message)
        {
            if (_logs == null)
            {
                await LoadLogEntriesAsync();
            }

            int logId = 0;

            if (_logs?.Count == 0)
            {
                logId = 1;
            }
            else
            {
                logId = ((int)_logs.Max(b => b.Id)) + 1;
            }

            LogEntry newEntry = new()
            {
                Id = logId,
                Date = DateTime.Now,
                UserId = userId,
                EntityType = dataObj.GetType().Name,
                EntityId = entityId,
                Action = lat.ToString(),
                Message = message,
                EntityJson = JsonSerializer.Serialize(dataObj)
            };

            await SaveAsync<LogEntry>(_logs, _settings.LogEntryFolder, $"{newEntry.Id}.json", newEntry);
        }

        public async Task SaveManualLogEntry(int userId, int entityId,
            Models.Interfaces.CookCompAPI.UserLogs ul, string entityType)
        {
            if (_logs == null)
            {
                await LoadLogEntriesAsync();
            }

            int logId = 0;

            if (_logs?.Count == 0)
            {
                logId = 1;
            }
            else
            {
                logId = ((int)_logs.Max(b => b.Id)) + 1;
            }

            LogEntry newEntry = new()
            {
                Id = logId,
                Date = DateTime.Now,
                UserId = userId,
                EntityId = entityId,
                Action = ul.ToString(),
                EntityType = entityType,
                IsManual = true
            };

            await SaveAsync<LogEntry>(_logs, _settings.LogEntryFolder, $"{newEntry.Id}.json", newEntry);
        }

        /// <summary>
        /// Return a list of log entries using the parameters specified
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public async Task<List<LogEntry>?> GetLogEntryListAsync(int userId, string entityType)
        {
            await LoadLogEntriesAsync();

            List<LogEntry> logList = new();

            foreach (LogEntry log in _logs)
            {
                if ((userId == 0 || log.UserId == userId) &&
                    (entityType == string.Empty || log.EntityType == entityType))
                {
                    logList.Add(log);
                }
            }

            return logList ?? new();
        }

        public async Task<List<LogEntry>?> GetManualLogEntryList()
        {
            await LoadLogEntriesAsync();

            List<LogEntry> logList = new();

            foreach (LogEntry log in _logs)
            {
                if (log.IsManual)
                {
                    logList.Add(log);
                }
            }

            return logList ?? new();
        }

        public async Task<List<LogEntry>?> GetUserManualLogEntryList(int userId)
        {
            await LoadLogEntriesAsync();

            List<LogEntry> logList = new();

            foreach (LogEntry log in _logs)
            {
                if (log.IsManual && log.UserId == userId)
                {
                    logList.Add(log);
                }
            }

            logList = logList.OrderBy(x => x.Date).ToList();

            return logList ?? new();
        }

        public void Output(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
    }
}
