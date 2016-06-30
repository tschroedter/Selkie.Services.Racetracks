using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Services.Racetracks.Converters.Dtos;
using Constants = Selkie.Geometry.Constants;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class PolylineToPolylineDtoConverterTests
    {
        private const int DoNotCareId = -1;

        [Theory]
        [AutoNSubstituteData]
        public void IsPolylineAUturn_ReturnsFalse_ForEmpty([NotNull] PolylineToPolylineDtoConverter sut)
        {
            // Arrange
            IPolyline polyline = CreateEmptyPolyline();

            // Act
            // Assert
            Assert.False(sut.IsPolylineAUturn(polyline));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsPolylineAUturn_ReturnsFalse_ForNormal([NotNull] PolylineToPolylineDtoConverter sut)
        {
            // Arrange
            IPolyline polyline = CreateNormalPolyline();

            // Act
            // Assert
            Assert.False(sut.IsPolylineAUturn(polyline));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsPolylineAUturn_ReturnsFalse_ForNotEnoughSegments([NotNull] PolylineToPolylineDtoConverter sut)
        {
            // Arrange
            IPolyline polyline = CreateWithNotEnoughSegmentsPolyline();

            // Act
            // Assert
            Assert.False(sut.IsPolylineAUturn(polyline));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsPolylineAUturn_ReturnsFalse_ForToManySegments([NotNull] PolylineToPolylineDtoConverter sut)
        {
            // Arrange
            IPolyline polyline = CreatePolylineWithToManySegments();

            // Act
            // Assert
            Assert.False(sut.IsPolylineAUturn(polyline));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsPolylineAUturn_ReturnsTrue_ForUTurn([NotNull] PolylineToPolylineDtoConverter sut)
        {
            // Arrange
            IPolyline polyline = CreateUTurnPolyline();

            // Act
            // Assert
            Assert.True(sut.IsPolylineAUturn(polyline));
        }

        private static ArcSegment CreateEndArcSegment()
        {
            var circle = new Circle(6.0,
                                    0.0,
                                    3.0);
            var startPoint = new Point(6.0,
                                       3.0);
            var endPoint = new Point(9.0,
                                     0.0);

            return new ArcSegment(circle,
                                  startPoint,
                                  endPoint);
        }

        private static ArcSegment CreateStartArcSegment()
        {
            var circle = new Circle(0.0,
                                    0.0,
                                    3.0);
            var startPoint = new Point(-3.0,
                                       0.0);
            var endPoint = new Point(0.0,
                                     3.0);

            return new ArcSegment(circle,
                                  startPoint,
                                  endPoint);
        }

        private static PolylineToPolylineDtoConverter CreateSut()
        {
            return new PolylineToPolylineDtoConverter(
                new SegmentToSegmentDtoConverter(
                    new ArcSegmentToArcSegmentDtoConverter(new PointToPointDtoConverter(),
                                                           new CircleToCircleDtoConverter(
                                                               new PointToPointDtoConverter())),
                    new LineToLineSegmentDtoConverter(new PointToPointDtoConverter())));
        }

        private IPolyline CreateEmptyPolyline()
        {
            var polyline = Substitute.For <IPolyline>();

            polyline.Segments.Returns(new IPolylineSegment[0]);

            return polyline;
        }

        private ILine CreateLineSegment()
        {
            return new Line(0.0,
                            3.0,
                            6.0,
                            3.0);
        }

        private IPolyline CreateNormalPolyline()
        {
            var uturn = new IPolylineSegment[]
                        {
                            Substitute.For <IArcSegment>(),
                            Substitute.For <ILine>(),
                            Substitute.For <IArcSegment>()
                        };

            var polyline = Substitute.For <IPolyline>();

            polyline.Segments.Returns(uturn);

            return polyline;
        }

        private IPolyline CreatePolyline()
        {
            ArcSegment startSegment = CreateStartArcSegment();
            ILine lineSegment = CreateLineSegment();
            ArcSegment endSegment = CreateEndArcSegment();

            var polyline = new Polyline(DoNotCareId,
                                        Constants.LineDirection.Forward);

            polyline.AddSegment(startSegment);
            polyline.AddSegment(lineSegment);
            polyline.AddSegment(endSegment);

            return polyline;
        }

        private IEnumerable <IPolylineSegment> CreatePolylineSegments(int numberOfSegments)
        {
            var segments = new List <IPolylineSegment>();

            for ( var i = 0 ; i < numberOfSegments ; i++ )
            {
                segments.Add(Substitute.For <IArcSegment>());
            }

            return segments;
        }

        private IPolyline CreatePolylineWithToManySegments()
        {
            IEnumerable <IPolylineSegment> segments = CreatePolylineSegments(4);

            var polyline = Substitute.For <IPolyline>();

            polyline.Segments.Returns(segments);

            return polyline;
        }

        private IPolyline CreateUTurnPolyline()
        {
            IEnumerable <IPolylineSegment> segments = CreatePolylineSegments(3);

            var polyline = Substitute.For <IPolyline>();

            polyline.Segments.Returns(segments);

            return polyline;
        }

        private IPolyline CreateWithNotEnoughSegmentsPolyline()
        {
            IEnumerable <IPolylineSegment> segments = CreatePolylineSegments(2);

            var polyline = Substitute.For <IPolyline>();

            polyline.Segments.Returns(segments);

            return polyline;
        }

        [Test]
        public void Convert_UpdatesDto_ForPolyline()
        {
            // Arrange
            IPolyline polyline = CreatePolyline();
            PolylineToPolylineDtoConverter sut = CreateSut();
            sut.Polyline = polyline;

            // Act
            sut.Convert();

            // Assert
            Assert.True(sut.Dto.Segments.Length == polyline.Segments.Count(),
                        "Segments count");
        }

        [Test]
        public void Polyline_Updates_ForNewPolyline()
        {
            // Arrange
            IPolyline polyline = CreatePolyline();
            PolylineToPolylineDtoConverter sut = CreateSut();
            sut.Polyline = polyline;

            // Act
            // Assert
            Assert.True(sut.Polyline == polyline);
        }
    }
}