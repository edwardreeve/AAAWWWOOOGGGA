using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        [Required]
        public int QuizId { get; set; }
        [Required]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }
        public int Position { get; set; }
        [Required]
        [Display(Name = "Answer Options")]
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}