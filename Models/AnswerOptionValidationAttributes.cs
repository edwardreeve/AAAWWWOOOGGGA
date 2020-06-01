using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QuizManager.Models
{
    public class AnswerOptionValidationAttributes
    {

        public class CorrectNumberOfAnswerOptionsAttribute : ValidationAttribute
        {
            private int _minNumber;
            private int _maxNumber;

            public CorrectNumberOfAnswerOptionsAttribute(int minNumber, int maxNumber)
            {
                _minNumber = minNumber;
                _maxNumber = maxNumber;
            }
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null || answers.All(ans => ans.AnswerText == null))
                {
                    return false;
                }
                return answers.Count >= 3 && answers.Count <= 5;
            }
        }

        public class NoDuplicatesAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null || answers.All(ans => ans.AnswerText == null))
                {
                    return false;
                }

                return !answers
                    .Where(ans => ans.AnswerText != null)
                    .GroupBy(ans => ans.AnswerText)
                    .Any(group => group.Count() > 1);
            }
        }

        public class OneCorrectAnswerAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null || answers.All(ans => ans.AnswerText == null))
                {
                    return false;
                }

                return answers.Count(ans => ans.Correct) == 1;
            }
        }
        public class CorrectAnswerCantBeBlank : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null || answers.All(ans => ans.AnswerText == null))
                {
                    return false;
                }

                var correctAnswer = answers.FirstOrDefault(ans => ans.Correct);
                return correctAnswer?.AnswerText != null;
            }
        }

        public class AtLeastThreeAnswersWithText : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null || answers.All(ans => ans.AnswerText == null))
                {
                    return false;
                }

                var textAnswers = answers.Where(ans => ans.AnswerText != null).ToList();
                return textAnswers.Count >= 3;
            }
        }
    }
}