using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Services.Racetracks.Converters.Dtos;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class PointToPointDtoConverterTests
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