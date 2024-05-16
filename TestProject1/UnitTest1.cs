using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalBrand.API.Controllers;
using PersonalBrand.Application.UseCases.IdentitieCases.Commands;
using PersonalBrand.Application.UseCases.IdentitieCases.Queries;
using PersonalBrand.Domain.Entities;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new IdentityController(mockMediator.Object);
            var createUserCommand = new CreateUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                Password = "password",
                PhoneNumber = "123456789"
            };

            mockMediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default))
                .ReturnsAsync(new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Created"
                });

            // Act
            var result = await controller.Rergister(createUserCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(201, response.StatusCode);
            Assert.Equal("Created", response.Message);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new IdentityController(mockMediator.Object);
            var users = new List<UserModel>
            {
                new UserModel { FirstName = "John", LastName = "Doe", Email = "john@example.com", UserName = "johndoe" },
                new UserModel { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", UserName = "janesmith" }
            };

            mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), default))
                .ReturnsAsync(users);

            // Act
            var result = await controller.GetAllUers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserModel>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count());
            Assert.Contains(users[0], returnedUsers);
            Assert.Contains(users[1], returnedUsers);
        }

        [Fact]
        public async Task Update_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new IdentityController(mockMediator.Object);
            var updateUserCommand = new UpdateUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "123456789"
            };

            mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
                .ReturnsAsync(new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 204,
                    Message = "Updated"
                });

            // Act
            var result = await controller.Update(updateUserCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal("Updated", response.Message);
        }
    }
}