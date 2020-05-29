using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NUnit.Framework;
using QuizManager.Controllers;
using QuizManager.Data;
using QuizManager.Models;

namespace QuizManager.Tests.UnitTests
{
    [TestFixture]
    public class QuizzesControllerTests : IDisposable
    {
        private DbConnection _dbConnection;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        private const int QuestionsPerQuiz = 2;
        private const int AnswersPerQuestion = 4;

        [SetUp]
        public async Task Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(CreateTestingDatabase())
                .Options;

            _dbConnection = RelationalOptionsExtension.Extract(_dbContextOptions).Connection;

            await SeedTestDatabase();
        }

        #region Index Tests

        [Test]
        public async Task Index_returns_view_result()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Index();
                actionResult.Should().BeOfType(typeof(ViewResult));

                var viewResult = (ViewResult)actionResult;

                var quizList = viewResult.Model as List<Quiz>;

                quizList.Count.Should().Be(2);
            }
        }

        [Test]
        public async Task Index_returns_view_result_with_two_quizVM_objects()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Index();

                var viewResult = (ViewResult)actionResult;
                var quizList = viewResult.Model as List<Quiz>;

                quizList.Count.Should().Be(2);
            }
        }

        #endregion

        #region Details Tests

        [Test]
        public async Task Details_returns_view_result_with_correct_quiz()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Details(1);

                actionResult.Should().BeOfType(typeof(ViewResult));

                var viewResult = actionResult as ViewResult;
                var quiz = viewResult.Model as Quiz;

                quiz.QuizId.Should().Be(1);
            }
        }

        [Test]
        public async Task Quiz_returned_by_details_view_has_all_associated_questions()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Details(1);
                var viewResult = actionResult as ViewResult;
                var quiz = viewResult.Model as Quiz;

                quiz.Questions.Count.Should().Be(QuestionsPerQuiz);
            }
        }

        [Test]
        public async Task Questions_returned_with_quiz_all_have_correct_quiz_id()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Details(1);
                var viewResult = actionResult as ViewResult;
                var quiz = viewResult.Model as Quiz;

                quiz.Questions.Should().OnlyContain(question => question.QuizId == quiz.QuizId);
            }
        }

        [Test]
        public async Task Questions_returned_with_quiz_have_all_associated_answers()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Details(1);
                var viewResult = actionResult as ViewResult;
                var quiz = viewResult.Model as Quiz;
                var questions = quiz.Questions;
                foreach (var question in questions)
                {
                    question.AnswerOptions.Count.Should().Be(AnswersPerQuestion);
                    question.AnswerOptions.Should().OnlyContain(answer => answer.QuestionId == question.QuestionId);
                }
            }
        }

        #endregion

        #region Create and Seed Test Database Methods

        private static DbConnection CreateTestingDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private async Task SeedTestDatabase(int numberOfQuizzes = 2)
        {
            using (var dbContext = new ApplicationDbContext(_dbContextOptions))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                var quizzes = await SeedTestDatabaseQuizzes(dbContext, numberOfQuizzes);
                var questions = await SeedTestDatabaseQuestions(dbContext, quizzes);
                await SeedTestDatabaseAnswers(dbContext, questions);
            }
        }

        private async Task<List<Quiz>> SeedTestDatabaseQuizzes(ApplicationDbContext dbContext, int numberOfQuizzes)
        {
            var quizzes = new List<Quiz>();

            for (var index = 0; index < numberOfQuizzes; index++)
            {
                var quiz = new Quiz
                {
                    Name = $"Quiz Number {index + 1}",
                    QuizId = index + 1
                };

                quizzes.Add(quiz);
            }

            await dbContext.Quiz.AddRangeAsync(quizzes);
            await dbContext.SaveChangesAsync();
            return quizzes;
        }

        private async Task<List<Question>> SeedTestDatabaseQuestions(ApplicationDbContext dbContext, List<Quiz> quizzes)
        {
            var questions = new List<Question>();

            var quizIdCounter = 1;
            foreach (var quiz in quizzes)
                for (var index = 1; index <= QuestionsPerQuiz; index++)
                {
                    var question = new Question
                    {
                        QuestionId = quizIdCounter
                    };
                    question.QuestionText = $"Text of Question {question.QuestionId}";
                    question.QuizId = quiz.QuizId;

                    questions.Add(question);

                    quizIdCounter += 1;
                }

            await dbContext.Question.AddRangeAsync(questions);
            await dbContext.SaveChangesAsync();
            return questions;
        }

        private static async Task SeedTestDatabaseAnswers(ApplicationDbContext dbContext, List<Question> questions)
        {
            var answerOptions = new List<AnswerOption>();

            var answerIdCounter = 1;
            foreach (var question in questions)
                for (var index = 1; index <= AnswersPerQuestion; index++)
                {
                    var answerOption = new AnswerOption { AnswerOptionId = answerIdCounter };

                    answerOption.AnswerText = $"Text of Answer {answerOption.AnswerOptionId}";
                    answerOption.Correct = index % AnswersPerQuestion == 0;
                    answerOption.QuestionId = question.QuestionId;

                    answerOptions.Add(answerOption);

                    answerIdCounter += 1;
                }

            await dbContext.AnswerOption.AddRangeAsync(answerOptions);
            await dbContext.SaveChangesAsync();
        }

        #endregion
    }
}