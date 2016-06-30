using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class LineToLineSegmentDtoConverterTests
    {
        private static LineToLineSegmentDtoConverter CreateSut()
        {
            return new LineToLineSegmentDtoConverter(new PointToPointDtoConverter());
        }

        [Test]
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

        [Test]
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
    }
}