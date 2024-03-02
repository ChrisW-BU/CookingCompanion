namespace Data.Models
{
    public class ConsentObj
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool PointOne { get; set; }

        public bool PointTwo { get; set; }

        public bool PointThree { get; set; }

        public bool PointFour { get; set; }

        public bool PointFive { get; set; }

        public bool PointSix { get; set; }

        public bool PointSeven { get; set; }

        public bool PointEight { get; set; }

        public bool PointNine { get; set; }

        public bool PointTen { get; set; }

        public bool PointEleven { get; set; }

        public string ParticipantName { get; set; } = string.Empty;

        public DateTime? ParticipantSignDate { get; set; }

        public string ResearcherName { get; set; } = "Chris Whitehead";

        public DateTime? ResearcherSignDate { get; set; }

        public bool ConsentCompleted { get; set; }
    }
}
