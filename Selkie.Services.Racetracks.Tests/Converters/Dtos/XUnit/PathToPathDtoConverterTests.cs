using JetBrains.Annotations;
using NSubstitute;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    public class PathToPathDtoConverterTests
    {
        private const int DoNotCareId = -1;

        [Theory]
        [AutoNSubstituteData]
        public void Path_Roundtrip_ForPath([NotNull] IPath path,
                                           [NotNull] PathToPathDtoConverter sut)
        {
            // Arrange
            // Act
            sut.Path = path;

            // Assert
            Assert.True(sut.Path == path);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsIsUnknownToFalse_ForPolyline([NotNull] IPointToPointDtoConverter pointToPointDto,
                                                             [NotNull] IPolylineToPolylineDtoConverter
                                                                 polylineToPolylineDto)
        {
            // Arrange
            var sut = new PathToPathDtoConverter(pointToPointDto,
                                                 polylineToPolylineDto)
                      {
                          Path = CreatePath()
                      };


            // Act
            sut.Convert();

            // Assert
            Assert.False(sut.Dto.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsStartPoint_ForPolyline([NotNull] PointDto startPointDto,
                                                       [NotNull] PointDto endPointDto,
                                                       [NotNull] IPointToPointDtoConverter pointToPointDto,
                                                       [NotNull] IPolylineToPolylineDtoConverter
                                                           polylineToPolylineDto)
        {
            // Arrange
            pointToPointDto.Dto.Returns(startPointDto,
                                        endPointDto);

            var sut = new PathToPathDtoConverter(pointToPointDto,
                                                 polylineToPolylineDto)
                      {
                          Path = CreatePath()
                      };


            // Act
            sut.Convert();

            // Assert
            Assert.True(sut.Dto.StartPoint == startPointDto);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsEndPoint_ForPolyline([NotNull] PointDto startPointDto,
                                                     [NotNull] PointDto endPointDto,
                                                     [NotNull] IPointToPointDtoConverter pointToPointDto,
                                                     [NotNull] IPolylineToPolylineDtoConverter
                                                         polylineToPolylineDto)
        {
            // Arrange
            pointToPointDto.Dto.Returns(startPointDto,
                                        endPointDto);

            var sut = new PathToPathDtoConverter(pointToPointDto,
                                                 polylineToPolylineDto)
                      {
                          Path = CreatePath()
                      };


            // Act
            sut.Convert();

            // Assert
            Assert.True(sut.Dto.EndPoint == endPointDto);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsPolyline_ForPolyline([NotNull] PolylineDto polylineDto,
                                                     [NotNull] IPointToPointDtoConverter pointToPointDto,
                                                     [NotNull] IPolylineToPolylineDtoConverter
                                                         polylineToPolylineDto)
        {
            // Arrange
            polylineToPolylineDto.Dto.Returns(polylineDto);

            var sut = new PathToPathDtoConverter(pointToPointDto,
                                                 polylineToPolylineDto)
                      {
                          Path = CreatePath()
                      };


            // Act
            sut.Convert();

            // Assert
            Assert.True(sut.Dto.Polyline == polylineDto);
        }

        private IPath CreatePath()
        {
            IPolyline polyline = CreatePolyline();

            var path = new Path(polyline);

            return path;
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

        private ILine CreateLineSegment()
        {
            return new Line(0.0,
                            3.0,
                            6.0,
                            3.0);
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
    }
}