using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MVC_ASP.NET_Core_Learn.Controllers;
using MVC_ASP.NET_Core_Learn.Models;
using MVC_ASP.NET_Core_Learn.ViewModels;
using System.Security.Claims;

namespace MVC_ASP.NET_CoreLearn.Tests.Controllers
{
    // TODO: Оптимизировать тесты
    public class AccountControllerTests
    {

        [Fact]
        public async Task Login_ReturnsViewResult_With_LoginViewModel()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<AppUser>>(
                            new Mock<IUserStore<AppUser>>().Object,
                            new Mock<IOptions<IdentityOptions>>().Object,
                            new Mock<IPasswordHasher<AppUser>>().Object,
                            new IUserValidator<AppUser>[0],
                            new IPasswordValidator<AppUser>[0],
                            new Mock<ILookupNormalizer>().Object,
                            new Mock<IdentityErrorDescriber>().Object,
                            new Mock<IServiceProvider>().Object,
                            new Mock<ILogger<UserManager<AppUser>>>().Object);
            var signInManagerMock = new Mock<SignInManager<AppUser>>(userManagerMock.Object,
                                                                        Mock.Of<IHttpContextAccessor>(),
                                                                        Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                                                                        null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);
            var model = new LoginViewModel { EmailAddress = "test@example.com", Password = "password" };

            // Act
            var result = await controller.Login(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
            Assert.IsType<AccountViewModel>(result.Model);
            var accountViewModel = Assert.IsType<AccountViewModel>(result.Model);
            Assert.Equal(model, accountViewModel.LoginViewModel);
        }

        [Fact]
        public async Task Register_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);
            var invalidModel = new RegisterViewModel();
            controller.ModelState.AddModelError("FieldName", "Error Message");

            // Act
            var result = await controller.Register(invalidModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<AccountViewModel>(viewResult.Model);
            Assert.Equal(invalidModel, viewModel.RegisterViewModel);
        }

        [Fact]
        public async Task LogOut_RedirectsToHomeControllerIndexAction()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<AppUser>>(
                            new Mock<IUserStore<AppUser>>().Object,
                            new Mock<IOptions<IdentityOptions>>().Object,
                            new Mock<IPasswordHasher<AppUser>>().Object,
                            new IUserValidator<AppUser>[0],
                            new IPasswordValidator<AppUser>[0],
                            new Mock<ILookupNormalizer>().Object,
                            new Mock<IdentityErrorDescriber>().Object,
                            new Mock<IServiceProvider>().Object,
                            new Mock<ILogger<UserManager<AppUser>>>().Object);
            var signInManagerMock = new Mock<SignInManager<AppUser>>(userManagerMock.Object,
                                                                        Mock.Of<IHttpContextAccessor>(),
                                                                        Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                                                                        null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = await controller.LogOut() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var user = new AppUser { UserName = "testuser", Email = "test@example.com" };
            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
            var signInManagerMock = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object);

            // Act
            var result = await controller.Delete() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Home", result.ControllerName);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
