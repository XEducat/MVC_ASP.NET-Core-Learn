//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using MVC_ASP.NET_Core_Learn.Controllers;
//using MVC_ASP.NET_Core_Learn.Models;
//using MVC_ASP.NET_Core_Learn.ViewModels;
//using System.Security.Claims;

//namespace MVC_ASP.NET_CoreLearn.Tests.Controllers
//{
//    public class UserControllerTests
//    {
//        [Fact]
//        public async Task About_ReturnsViewResult_WithUserDetailViewModel_ForAuthenticatedUser()
//        {
//            // Arrange
//            var userManagerMock = new Mock<UserManager<AppUser>>(
//                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
//            var user = new AppUser { Id = "testUserId", Email = "test@example.com", UserName = "testuser" };
//            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
//                           .ReturnsAsync(user);
//            var controller = new UserController(userManagerMock.Object);

//            // Act
//            var result = await controller.About();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<UserDetailViewModel>(viewResult.Model);
//            Assert.Equal(user.Id, model.Id);
//            Assert.Equal(user.Email, model.Email);
//            Assert.Equal(user.UserName, model.UserName);
//        }

//        [Fact]
//        public async Task About_ReturnsErrorView_WhenUserIsNull()
//        {
//            // Arrange
//            var userManagerMock = new Mock<UserManager<AppUser>>(
//                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
//            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
//                           .ReturnsAsync((AppUser)null);
//            var controller = new UserController(userManagerMock.Object);

//            // Act
//            var result = await controller.About();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal("Error", viewResult.ViewName);
//        }
//    }
//}