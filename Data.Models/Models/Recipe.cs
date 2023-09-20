namespace Data.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        public bool? IsCompleted { get; set; }
    }
}
