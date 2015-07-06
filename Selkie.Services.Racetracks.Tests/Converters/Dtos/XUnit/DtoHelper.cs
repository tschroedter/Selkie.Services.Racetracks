using System;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Services.Racetracks.Common.Dto;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    internal class DtoHelper
    {
        public const double Tolerance = 0.01;

        public static void AssertPointDto(PointDto actual,
                                          Point point,
                                          string text = "Point")
        {
            AssertPointDto(actual,
                           point.X,
                           point.Y,
                           text);
        }

        public static void AssertPointDto(PointDto actual,
                                          double expectedX,
                                          double expectedY,
                                          string text = "Point")
        {
            Assert.True(Math.Abs(actual.X - expectedX) < Tolerance,
                        "{0} X: Expected {1} but actual {2}".Inject(text,
                                                                    expectedX,
                                                                    actual.X));

            Assert.True(Math.Abs(actual.Y - expectedY) < Tolerance,
                        "{0} Y: Expected {1} but actual {2}".Inject(text,
                                                                    expectedY,
                                                                    actual.Y));
        }
    }
}