namespace Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public Guid UserToken { get; set; } = Guid.Empty;

        public bool TasksCompleted { get; set; }

        public bool QuestionnaireCompleted { get; set; }

        public bool ConsentCompleted { get; set; }
    }
}
