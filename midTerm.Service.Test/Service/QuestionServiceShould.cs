using AutoMapper;
using midTerm.Service.Test.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Service.Test.Service
{
    public class QuestionServiceShould
    : SqlLiteContext

    {
        private readonly IMapper _mapper;
        private readonly QuestionService _service;

        public QuestionServiceShould()
        : base(true)
        {
            if (_mapper == null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(QuestionProfile));
                }).CreateMapper();
                _mapper = mapper;
            }
            _service = new QuestionService(DbContext, _mapper);
        }

        [Fact]
        public async Task GetQuestions()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.Get(expected);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Models.Models.Question.QuestionModelExtended>();
            result.Id.Should().Be(expected);
        }

        [Fact]
        public async Task GetQuestionById()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.Get(expected);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Models.Models.Question.QuestionModelExtended>();
            result.Id.Should().Be(expected);
        }

        [Fact]
        public async Task InsertNewQuestion()
        {
            // Arrange
            var question = new QuestionCreateModel
            {
                Id = 1,
                Text = "Question 1",
                Description = "Description 1"
            };

            // Act
            var result = await _service.Insert(question);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task UpdateOption()
        {
            // Arrange
            var question = new QuestionUpdateModel
            {
                Id = 1,
                Text = "Question 1",
                Description = "Description 1"
            };

            // Act
            var result = await _service.Update(question);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().Be(question.Id);
            result.Text.Should().Be(question.Text);
            result.Description.Should().Be(question.Description);

        }

        [Fact]
        public async Task ThrowExceptionOnUpdateQuestion()
        {
            // Arrange
            var question = new QuestionUpdateModel
            {
                Id = 12,
                Text = "Question 12",
                Description = "Description 12"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Update(question));
            Assert.Equal("Question not found", ex.Message);

        }

        [Fact]
        public async Task DeleteOption()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.Delete(expected);
            var question = await _service.Get(expected);

            // Assert
            result.Should().Be(true);
            question.Should().BeNull();
        }

    }
}
