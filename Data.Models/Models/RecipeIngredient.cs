﻿namespace Data.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public decimal Quantity { get; set; }

        public int QuantityType { get; set; }

        public string IngredientName { get; set; } = string.Empty;
    }
}
