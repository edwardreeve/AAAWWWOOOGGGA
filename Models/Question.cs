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
        [AnswerOptionValidationAttributes.CorrectNumberOfAnswerOptions(3,5, ErrorMessage = "Question can only have between 3 and 5 Answer Options")]
        [AnswerOptionValidationAttributes.OneCorrectAnswer(ErrorMessage = "Question must have one Correct answer")]
        [AnswerOptionValidationAttributes.NoDuplicates(ErrorMessage = "Question can't have 2 or more identical answers")]
        [AnswerOptionValidationAttributes.NoBlankAnswers(ErrorMessage = "Answer Options can't be left blank")]
        [Required]
        [Display(Name = "Answer Options")]
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}