namespace Data.Models
{
    public class TaskObj
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TaskId { get; set; }

        public DateTime? TimeStarted { get; set; }

        public DateTime? TimeEnded { get; set; }

        public bool TaskFailed { get; set; }
    }
}
