﻿namespace Data.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
