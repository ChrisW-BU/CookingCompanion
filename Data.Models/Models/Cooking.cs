namespace Data.Models
{
    public class Cooking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public DateTime? TimeStarted { get; set; }

        public DateTime? TimeEnded { get; set; }
    }
}
