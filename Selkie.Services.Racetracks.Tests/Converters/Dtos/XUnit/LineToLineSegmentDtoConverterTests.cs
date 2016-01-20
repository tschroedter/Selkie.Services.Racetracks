using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    public class LineToLineSegmentDtoConverterTests
    {
        [Fact]
        public void Convert_SetsStartPoint_ForCircle()
        {
            // Arrange
            LineToLineSegmentDtoConverter sut = CreateSut();
            sut.Line = new Line(1.0,
                                2.0,
                                3.0,
                                4.0);

            // Act
            sut.Convert();
            PointDto actual = sut.Dto.StartPoint;

            // Assert
            DtoHelper.AssertPointDto(actual,
                                     1.0,
                                     2.0);
        }

        [Fact]
        public void Convert_SetsEndPoint_ForCircle()
        {
            // Arrange
            LineToLineSegmentDtoConverter sut = CreateSut();
            sut.Line = new Line(1.0,
                                2.0,
                                3.0,
                                4.0);

            // Act
            sut.Convert();
            PointDto actual = sut.Dto.EndPoint;

            // Assert
            DtoHelper.AssertPointDto(actual,
                                     3.0,
                                     4.0);
        }

        private static LineToLineSegmentDtoConverter CreateSut()
        {
            return new LineToLineSegmentDtoConverter(new PointToPointDtoConverter());
        }
    }
}