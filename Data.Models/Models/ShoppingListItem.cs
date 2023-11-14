namespace Data.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        public int ListId { get; set; } // Shopping List ID

        public int RecipeIngId { get; set; } // Ingredient ID

        public bool HasObtained { get; set; }

        public RecipeIngredient Details { get; set; }
    }
}
