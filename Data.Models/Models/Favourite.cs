namespace Data.Models
{
    public class Favourite
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public bool IsFavourite { get; set; }
    }
}
