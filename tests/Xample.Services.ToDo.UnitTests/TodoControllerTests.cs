using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xample.Services.Todo.Api;
using Xample.Services.ToDo.Api.Controllers;
using Xample.Services.ToDo.Business.Interfaces;
using Xunit;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;
using ApiModels = Xample.Services.Todo.Api.ApiModels;

namespace Xample.Services.ToDo.UnitTests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _todoServiceMock;
        private readonly IMapper _mapper;

        public TodoControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            _mapper = config.CreateMapper();

            _todoServiceMock = new Mock<ITodoService>();
        }

        #region GetAll
        [Fact]
        public async Task GetAll_ReturnsOkAndToDoList_WhenServiceReturnsToDoList()
        {
            // Arrange

            var mockServiceResponse = new List<BusinessModels.Todo>()
            {
                new BusinessModels.Todo()
                {
                    Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                    Name = "ToDo1"
                },
                new  BusinessModels.Todo()
                {
                    Id = Guid.Parse("a7f01520-537d-47fb-afbf-796607e95701"),
                    Name = "ToDo2"
                }
            };

            _todoServiceMock.Setup(s => s.GetAllTodos()).ReturnsAsync(mockServiceResponse);

            var unitUnderTest = new ToDoController(_mapper, _todoServiceMock.Object);

            var expectedControllerResponse = new List<ApiModels.TodoModel>()
            {
                new ApiModels.TodoModel()
                {
                    Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                    Name = "ToDo1"
                },
                new  ApiModels.TodoModel()
                {
                    Id = Guid.Parse("a7f01520-537d-47fb-afbf-796607e95701"),
                    Name = "ToDo2"
                }
            };

            // Act
            var response = await unitUnderTest.GetAll();

            // Assert
            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBe(null);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var todoReturned = okObjectResult.Value as List<ApiModels.TodoModel>;
            todoReturned.Should().BeEquivalentTo(expectedControllerResponse);
        }
        #endregion

        #region Get
        [Fact]
        public async Task Get_ReturnsOkAndToDo_WhenServiceReturnsToDo()
        {
            // Arrange
            var mockServiceResponse = new BusinessModels.Todo()
            {
                Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                Name = "ToDo1"
            };

            _todoServiceMock.Setup(s => s.GetTodo(It.IsAny<Guid>())).ReturnsAsync(mockServiceResponse); // We dont care to specify Id, we don't test the logic of the service/repository in controller tests.

            var unitUnderTest = new ToDoController(_mapper, _todoServiceMock.Object);

            var expectedControllerResponse = new ApiModels.TodoModel()
            {
                Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                Name = "ToDo1"
            };

            // Act
            var response = await unitUnderTest.Get(Guid.NewGuid()); // We dont care to specify Id, we don't test the logic of the service/repository in controller tests.

            // Assert
            var okObjectResult = response as OkObjectResult;
            okObjectResult.Should().NotBe(null);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var todoReturned = okObjectResult.Value as ApiModels.TodoModel;
            todoReturned.Should().BeEquivalentTo(expectedControllerResponse);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenServiceReturnsNull()
        {
            // Arrange
       

            _todoServiceMock.Setup(s => s.GetTodo(It.IsAny<Guid>())).ReturnsAsync((BusinessModels.Todo)null);

            var unitUnderTest = new ToDoController(_mapper, _todoServiceMock.Object);

            var expectedControllerResponse = new ApiModels.TodoModel()
            {
                Id = Guid.Parse("e0f36c5f-7b2d-45bd-a646-8bb1198879ae"),
                Name = "ToDo1"
            };

            // Act
            var response = await unitUnderTest.Get(Guid.NewGuid());

            // Assert
            var notFoundResult = response as NotFoundResult;
            notFoundResult.Should().NotBe(null);
            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        #endregion
    }
}
