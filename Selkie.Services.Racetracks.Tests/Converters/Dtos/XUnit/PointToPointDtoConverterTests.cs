using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Converters.Dtos;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    public class PointToPointDtoConverterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsPointDto_ForPoint([NotNull] Point point,
                                                  [NotNull] PointToPointDtoConverter sut)
        {
            // Arrange
            sut.Point = point;

            // Act
            sut.Convert();

            // Assert;
            DtoHelper.AssertPointDto(sut.Dto,
                                     point.X,
                                     point.Y);
        }
    }
}