using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xample.Services.ToDo.Business;
using Xample.Services.ToDo.Business.Interfaces;
using Xunit;
using BusinessModels = Xample.Services.ToDo.Business.BusinessModels;

namespace Xample.Services.ToDo.UnitTests
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;

        public TodoServiceTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
        }

        [Fact]
        public async Task GetAllTodos_ReturnsTodosList_WhenRepositoryReturnsTodosList()
        {
            // Arrange
            var mockRepositoryResponse = new List<BusinessModels.Todo>()
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

            _todoRepositoryMock.Setup(s => s.GetAllTodos()).ReturnsAsync(mockRepositoryResponse);

            var expectedServiceResponse = new List<BusinessModels.Todo>()
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

            var unitUnderTest = new TodoService(_todoRepositoryMock.Object);

            // Act 
            var response = await unitUnderTest.GetAllTodos();

            //Assert
            response.Should().BeEquivalentTo(expectedServiceResponse);
        }
    }
}
