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

                if (answers == null)
                {
                    return false;
                }

                return answers != null && answers.Count >= 3 && answers.Count <= 5;
            }
        }

        public class NoDuplicatesAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null)
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

                if (answers == null)
                {
                    return false;
                }

                return answers.Count(ans => ans.Correct) == 1;
            }
        }

        public class NoBlankAnswersAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var answers = value as List<AnswerOption>;

                if (answers == null)
                {
                    return false;
                }

                return answers.All(ans => ans.AnswerText != null);
            }
        }
    }
}