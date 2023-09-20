namespace Data.Models
{
    public class RecipeStep
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public int Quantity { get; set; }

        public string QuantityType { get; set; } = string.Empty;
    }
}
