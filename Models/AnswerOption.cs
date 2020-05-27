using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class AnswerOption
    {
        public int AnswerOptionId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool Correct { get; set; }
    }
}