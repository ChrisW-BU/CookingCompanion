using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RPCC.Data;
using Data;
using Data.Models.Interfaces;
using Data.Models.Models;
using Components.RazorComponents.Forms;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<UserGlobal>();
builder.Services.AddScoped<PageHistory>();
builder.Services.AddBlazoredLocalStorage();

// Setup JSON API
builder.Services.AddOptions<CookCompApiJsonAccessSetting>().Configure(options =>
{
    options.DataRootPath = @"..\Data\";
    options.RecipeFolder = "Recipes";
    options.RecipeIngredientFolder = "RecipeIngredients";
    options.RecipeStepFolder = "RecipeSteps";
    options.IngredientFolder = "Ingredients";
    options.UserFolder = "Users";
    options.ShoppingListFolder = "ShoppingLists";
    options.ShoppingListItemFolder = "ShoppingListItems";
    options.FavouriteFolder = "Favourites";
    options.MeasurementTypeFolder = "MeasurementTypes";
    options.CookingFolder = "Cooking";
    options.CookingStepFolder = "CookingSteps";
}
);

// When we ask for CompCookAPI, we will get an instance of JsonAccess back from dependency injection
builder.Services.AddScoped<CookCompAPI, CookCompApiJsonAccess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
