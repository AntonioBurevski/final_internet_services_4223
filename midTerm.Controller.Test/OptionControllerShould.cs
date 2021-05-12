using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midTerm.Controller.Test
{
    public class OptionControllerShould
    {
        private readonly Mock<IOptionService> _mockService;
        private readonly OptionsController _controller;

        public OptionControllerShould()
        {
            _mockService = new Mock<IOptionService>();

            _controller = new OptionsController(_mockService.Object);
        }

        [Fact]
        public async Task ReturnNoContentWhenHasNoData()
        {
            // Arrange
            int expectedId = 1;
            var option = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1);


            _mockService.Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync(option.Find(x => x.Id == expectedId))
                .Verifiable();

            // Act

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnOptionWhenHasData()
        {
            // Arrange
            int expectedCount = 10;
            var option = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate(expectedCount);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(option)
                .Verifiable();

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<OptionModelBase>>().Subject.Count().Should().Be(expectedCount);
        }

        [Fact]
        public async Task ReturnEmptyListWhenHasNoData()
        {
            // Arrange
            var option = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate(expectedCount)
                .Generate(0);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(option)
                .Verifiable();

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnCreatedOptionOnCreateWhenCorrectModel()
        {
            // Arrange
            var option = new Faker<OptionCreateModel>()
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();
            var expected = new Faker<PlayerModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<CreatedResult>();

            var model = result as CreatedResult;
            model?.Value.Should().Be(1);
            model?.Location.Should().Be("/api/Options/1");
        }

        [Fact]
        public async Task ReturnConflictOnCreateWhenRepositoryError()
        {
            // Arrange
            var option = new Faker<OptionCreateModel>()
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var option = new Faker<OptionCreateModel>()
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();
            var expected = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();
            var expected = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Put(option.Id, option);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ReturnOptionOnUpdateWhenCorrectModel()
        {
            // Arrange
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();
            var expected = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Put(option.Id, option);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);
        }

        [Fact]
        public async Task ReturnNoContentOnUpdateWhenRepositoryError()
        {
            // Arrange
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();
            var expected = new Faker<OptionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Option 1")
                .RuleFor(s => s.Order, v => 1)
                .RuleFor(s => s.QuestionId, v => 1)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var result = await _controller.Put(option.Id, option);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnOkWhenDeletedData()
        {
            // Arrange
            int id = 1;
            bool expected = true;

            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);
        }

        [Fact]
        public async Task ReturnOkFalseWhenNoData()
        {
            // Arrange
            int id = 1;
            bool expected = false;

            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);
        }

        [Fact]
        public async Task ReturnBadResultWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}

