using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class FeaturesValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_Sut = new FeaturesValidator();
        }

        private FeaturesValidator m_Sut;

        private IEnumerable <ISurveyFeature> CreateValidFeatures()
        {
            var one = Substitute.For <ISurveyFeature>();
            one.Id.Returns(0);

            var two = Substitute.For <ISurveyFeature>();
            two.Id.Returns(1);

            var three = Substitute.For <ISurveyFeature>();
            three.Id.Returns(2);

            var list = new[]
                       {
                           one,
                           two,
                           three
                       };

            return list;
        }

        private IEnumerable <ISurveyFeature> CreateInvalidFeatures()
        {
            var one = Substitute.For <ISurveyFeature>();
            one.Id.Returns(1);

            var two = Substitute.For <ISurveyFeature>();
            two.Id.Returns(2);

            var three = Substitute.For <ISurveyFeature>();
            three.Id.Returns(3);

            var list = new[]
                       {
                           one,
                           two,
                           three
                       };

            return list;
        }

        private IEnumerable <SurveyFeatureDto> CreateValidDtos()
        {
            var one = new SurveyFeatureDto();
            one.Id = 0;

            var two = new SurveyFeatureDto();
            two.Id = 1;

            var three = new SurveyFeatureDto();
            three.Id = 2;

            var list = new[]
                       {
                           one,
                           two,
                           three
                       };

            return list;
        }

        private IEnumerable <SurveyFeatureDto> CreateInvalidDtos()
        {
            var one = new SurveyFeatureDto();
            one.Id = 1;

            var two = new SurveyFeatureDto();
            two.Id = 2;

            var three = new SurveyFeatureDto();
            three.Id = 3;

            var list = new[]
                       {
                           one,
                           two,
                           three
                       };

            return list;
        }

        [Test]
        public void ValidateDtos_ReturnsFalse_ForEmptyArray()
        {
            // Arrange
            IEnumerable <SurveyFeatureDto> dtos = new SurveyFeatureDto[0];

            // Act
            // Assert
            Assert.False(m_Sut.ValidateDtos(dtos));
        }

        [Test]
        public void ValidateDtos_ReturnsFalse_ForInvalidDtos()
        {
            // Arrange
            IEnumerable <SurveyFeatureDto> dtos = CreateInvalidDtos();

            // Act
            // Assert
            Assert.False(m_Sut.ValidateDtos(dtos));
        }

        [Test]
        public void ValidateDtos_ReturnsTrue_ForValidDtos()
        {
            // Arrange
            IEnumerable <SurveyFeatureDto> dtos = CreateValidDtos();

            // Act
            // Assert
            Assert.True(m_Sut.ValidateDtos(dtos));
        }

        [Test]
        public void ValidateFeatures_ReturnsFalse_ForEmptyArray()
        {
            // Arrange
            IEnumerable <ISurveyFeature> features = new SurveyFeature[0];

            // Act
            // Assert
            Assert.False(m_Sut.ValidateFeatures(features));
        }

        [Test]
        public void ValidateFeatures_ReturnsFalse_ForInvalidFeatures()
        {
            // Arrange
            IEnumerable <ISurveyFeature> features = CreateInvalidFeatures();

            // Act
            // Assert
            Assert.False(m_Sut.ValidateFeatures(features));
        }

        [Test]
        public void ValidateFeatures_ReturnsTrue_ForValidFeatures()
        {
            // Arrange
            IEnumerable <ISurveyFeature> features = CreateValidFeatures();

            // Act
            // Assert
            Assert.True(m_Sut.ValidateFeatures(features));
        }
    }
}