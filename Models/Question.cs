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
        [AnswerOptionValidationAttributes.CorrectNumberOfAnswerOptions(3,5, ErrorMessage = "Question can only have between 3 and 5 answer options")]
        [AnswerOptionValidationAttributes.OneCorrectAnswer(ErrorMessage = "Question must have one Correct answer")]
        [AnswerOptionValidationAttributes.NoDuplicates(ErrorMessage = "Question can't have 2 or more identical answers")]
        [AnswerOptionValidationAttributes.CorrectAnswerCantBeBlank(ErrorMessage = "Correct answer can't be blank")]
        [AnswerOptionValidationAttributes.AtLeastThreeAnswersWithText(ErrorMessage = "Too many blank answers - question must have at least 3 valid answer options")]
        [Required]
        [Display(Name = "Answer Options")]
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}