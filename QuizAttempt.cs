using System;

namespace Excel_Test
{
    public class QuizAttempt
    {
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public double Percentage { get; set; }
        public string Grade { get; set; }
    }
}
