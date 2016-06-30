using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Common;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class SegmentToSegmentDtoConverterTests
    {
        private void AssertArcSegmentDto(ArcSegmentDto actual,
                                         IArcSegment segment)
        {
            DtoHelper.AssertPointDto(actual.Circle.CentrePoint,
                                     segment.CentrePoint,
                                     "CentrePoint");

            DtoHelper.AssertPointDto(actual.StartPoint,
                                     segment.StartPoint,
                                     "StartPoint");

            DtoHelper.AssertPointDto(actual.EndPoint,
                                     segment.EndPoint,
                                     "EndPoint");

            DtoHelper.AssertPointDto(actual.EndPoint,
                                     segment.EndPoint,
                                     "EndPoint");

            string expectedTurnDirection = Constants.TurnDirection.Clockwise.ToString();

            Assert.True(expectedTurnDirection == actual.TurnDirection,
                        "TurnDirection");
        }

        private void AssertLineDto(SegmentDto actual,
                                   double expectedX1,
                                   double expectedY1,
                                   double expectedX2,
                                   double expectedY2)
        {
            DtoHelper.AssertPointDto(actual.StartPoint,
                                     expectedX1,
                                     expectedY1,
                                     "StartPoint");

            DtoHelper.AssertPointDto(actual.EndPoint,
                                     expectedX2,
                                     expectedY2,
                                     "EndPoint");
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

        private IPolylineSegment CreateLineSegment()
        {
            return new Line(1.0,
                            2.0,
                            3.0,
                            4.0);
        }

        private SegmentToSegmentDtoConverter CreateSut()
        {
            return
                new SegmentToSegmentDtoConverter(new ArcSegmentToArcSegmentDtoConverter(new PointToPointDtoConverter(),
                                                                                        new CircleToCircleDtoConverter(
                                                                                            new PointToPointDtoConverter
                                                                                                ())),
                                                 new LineToLineSegmentDtoConverter(new PointToPointDtoConverter()));
        }

        [Test]
        public void Convert_ReturnsDto_ForArcSegment()
        {
            // Arrange
            SegmentToSegmentDtoConverter sut = CreateSut();
            IArcSegment arcSegment = CreateArcSegment();
            sut.Segment = arcSegment;

            // Act
            sut.Convert();
            var actual = sut.Dto as ArcSegmentDto;

            // Assert
            AssertArcSegmentDto(actual,
                                arcSegment);
        }

        [Test]
        public void Convert_ReturnsDto_ForLineSegment()
        {
            // Arrange
            SegmentToSegmentDtoConverter sut = CreateSut();
            sut.Segment = CreateLineSegment();

            // Act
            sut.Convert();
            SegmentDto actual = sut.Dto;

            // Assert
            AssertLineDto(actual,
                          1.0,
                          2.0,
                          3.0,
                          4.0);
        }

        [Test]
        public void Convert_Throws_ForUnknwonSegment()
        {
            // Arrange
            SegmentToSegmentDtoConverter sut = CreateSut();
            sut.Segment = Substitute.For <IPolylineSegment>();

            // Act
            // Assert
            Assert.Throws <ArgumentException>(() => sut.Convert());
        }
    }
}