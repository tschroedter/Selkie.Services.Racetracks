using System;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;
using Selkie.Services.Racetracks.Interfaces.Converters.Dtos;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    public class CircleToCircleDtoConverterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsDtoCentrePoint_ForCircle([NotNull, Frozen] IPointToPointDtoConverter converter,
                                                         [NotNull] CircleToCircleDtoConverter sut)
        {
            // Arrange
            converter.Dto.Returns(new PointDto
                                  {
                                      X = 1.0,
                                      Y = 2.0
                                  });

            sut.Circle = new Circle(1.0,
                                    2.0,
                                    3.0);

            // Act
            sut.Convert();

            // Assert
            DtoHelper.AssertPointDto(sut.Dto.CentrePoint,
                                     1.0,
                                     2.0);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsDtoRadius_ForCircle([NotNull] CircleToCircleDtoConverter sut)
        {
            // Arrange
            sut.Circle = new Circle(1.0,
                                    2.0,
                                    3.0);

            // Act
            sut.Convert();
            CircleDto actual = sut.Dto;

            // Assert
            Assert.True(Math.Abs(3.0 - actual.Radius) < DtoHelper.Tolerance);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Convert_SetsDtoIsUnknown_ForCircle([NotNull] CircleToCircleDtoConverter sut)
        {
            // Arrange
            sut.Circle = new Circle(1.0,
                                    2.0,
                                    3.0);

            // Act
            sut.Convert();
            CircleDto actual = sut.Dto;

            // Assert
            Assert.False(actual.IsUnknown);
        }
    }
}