using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MVC_ASP.NET_Core_Learn.Controllers;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Diagnostics;
using Xunit;

namespace MVC_ASP.NET_CoreLearn.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResult_With_ErrorViewModel()
        {
            // Arrange
            var activity = new Activity("testActivity");
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.TraceIdentifier).Returns("testTraceIdentifier");

            var controller = new HomeController(new NullLogger<HomeController>());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext.Object
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.ViewData.Model);
            Assert.Equal("testTraceIdentifier", model.RequestId);
        }

        [Fact]
        public void PageNotFound_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.PageNotFound();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}