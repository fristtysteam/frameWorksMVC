using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcGame.Controllers;
using MvcGame.Data;
using MvcGame.Models;
using Xunit;
using Moq;

namespace MvcGame.Tests
///used this website to teach me how to xUnit test https://medium.com/bina-nusantara-it-division/a-comprehensive-guide-to-implementing-xunit-tests-in-c-net-b2eea43b48b
{
    public class GamesControllerTests
    {
        [Fact]
        public async Task IndexReturnsAViewResultWithAGameList()
        {
            // Arrange
            var mockContext = new Mock<MvcGameContext>();
            var gamesController = new GamesController(mockContext.Object);

            // Act
            var result = await gamesController.Index(null, null) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<GameGenreViewModel>(result.Model);
        }

        [Fact]
        public async Task DetailsReturnsAViewResult_WithGameDetails()
        {
            // Arrange
            var mockContext = new Mock<MvcGameContext>();
            mockContext.Setup(c => c.Game.FindAsync(It.IsAny<int>())).ReturnsAsync(new Game());
            var gamesController = new GamesController(mockContext.Object);

            // Act
            var result = await gamesController.Details(1) as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Game>(result.Model);
        }

        [Fact]
        public async Task EditReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = new Mock<MvcGameContext>();
            var gamesController = new GamesController(mockContext.Object);

            // Act
            var result = await gamesController.Edit(null) as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
