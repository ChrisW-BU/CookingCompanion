namespace Data.Models
{
    public class CookingStep
    {
        public int Id { get; set; }

        public int CookingId { get; set; }

        public int StepId { get; set; }

        public decimal StepNumber { get; set; }

        public bool IsCompleted { get; set; }

        //
        public DateTime? StepTimerStarted { get; set; }

        public DateTime? StepTimerEnded { get; set; }
    }
}
