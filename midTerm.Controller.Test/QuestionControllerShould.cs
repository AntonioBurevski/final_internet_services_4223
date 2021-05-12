using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midTerm.Controller.Test
{
    public class QuestionControllerShould
    {
        private readonly Mock<IQuestionService> _mockService;
        private readonly QuestionsController _controller;

        public QuestionControllerShould()
        {
            _mockService = new Mock<IQuestionService>();

            _controller = new QuestionsController(_mockService.Object);
        }

        [Fact]
        public async Task ReturnNoContentWhenHasNoData()
        {
            // Arrange
            int expectedId = 1;
            var question = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1");

            _mockService.Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync(question.Find(x => x.Id == expectedId))
                .Verifiable();

            // Act
            var result = await _controller.GetQuestion(expectedId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnQuestionWhenHasData()
        {
            // Arrange
            int expectedCount = 10;
            var question = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate(expectedCount);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question)
                .Verifiable();

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<QuestionModelBase>>().Subject.Count().Should().Be(expectedCount);
        }

        [Fact]
        public async Task ReturnEmptyListWhenHasNoData()
        {
            // Arrange
            var question = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, v => ++v.IndexVariable)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate(0);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question)
                .Verifiable();

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnCreatedQuestionOnCreateWhenCorrectModel()
        {
            // Arrange
            var question = new Faker<QuestionCreateModel>()
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();
            var expected = new Faker<PlayerModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<CreatedResult>();

            var model = result as CreatedResult;
            model?.Value.Should().Be(1);
            model?.Location.Should().Be("/api/Questions/1");
        }

        [Fact]
        public async Task ReturnConflictOnCreateWhenRepositoryError()
        {
            // Arrange
            var question = new Faker<QuestionCreateModel>()
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionCreateModel>()
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();
            var expected = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();
            var expected = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Put(question.Id, question);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ReturnQuestionOnUpdateWhenCorrectModel()
        {
            // Arrange
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();
            var expected = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected)
                .Verifiable();

            // Act
            var result = await _controller.Put(question.Id, question);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);
        }

        [Fact]
        public async Task ReturnNoContentOnUpdateWhenRepositoryError()
        {
            // Arrange
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();
            var expected = new Faker<QuestionModelBase>()
                .RuleFor(s => s.Id, 1)
                .RuleFor(s => s.Text, v => "Question 1")
                .RuleFor(s => s.Description, v => "Description 1")
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var result = await _controller.Put(question.Id, question);

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

