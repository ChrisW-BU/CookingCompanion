namespace Data.Models
{
    public class QuestionnaireObj
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        // USER INFORMATION
        protected string QuestionOne = "What device did you use to perform this test?";
        public int QuestionOneAnswer { get; set; }


        public string QuestionTwo = "What age group do you fall in?";
        public int QuestionTwoAnswer { get; set; }


        public string QuestionThree = "How frequently do you use digital recipe aids? " +
            "Examples include cooking apps, website recipes and videos.";
        public int QuestionThreeAnswer { get; set; }

        // SYSTEM FEEDBACK
        public string QuestionFour = "When using the app, did you encounter any errors? " +
            "If so, can you please give some details as to what happened. ";
        public int QuestionFourAnswer { get; set; }


        public string QuestionFive = "Did the app respond promptly to your interactions?";
        public int QuestionFiveAnswer { get; set; }


        public string QuestionSix = "•\tWere there any times you performed an action and"
            +" were met with an unexpected reaction? If so, can you please elaborate. ";
        public int QuestionSixAnswer { get; set; }


        // UX/UI QUESTIONS
        public string QuestionSeven = "Did you find the user interface intuitive and user-friendly? " +
            "If not, can you elaborate on the issues? ";
        public int QuestionSevenAnswer { get; set; }


        public string QuestionEight = "Were you able to successfully complete a recipe from start to finish? " +
            "If not, what were the main factors in this? ";
        public int QuestionEightAnswer { get; set; }


        public string QuestionNine = "Did all features work as you anticipated e.g., the cooking process, favourites, " +
            "and shopping list? If not, can you explain why? ";
        public int QuestionNineAnswer { get; set; }


        public string QuestionTen = "Is the cooking process clear and helpful? If not, can you explain why? ";
        public int QuestionTenAnswer { get; set; }


        public string QuestionEleven = "Were there any elements of the design that you found distracting or overwhelming? " +
            "If yes, can you briefly detail them. ";
        public int QuestionElevenAnswer { get; set; }


        public string QuestionTwelve = "How successful was the app in guiding you through the whole process " + 
            "(from initial sign in to cooking a recipe)?";
        public int QuestionTwelveAnswer { get; set; }


        public string QuestionThirteen = "What changes would you make to the app to improve the experience, and why?";
        public int QuestionThirteenAnswer { get; set; }


        public string QuestionFourteen = "Would you use this tool when cooking? ";
        public int QuestionFourteenAnswer { get; set; }


        public DateTime? TimeStarted { get; set; }

        public DateTime? TimeEnded { get; set; }

        public bool IsCompleted { get; set; }
    }
}
