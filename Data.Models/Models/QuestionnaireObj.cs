namespace Data.Models
{
    public class QuestionnaireObj
    {
        public int Id { get; set; }

        public int UserId { get; set; }



        // USER INFORMATION
        protected string QuestionOne = "1). Which type of device did you use to perform this test?";
        public string GetQuestionOne()
        {
            return QuestionOne;
        }
        protected List<string> QuestionOneOptions = new() { "Smart Phone", "Desktop/Laptop", "Tablet", "Smart TV", "Gaming Console"};
        public List<string> GetQuestionOneOptions()
        {
            return QuestionOneOptions;
        }
        public int QuestionOneAnswer { get; set; }



        protected string QuestionTwo = "2). What age group do you fall in?";
        public string GetQuestionTwo()
        {
            return QuestionTwo;
        }
        protected List<string> QuestionTwoOptions = new() { "18-35", "36-49", "50-64", "65+" };
        public List<string> GetQuestionTwoOptions()
        {
            return QuestionTwoOptions;
        }
        public int QuestionTwoAnswer { get; set; }



        protected string QuestionThree = "3). How frequently do you use digital recipe aids? " +
            "Examples include cooking apps, website recipes and videos.";
        public string GetQuestionThree()
        {
            return QuestionThree;
        }
        protected List<string> QuestionThreeOptions = new() { "Always", "Sometimes", "Never" };
        public List<string> GetQuestionThreeOptions()
        {
            return QuestionThreeOptions;
        }
        public int QuestionThreeAnswer { get; set; }



        // SYSTEM FEEDBACK
        protected string QuestionFour = "4). When using the prototype, did you encounter any problems? " +
            "If so, can you please give some details as to what happened. "; // YES/NO
        public string GetQuestionFour()
        {
            return QuestionFour;
        }
        public int QuestionFourAnswer { get; set; }
        public string QuestionFourAnswerString { get; set; } = string.Empty;



        protected string QuestionFive = "5). 'The prototype responded promptly to my interactions.'";
        public string GetQuestionFive() // LIKERT
        {
            return QuestionFive;
        }
        public int QuestionFiveAnswer { get; set; }



        //public string QuestionSix = "6). Were there any times you performed an action and"
        //    +" were met with an unexpected reaction? If so, can you please give some details of what occurred. ";
        //public string GetQuestionSix()
        //{
        //    return QuestionSix;
        //}
        //public int QuestionSixAnswer { get; set; }
        //public string QuestionSixAnswerString { get; set; } = string.Empty;



        // UX/UI QUESTIONS
        public string QuestionSeven = "6). 'I found the user interface intuitive and user friendly.'";
        public string GetQuestionSeven() // LIKERT
        {
            return QuestionSeven;
        }
        public int QuestionSevenAnswer { get; set; }



        public string QuestionEight = "7). 'I was able to complete a recipe from start to finish.'";
        public string GetQuestionEight() // YES/NO
        {
            return QuestionEight;
        }
        public int QuestionEightAnswer { get; set; }



        public string QuestionNine = "8). Did all features work as you anticipated e.g., the cooking process, favourites, " +
            "and shopping list? If not, can you explain why? ";
        public string GetQuestionNine() // YES/NO
        {
            return QuestionNine;
        }
        public int QuestionNineAnswer { get; set; }
        public string QuestionNineAnswerString { get; set; } = string.Empty;



        public string QuestionTen = "9). 'The cooking process was clear and helpful.'";
        public string GetQuestionTen() // LIKERT
        {
            return QuestionTen;
        }
        public int QuestionTenAnswer { get; set; }



        public string QuestionEleven = "10). Were there any elements of the design that you found distracting or overwhelming? " +
            "If yes, can you briefly detail them. ";
        public string GetQuestionEleven() // BINARY
        {
            return QuestionEleven;
        }
        public int QuestionElevenAnswer { get; set; }
        public string QuestionElevenAnswerString { get; set; } = string.Empty;



        public string QuestionTwelve = "11). 'The prototype succeeded in guiding me through it's features.'";
        public string GetQuestionTwelve() // LIKERT
        {
            return QuestionTwelve;
        }
        public int QuestionTwelveAnswer { get; set; }



        public string QuestionThirteen = "12). What changes would you make to the prototype to improve the user experience, and why?";
        public string GetQuestionThirteen() // TEXT
        {
            return QuestionThirteen;
        }
        public string QuestionThirteenAnswer { get; set; } = string.Empty;



        public string QuestionFourteen = "13). 'I would use this tool when I am cooking a recipe.'";
        public string GetQuestionFourteen() // LIKERT
        {
            return QuestionFourteen;
        }
        public int QuestionFourteenAnswer { get; set; }



        public string QuestionFifteen = "14). 'I would recommend this tool to someone else.'";
        public string GetQuestionFifteen() // LIKERT
        {
            return QuestionFifteen;
        }
        public int QuestionFifteenAnswer { get; set; }



        public List<string> binaryOptions = new() { "Yes", "No" };
        public List<string> GetBinaryOptions()
        {
            return binaryOptions;
        }



        public List<string> likertOptions = new() { "Strongly Disagree", "Disagree", "Undecided", "Agree", "Strongly Agree" };
        public List<string> GetLikertOptions()
        {
            return likertOptions;
        }


        public DateTime? TimeStarted { get; set; }

        public DateTime? TimeEnded { get; set; }

        public bool IsCompleted { get; set; }
    }
}
