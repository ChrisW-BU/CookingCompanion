namespace Data.Models
{
    public class RecipeStep
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public decimal StepNumber { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImgUrl { get; set; } = string.Empty;

        public string PreviousStep { get; set; } = string.Empty;

        public string PreviousStepImgUrl { get; set; } = string.Empty;

        public string TimerString { get; set; } = string.Empty;


        // When we create a cooking recipe step, keep track of timings
        public bool HasTimer { get; set; }

        public decimal TimerDuration { get; set; } // seconds
    }
}
