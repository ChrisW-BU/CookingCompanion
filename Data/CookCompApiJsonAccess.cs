using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Data.Models;
using RPCC.Data.Models;

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


    /// <summary>
    /// Load a list of all ingredients.
    /// </summary>
    /// <returns></returns>
    private Task LoadIngredientAsync()
    {
        Load<Ingredient>(ref _ingredients, _settings.IngredientFolder);
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync()
    {
        _recipes = null;
        _recipe_steps = null;
        _recipe_ingredients = null;
        _ingredients = null;
        _shopping_list = null;
        _shopping_list_item = null;
        _favourites = null;
        //_users = null;

        return Task.CompletedTask;
    }

    /////////////////////
    // Load Ingredients
    /////////////////////
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
            throw new Exception("No ingredients have been found");
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
            return 0;
        else
            return _ingredients.Count();
    }


    ////////////////
    // Save Entities
    ////////////////
    public async Task<Ingredient?> SaveIngredientAsync(Ingredient editedObj)
    {
        if (editedObj.Id == 0)
        {
            if (_ingredients != null)
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

    //////////////////
    // Delete Entities
    //////////////////

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
}
