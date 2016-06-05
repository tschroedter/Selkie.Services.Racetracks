using System.Diagnostics.CodeAnalysis;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;
using Xunit;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class SurveyFeatureToSurveyFeatureDtoConverterTests
    {
        public SurveyFeatureToSurveyFeatureDtoConverterTests()
        {
            var featureStartPoint = new Point(1.0,
                                              2.0);

            var featureEndPoint = new Point(11.0,
                                            22.0);

            m_Feature = new SurveyFeature(1,
                                          featureStartPoint,
                                          featureEndPoint,
                                          Angle.For45Degrees,
                                          Angle.For90Degrees,
                                          Constants.LineDirection.Forward,
                                          14.14);

            var startPoint = new PointDto
                             {
                                 X = 1.0,
                                 Y = 2.0
                             };

            var endPoint = new PointDto
                           {
                               X = 11.0,
                               Y = 22.0
                           };

            m_Dto = new SurveyFeatureDto
                    {
                        RunDirection = Constants.LineDirection.Forward.ToString(),
                        AngleToXAxisAtEndPoint = 90.0,
                        AngleToXAxisAtStartPoint = 45.0,
                        EndPoint = endPoint,
                        Id = 1,
                        IsUnknown = false,
                        Length = 14.14,
                        StartPoint = startPoint
                    };
        }

        private readonly SurveyFeatureDto m_Dto;
        private readonly SurveyFeature m_Feature;

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectAngleToXAxisAtEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(90.0,
                         actual.AngleToXAxisAtEndPoint);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectAngleToXAxisAtStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(45.0,
                         actual.AngleToXAxisAtStartPoint);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(11.0,
                         actual.EndPoint.X);
            Assert.Equal(22.0,
                         actual.EndPoint.Y);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectId_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(1,
                         actual.Id);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectLength_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(14.14,
                         actual.Length);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectRunDirection_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal("Forward",
                         actual.RunDirection);
        }

        [Fact]
        public void ConvertToDto_ReturnsDtoWithCorrectStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.Equal(1.0,
                         actual.StartPoint.X);
            Assert.Equal(2.0,
                         actual.StartPoint.Y);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectAngleToXAxisAtEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(Angle.For90Degrees,
                         actual.AngleToXAxisAtEndPoint);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectAngleToXAxisAtStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(Angle.For45Degrees,
                         actual.AngleToXAxisAtStartPoint);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(11.0,
                         actual.EndPoint.X);
            Assert.Equal(22.0,
                         actual.EndPoint.Y);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectId_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(1,
                         actual.Id);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectLength_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(14.14,
                         actual.Length);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectRunDirection_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(Constants.LineDirection.Forward,
                         actual.RunDirection);
        }

        [Fact]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.Equal(1.0,
                         actual.StartPoint.X);
            Assert.Equal(2.0,
                         actual.StartPoint.Y);
        }
    }
}