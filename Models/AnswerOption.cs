using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class AnswerOption
    {
        public int AnswerOptionId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Display(Name = "Answer Text")]
        [Required]
        public string AnswerText { get; set; }
        [Required]
        public bool Correct { get; set; }
    }
}