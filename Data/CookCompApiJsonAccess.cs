using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Data.Models;

namespace Data;

public class CookCompApiJsonAccess: CookCompAPI
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

    /// <summary>
    /// This allows the injection of IOptions and creates a settings structure for the data folders.
    /// </summary>
    /// <param name="option"></param>
    public CookCompApiJsonAccess(IOptions<CookCompApiJsonAccessSetting>option)
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
        if(_ingredients == null)
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
        if(_recipes == null)
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
        if(_recipe_ingredients == null)
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
}
