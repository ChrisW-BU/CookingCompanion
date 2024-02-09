namespace Data.Models
{
    public class LogEntry
    {
        public LogEntry() { }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } // The logged in user id
        public string EntityType { get; set; } = string.Empty;// The model
        public int EntityId { get; set; } // The id of the object
        public string Action { get; set; } = string.Empty; // Read, save, delete
        public string Message { get; set; } = string.Empty; // A further description
        public string EntityJson { get; set; } = string.Empty; // Json of the object at the time
        public bool IsManual { get; set; }
    }
}