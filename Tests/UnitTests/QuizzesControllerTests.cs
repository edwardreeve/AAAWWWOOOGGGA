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

            await SeedTestDatabase(2);
        }

        [Test]
        public async Task Index_returns_view_result()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var sut = new QuizzesController(context);

                var actionResult = await sut.Index();
                actionResult.Should().BeOfType(typeof(ViewResult));
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

        #region Create and Seed Test Database Methods
        private static DbConnection CreateTestingDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => _dbConnection.Dispose();

        private async Task SeedTestDatabase(int numberOfQuizzes)
        {
            using (var dbContext = new ApplicationDbContext(_dbContextOptions))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                 await SeedTestDatabaseQuizzes(dbContext, numberOfQuizzes);

                 await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedTestDatabaseQuizzes(ApplicationDbContext dbContext, int numberOfQuizzes)
        {
            var quizzes = new List<Quiz>();

            for (var index = 0; index < numberOfQuizzes; index++)
            {
                var quiz = new Quiz();
                quiz.Name = $"Quiz Number {index + 1}";
                quiz.QuizId = index + 1;

                quizzes.Add(quiz);
            }

            await dbContext.Quiz.AddRangeAsync(quizzes);
        }
        #endregion
    }
}
