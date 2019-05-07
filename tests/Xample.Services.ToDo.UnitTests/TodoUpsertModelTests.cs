using FluentAssertions;
using Xample.Services.Todo.Api.ApiModels;
using Xample.Services.ToDo.UnitTests.Helpers;
using Xunit;

namespace Xample.Services.ToDo.UnitTests
{
    public class TodoUpsertModelTests : ModelValidationHelper
    {
        #region Name
        [Fact]
        public void Name_ShouldPassValidation_WhenBetween1to100Characters()
        {
            // Arrange
            var testModel = new TodoUpsertModel()
            {
                Name = "Valid Name"
            };

            // Act
            var validationResults = Validate(testModel);

            // Assert
            validationResults.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Name_ShouldFailValidation_WhenNullOrEmptyAndThusLessThan1Character(string name)
        {
            // Arrange
            var testModel = new TodoUpsertModel()
            {
                Name = name
            };

            // Act
            var validationResults = Validate(testModel);

            // Assert
            validationResults.Count.Should().BeGreaterThan(0);
            validationResults[0].ErrorMessage.Should().BeEquivalentTo("The Name field is required.");
        }

        [Fact]
        public void Name_ShouldFailValidation_WhenMoreThan100Characters()
        {
            // Arrange
            var testModel = new TodoUpsertModel()
            {
                Name = new string('c', 101)
            };

            // Act
            var validationResults = Validate(testModel);

            // Assert
            validationResults.Count.Should().BeGreaterThan(0);
            validationResults[0].ErrorMessage.Should()
                .BeEquivalentTo(
                    "The field Name must be a string with a minimum length of 1 and a maximum length of 100.");
        }
        #endregion
    }
}

