using midTerm.Models.Profiles;
using midTerm.Models.Test.Internal;
using Xunit;

namespace midTerm.Models.Test
{
    public class AutoMapperConfigurationIsValid
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            // Arrange
            var configuration = AutoMapperModule.CreateMapperConfiguration<QuestionProfile>();

            // Act/Assert
            configuration.AssertConfigurationIsValid();
        }
    }
}
