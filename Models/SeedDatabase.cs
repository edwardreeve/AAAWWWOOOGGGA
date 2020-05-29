using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizManager.Data;

namespace QuizManager.Models
{
    public class SeedDatabase
    {
        private const int NumberOfQuizzes = 2;
        private const int QuestionsPerQuiz = 10;
        private const int AnswersPerQuestion = 4;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                //The app should only seed if the DB is empty
                if (context.Quiz.Any())
                {
                    return;
                }

                SeedAllDatabaseItems(context);

                context.SaveChanges();
            }
        }

        private static void SeedAllDatabaseItems(ApplicationDbContext context)
        {
            var quizzes = SeedDatabaseQuizzes(context);
            var questions = SeedDatabaseQuestions(context, quizzes);
            SeedDatabaseAnswers(context, questions);
        }

        private static List<Quiz> SeedDatabaseQuizzes(ApplicationDbContext context)
        {
            //This resets the ID auto-incrementing ID column to 0, to keep the quiz/question/answers synced up
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Quiz', RESEED, 0)");
            var quizzes = new List<Quiz>();

            for (var index = 1; index <= NumberOfQuizzes; index++)
            {
                var quiz = new Quiz
                {
                    Name = $"Quiz Number {index}"
                };

                quizzes.Add(quiz);
            }

            context.Quiz.AddRange(quizzes);
            context.SaveChanges();
            return quizzes;
        }

        private static List<Question> SeedDatabaseQuestions(ApplicationDbContext context, List<Quiz> quizzes)
        {
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Question', RESEED, 0)");
            var questions = new List<Question>();

            var questionIdCounter = 1;
            foreach (var quiz in quizzes)
            {
                var positionCounter = 1;
                for (var index = 1; index <= QuestionsPerQuiz; index++)
                {
                    var question = new Question
                    {
                        QuestionText = $"Text of Question {questionIdCounter} for '{quiz.Name}' (id:{quiz.QuizId})",
                        QuizId = quiz.QuizId,
                        Position = positionCounter
                    };

                    questions.Add(question);

                    positionCounter += 1;
                    questionIdCounter += 1;
                }
            }

            context.Question.AddRange(questions);
            context.SaveChanges();
            return questions;
        }

        private static void SeedDatabaseAnswers(ApplicationDbContext context, List<Question> questions)
        {
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('AnswerOption', RESEED, 0)");
            var answerOptions = new List<AnswerOption>();

            var answerOptionIdCounter = 1;
            foreach (var question in questions)
                for (var index = 1; index <= AnswersPerQuestion; index++)
                {
                    var answerOption = new AnswerOption
                    {
                        AnswerText =
                        $"Text of Answer {answerOptionIdCounter} for Question {question.QuestionId}",
                        //Sets 1 question as true out of every 'batch'
                        Correct = index % AnswersPerQuestion == 0,
                        QuestionId = question.QuestionId
                    };

                    answerOptions.Add(answerOption);

                    answerOptionIdCounter += 1;
                }

            context.AnswerOption.AddRange(answerOptions);
            context.SaveChanges();
        }
    }
}