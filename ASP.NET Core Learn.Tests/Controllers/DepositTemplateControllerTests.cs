using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC_ASP.NET_Core_Learn.Controllers;
using MVC_ASP.NET_Core_Learn.Data.Enums;
using MVC_ASP.NET_Core_Learn.Data.Interfaces;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;

namespace MVC_ASP.NET_CoreLearn.Tests.Controllers
{
    public class DepositTemplateControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewWithDeposits()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var expectedDeposits = new List<DepositTemplate>
            {
                new DepositTemplate { Id = 1, Title = "Deposit 1" },
                new DepositTemplate { Id = 2, Title = "Deposit 2" }
            };
            depositRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedDeposits);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<DepositTemplate>>(viewResult.Model);
            Assert.Equal(expectedDeposits, model);
        }

        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result); // Ensure that a view is returned
            Assert.IsType<EditDepositViewModel>(result.Model); // Ensure that the correct model type is passed to the view
            var model = Assert.IsAssignableFrom<EditDepositViewModel>(result.Model);
            Assert.Equal(0, model.Id); // Ensure that the model properties are correctly initialized
                                       // Add assertions for other properties if needed
        }

        [Fact]
        public void Create_POST_ReturnsRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var depositVM = new EditDepositViewModel
            {
                Title = "Test Deposit",
                ShortDescription = "Test Description",
                Replenishment = true,
                InterestRate = InterestPayment.Monthly,
                Terms = new List<DepositTerm>(),
                InterestRateNoEarlyClosure = 5.5,
                InterestRateEarlyClosure = 6.5
            };

            // Act
            var result = controller.Create(depositVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_POST_ReturnsViewWithModelError_WhenModelStateIsInvalid()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var depositVM = new EditDepositViewModel
            {
                // Initialize ViewModel properties here
            };
            controller.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = controller.Create(depositVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(depositVM, viewResult.Model); // Ensure the model is passed back to the view
        }

        [Fact]
        public async Task Edit_ReturnsErrorView_WhenDepositNotFound()
        {
            // Arrange
            int depositId = 1;
            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(depositId)).ReturnsAsync((DepositTemplate)null);
            var controller = new DepositTemplateController(depositRepositoryMock.Object);

            // Act
            var result = await controller.Edit(depositId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task Edit_ReturnsEditViewWithDepositViewModel_WhenDepositFound()
        {
            // Arrange
            int depositId = 1;
            var deposit = new DepositTemplate
            {
                Id = depositId,
                Title = "Test Deposit",
                ShortDescription = "Test Description",
                Replenishment = true,
                InterestPayment = InterestPayment.Monthly,
                Terms = new List<DepositTerm>(),
                InterestRateNoEarlyClosure = 5.5,
                InterestRateEarlyClosure = 6.5
            };
            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(depositId)).ReturnsAsync(deposit);
            var controller = new DepositTemplateController(depositRepositoryMock.Object);

            // Act
            var result = await controller.Edit(depositId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditDepositViewModel>(viewResult.Model);
            Assert.Equal(deposit.Id, model.Id);
            Assert.Equal(deposit.Title, model.Title);
            Assert.Equal(deposit.ShortDescription, model.ShortDescription);
            Assert.Equal(deposit.Replenishment, model.Replenishment);
            Assert.Equal(deposit.InterestPayment, model.InterestRate);
            Assert.Equal(deposit.InterestRateNoEarlyClosure, model.InterestRateNoEarlyClosure);
            Assert.Equal(deposit.InterestRateEarlyClosure, model.InterestRateEarlyClosure);
        }
        //[Fact]
        //public async Task Edit_POST_ReturnsRedirectToIndex_WhenModelStateIsValid()
        //{
        //    // Arrange
        //    var depositRepositoryMock = new Mock<IDepositRepository>();
        //    var controller = new DepositTemplateController(depositRepositoryMock.Object);
        //    var depositVM = new EditDepositViewModel
        //    {
        //        Id = 1, // Assuming the ID is valid
        //        Title = "Updated Test Deposit",
        //        ShortDescription = "Updated Test Description",
        //        Replenishment = true,
        //        InterestRate = InterestPayment.Monthly,
        //        Terms = new List<DepositTerm>(),
        //        InterestRateNoEarlyClosure = 5.5,
        //        InterestRateEarlyClosure = 6.5
        //    };

        //    // Act
        //    var result = await controller.Edit(depositVM.Id, depositVM);

        //    // Assert
        //    var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.Equal("Index", redirectToActionResult.ActionName);
        //}

        [Fact]
        public async Task Edit_POST_ReturnsViewWithModelError_WhenModelStateIsInvalid()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var depositVM = new EditDepositViewModel
            {
                Id = 1, // Assuming the ID is valid
                // Set other properties accordingly for invalid model state
            };
            controller.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = await controller.Edit(depositVM.Id, depositVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Edit", viewResult.ViewName); // Ensure the correct view is returned
            Assert.Equal(depositVM, viewResult.Model); // Ensure the model is passed back to the view
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenDepositExists()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var depositId = 1; // Assuming the ID is valid
            var deposit = new DepositTemplate { Id = depositId }; // Assuming the deposit exists in the repository
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(depositId)).ReturnsAsync(deposit);

            // Act
            var result = await controller.Delete(depositId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenDepositDoesNotExist()
        {
            // Arrange
            var depositRepositoryMock = new Mock<IDepositRepository>();
            var controller = new DepositTemplateController(depositRepositoryMock.Object);
            var depositId = 1; // Assuming the ID is valid
            depositRepositoryMock.Setup(repo => repo.GetByIdAsync(depositId)).ReturnsAsync((DepositTemplate)null); // Simulating the deposit not found in the repository

            // Act
            var result = await controller.Delete(depositId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
