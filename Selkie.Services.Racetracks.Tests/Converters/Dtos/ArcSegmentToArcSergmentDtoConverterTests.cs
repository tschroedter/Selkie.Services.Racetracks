using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class ArcSegmentToArcSergmentDtoConverterTests
    {
        private static ArcSegmentToArcSegmentDtoConverter CreateSut()
        {
            return new ArcSegmentToArcSegmentDtoConverter(new PointToPointDtoConverter(),
                                                          new CircleToCircleDtoConverter(
                                                              new PointToPointDtoConverter()));
        }

        private void AssertCircleDto(CircleDto actual,
                                     double expectedX,
                                     double expectedY,
                                     double expectedRadius)
        {
            DtoHelper.AssertPointDto(actual.CentrePoint,
                                     expectedX,
                                     expectedY);

            Assert.True(Math.Abs(actual.Radius - expectedRadius) < DtoHelper.Tolerance,
                        "Radius: Expected {0} but actual {1}".Inject(expectedRadius,
                                                                     actual.Radius));
        }

        private IArcSegment CreateArcSegment()
        {
            var circle = new Circle(1.0,
                                    2.0,
                                    3.0);
            var startPoint = new Point(1.0,
                                       5.0);
            var endPoint = new Point(4.0,
                                     2.0);

            var arcSegment = new ArcSegment(circle,
                                            startPoint,
                                            endPoint);

            return arcSegment;
        }

        [Test]
        public void Convert_SetsCircleDto_ForArcSegment()
        {
            // Arrange
            ArcSegmentToArcSegmentDtoConverter sut = CreateSut();
            sut.ArcSegment = CreateArcSegment();

            // Act
            sut.Convert();

            // Assert
            AssertCircleDto(sut.Dto.Circle,
                            1.0,
                            2.0,
                            3.0);
        }

        [Test]
        public void Convert_SetsEndPointDto_ForArcSegment()
        {
            // Arrange
            ArcSegmentToArcSegmentDtoConverter sut = CreateSut();
            sut.ArcSegment = CreateArcSegment();

            // Act
            sut.Convert();

            // Assert
            DtoHelper.AssertPointDto(sut.Dto.EndPoint,
                                     4.0,
                                     2.0);
        }

        [Test]
        public void Convert_SetsIsUnknown_ForArcSegment()
        {
            // Arrange
            ArcSegmentToArcSegmentDtoConverter sut = CreateSut();
            sut.ArcSegment = CreateArcSegment();

            // Act
            sut.Convert();
            ArcSegmentDto actual = sut.Dto;

            // Assert
            Assert.False(actual.IsUnknown);
        }

        [Test]
        public void Convert_SetsStartPointDto_ForArcSegment()
        {
            // Arrange
            ArcSegmentToArcSegmentDtoConverter sut = CreateSut();
            sut.ArcSegment = CreateArcSegment();

            // Act
            sut.Convert();

            // Assert
            DtoHelper.AssertPointDto(sut.Dto.StartPoint,
                                     1.0,
                                     5.0);
        }

        [Test]
        public void Convert_SetsTurnDirection_ForArcSegment()
        {
            // Arrange
            ArcSegmentToArcSegmentDtoConverter sut = CreateSut();
            sut.ArcSegment = CreateArcSegment();

            // Act
            sut.Convert();
            string actual = sut.Dto.TurnDirection;

            // Assert
            Assert.True(actual == Constants.TurnDirection.Clockwise.ToString());
        }
    }
}