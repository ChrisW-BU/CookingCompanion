namespace Data.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public bool IsPinned { get; set; }

        public bool IsCompleted { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ShoppingListItem> ShoppingListItems { get; set; } = new();

    }
}
