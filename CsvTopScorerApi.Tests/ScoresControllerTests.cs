using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopScorersApp.Controllers;
using CsvTopScorerApi.Data;
using CsvTopScorerApi.Models;
using Xunit;

namespace CsvTopScorerApi.Tests
{
    public class ScoresControllerTests
    {
        private readonly ScoresController _controller;
        private readonly ScoreContext _context;

        public ScoresControllerTests()
        {
            // Create an in-memory database context for testing
            var options = new DbContextOptionsBuilder<ScoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ScoreContext(options);
            _controller = new ScoresController(_context);

            // Seed the in-memory database with initial data
            _context.Scores.AddRange(new List<Score>
            {
                new Score { Id = 1, FirstName = "John", SecondName = "Doe", ScoreValue = 75 },
                new Score { Id = 2, FirstName = "Jane", SecondName = "Smith", ScoreValue = 85 },
                new Score { Id = 3, FirstName = "Alice", SecondName = "Johnson", ScoreValue = 85 }
            });
            _context.SaveChanges();
        }

        [Fact]
        public void AddScore_ShouldReturnCreatedResult()
        {
            // Arrange
            var newScore = new Score { FirstName = "Bob", SecondName = "Builder", ScoreValue = 90 };

            // Act
            var result = _controller.AddScore(newScore) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(newScore, result.Value);
        }

        [Fact]
        public void GetScore_ValidName_ShouldReturnOkResult()
        {
            // Act
            var result = _controller.GetScore("John", "Doe") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Score>(result.Value);
            Assert.Equal(75, ((Score)result.Value).ScoreValue);
        }

        [Fact]
        public void GetScore_InvalidName_ShouldReturnNotFound()
        {
            // Act
            var result = _controller.GetScore("Non", "Existing") as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetTopScorers_ShouldReturnTopScorers()
        {
            // Act
            var result = _controller.GetTopScorers() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var topScorers = Assert.IsType<List<Score>>(result.Value);
            Assert.Equal(2, topScorers.Count); // Expecting Jane Smith and Alice Johnson
            Assert.All(topScorers, s => Assert.Equal(85, s.ScoreValue)); // Check score value
        }
    }
}
