using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MVC_ASP.NET_Core_Learn.Controllers;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Security.Claims;


namespace MVC_ASP.NET_CoreLearn.Tests.Controllers
{
    public class UserDepositControllerTests
    {
        [Fact]
        public async Task OrderForm_ReturnsErrorView_WhenDepositNotFound()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DepositTemplate)null);

            var controller = new UserDepositController(depositRepositoryMock.Object, null, null);

            // Act
            var result = await controller.OrderForm(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Contains("Депозит не знайдено", model.Errors);
        }

        [Fact]
        public async Task OrderForm_ReturnsViewWithCorrectViewModel_WhenDepositFound()
        {
            // Arrange
            var deposit = new DepositTemplate
            {
                Id = 1,
                Title = "Test Deposit"
            };

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(deposit);

            var controller = new UserDepositController(depositRepositoryMock.Object, null, null);

            // Act
            var result = await controller.OrderForm(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDepositViewModel>(viewResult.Model);
            Assert.Equal(deposit.Id, model.DepositId);
            Assert.Equal(deposit.Title, model.Title);
        }

        [Fact]
        public async Task All_ReturnsViewWithUserDeposits()
        {
            // Arrange
            var currentUser = new AppUser { Id = "testUserId" };
            var userDeposits = new List<UserDeposit> { new UserDeposit { Id = 1, Title = "Test Deposit" } };

            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var user = new AppUser { Id = "testUserId", Email = "test@example.com", UserName = "testuser" };
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(currentUser);

            var userDepositRepositoryMock = new Mock<IUserDepositRepository>();
            userDepositRepositoryMock.Setup(repo => repo.GetDepositsByUserIdAsync(currentUser.Id))
                .ReturnsAsync(userDeposits);

            var controller = new UserDepositController(null, userManagerMock.Object, userDepositRepositoryMock.Object);

            // Act
            var result = await controller.All();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserDeposit>>(viewResult.Model);
            Assert.Equal(userDeposits, model);
        }

        // Тест метода ReplenishmentDeposit
        [Fact]
        public async Task ReplenishmentDeposit_ReturnsViewWithCorrectData()
        {
            // Arrange
            int depositId = 1;
            var selectedUserDeposit = new UserDeposit
            {
                Id = depositId,
                Title = "Test Deposit"
            };
            var userDepositRepositoryMock = new Mock<IUserDepositRepository>();
            userDepositRepositoryMock.Setup(repo => repo.GetByIdAsync(depositId)).ReturnsAsync(selectedUserDeposit);
            var controller = new UserDepositController(null, null, userDepositRepositoryMock.Object);

            // Act
            var result = await controller.ReplenishmentDeposit(depositId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(selectedUserDeposit.Id, viewResult.ViewData["UserDepositId"]);
            Assert.Equal(selectedUserDeposit.Title, viewResult.ViewData["SelectedDepositTitle"]);
        }
        [Fact]
        public async Task ReplenishmentDeposit_Post_ReturnsRedirectToAll_WhenReplenishmentAmountIsPositive()
        {
            // Arrange
            var selectedUserDeposit = new UserDeposit { Id = 1, Title = "Test Deposit" };

            var userDepositRepositoryMock = new Mock<IUserDepositRepository>();
            userDepositRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(selectedUserDeposit);

            var controller = new UserDepositController(null, null, userDepositRepositoryMock.Object);
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await controller.ReplenishmentDeposit(100, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("All", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task ReplenishmentDeposit_Post_ReturnsRedirectToReplenishmentDeposit_WhenReplenishmentAmountIsNotPositive()
        {
            // Arrange
            var controller = new UserDepositController(null, null, null);
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await controller.ReplenishmentDeposit(0, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ReplenishmentDeposit", redirectToActionResult.ActionName);
        }

    }

    public class TestUserManager : UserManager<AppUser>
    {
        public TestUserManager() : base(
            new Mock<IUserStore<AppUser>>().Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null)
        {
        }

        public override Task<AppUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult<AppUser>(null);
        }
    }
}
