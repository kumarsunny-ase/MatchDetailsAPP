using System;
using MatchDetailsApp.Controllers;
using MatchDetailsApp.Models.DTOs;
using MatchDetailsApp.Repositories;
using MatchDetailsApp.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace MatchDetailsApp.Tests
{
	public class AuthControllerTests
	{
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<TokenRepository> _mockTokenRepository;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var userConfirmation = new Mock<IUserConfirmation<IdentityUser>>();

            // Create UserManager mock
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                userConfirmation.Object,
                null,
                null);

            // Mock TokenRepository
            _mockTokenRepository = new Mock<TokenRepository>();

            // Instantiate controller with mocked UserManager and TokenRepository
            _controller = new AuthController(_mockUserManager.Object, _mockTokenRepository.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Test@123";
            var user = new IdentityUser { UserName = email, Email = email };
            var roles = new List<string> { "User" };
            var token = "fake-jwt-token";

            _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, password)).ReturnsAsync(true);
            _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockTokenRepository.Setup(x => x.CreateJwtToken(user, roles)).Returns(token);

            var request = new LoginRequestDto { Email = email, Password = password };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var response = actionResult.Value as LoginResponseDto;
            Assert.NotNull(response);
            Assert.Equal(email, response.Email);
            Assert.Equal(roles, response.Roles);
            Assert.Equal(token, response.Token);
        }

        [Fact]
        public async Task Register_ValidDetails_ReturnsOkResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Test@123";
            var user = new IdentityUser { UserName = email, Email = email };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "User")).ReturnsAsync(IdentityResult.Success);

            var request = new RegisterRequestDto { Email = email, Password = password };

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_InvalidDetails_ReturnsValidationProblem()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Test@123";
            var user = new IdentityUser { UserName = email, Email = email };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Invalid details" });

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), password)).ReturnsAsync(identityResult);

            var request = new RegisterRequestDto { Email = email, Password = password };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Invalid details", ((ValidationProblemDetails)actionResult.Value).Errors[string.Empty].First());
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsValidationProblem()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword";
            var user = new IdentityUser { UserName = email, Email = email };

            _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, password)).ReturnsAsync(false);

            var request = new LoginRequestDto { Email = email, Password = password };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Email or Password Incorrect", ((ValidationProblemDetails)actionResult.Value).Errors[string.Empty].First());
        }
    }
}

