using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class SurveyFeatureToSurveyFeatureDtoConverterTests
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

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectAngleToXAxisAtEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(90.0,
                            actual.AngleToXAxisAtEndPoint);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectAngleToXAxisAtStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(45.0,
                            actual.AngleToXAxisAtStartPoint);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(11.0,
                            actual.EndPoint.X);
            Assert.AreEqual(22.0,
                            actual.EndPoint.Y);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectId_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(1,
                            actual.Id);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectLength_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(14.14,
                            actual.Length);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectRunDirection_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual("Forward",
                            actual.RunDirection);
        }

        [Test]
        public void ConvertToDto_ReturnsDtoWithCorrectStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            SurveyFeatureDto actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToDto(m_Feature);

            // Assert
            Assert.AreEqual(1.0,
                            actual.StartPoint.X);
            Assert.AreEqual(2.0,
                            actual.StartPoint.Y);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectAngleToXAxisAtEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(Angle.For90Degrees,
                            actual.AngleToXAxisAtEndPoint);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectAngleToXAxisAtStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(Angle.For45Degrees,
                            actual.AngleToXAxisAtStartPoint);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectEndPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(11.0,
                            actual.EndPoint.X);
            Assert.AreEqual(22.0,
                            actual.EndPoint.Y);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectId_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(1,
                            actual.Id);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectLength_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(14.14,
                            actual.Length);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectRunDirection_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(Constants.LineDirection.Forward,
                            actual.RunDirection);
        }

        [Test]
        public void ConvertToSurveyFeature_ReturnsDtoWithCorrectStartPoint_WhenCalled()
        {
            // Arrange
            // Act
            ISurveyFeature actual = SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature(m_Dto);

            // Assert
            Assert.AreEqual(1.0,
                            actual.StartPoint.X);
            Assert.AreEqual(2.0,
                            actual.StartPoint.Y);
        }
    }
}