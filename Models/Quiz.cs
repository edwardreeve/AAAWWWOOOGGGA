using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Quiz name must be between 2 and 255 characters")]
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
    }
}