using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class AnswerOption
    {
        //Just a test comment for git purposes
        public int AnswerOptionId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Display(Name = "Answer Text")]
        public string AnswerText { get; set; }
        public bool Correct { get; set; }
    }
}