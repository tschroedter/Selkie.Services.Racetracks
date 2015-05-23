using System.Diagnostics.CodeAnalysis;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.XUnit
{
    //ncrunch: no coverage start
    // ReSharper disable once ClassTooBig
    [ExcludeFromCodeCoverage]
    public sealed class LineToLineDtoConverterTests
    {
        private readonly LineDto m_Dto;
        private readonly LineDto m_DtoFromLine;

        public LineToLineDtoConverterTests()
        {
            ILine line = new Line(1,
                                  2.0,
                                  3.0,
                                  4.0,
                                  5.0,
                                  true,
                                  Constants.LineDirection.Reverse);

            m_DtoFromLine = LineToLineDtoConverter.ConvertFrom(line);

            m_Dto = new LineDto
                    {
                        Id = 1,
                        X1 = 2.0,
                        Y1 = 3.0,
                        X2 = 4.0,
                        Y2 = 5.0,
                        IsUnknown = true,
                        RunDirection = "Reverse"
                    };
        }

        [Fact]
        public void IdDefaultForFromLineTest()
        {
            Assert.Equal(1,
                         m_DtoFromLine.Id);
        }

        [Fact]
        public void IdDefaultTest()
        {
            Assert.Equal(1,
                         m_Dto.Id);
        }

        [Fact]
        public void IdRoundtripTest()
        {
            m_Dto.X1 = 2;

            Assert.Equal(2,
                         m_Dto.X1);
        }

        [Fact]
        public void IsUnknownDefaultForFromLineTest()
        {
            Assert.True(m_DtoFromLine.IsUnknown);
        }

        [Fact]
        public void IsUnknownDefaultTest()
        {
            Assert.True(m_Dto.IsUnknown);
        }

        [Fact]
        public void IsUnknownRoundtripTest()
        {
            m_Dto.IsUnknown = false;

            Assert.False(m_Dto.IsUnknown);
        }

        [Fact]
        public void RunDirectionDefaultForFromLineTest()
        {
            Assert.Equal(Selkie.Common.Constants.LineDirection.Reverse.ToString(),
                         m_DtoFromLine.RunDirection);
        }

        [Fact]
        public void RunDirectionDefaultTest()
        {
            Assert.Equal(Selkie.Common.Constants.LineDirection.Reverse.ToString(),
                         m_Dto.RunDirection);
        }

        [Fact]
        public void RunDirectionRoundtripTest()
        {
            m_Dto.RunDirection = Selkie.Common.Constants.LineDirection.Forward.ToString();

            Assert.Equal(Selkie.Common.Constants.LineDirection.Forward.ToString(),
                         m_Dto.RunDirection);
        }

        [Fact]
        public void X1DefaultForFromLineTest()
        {
            Assert.Equal(2.0,
                         m_DtoFromLine.X1);
        }

        [Fact]
        public void X1DefaultTest()
        {
            Assert.Equal(2.0,
                         m_Dto.X1);
        }

        [Fact]
        public void X1RoundtripForFromLineTest()
        {
            var dto = new LineDto
                      {
                          X1 = 1.0
                      };
            Assert.Equal(1.0,
                         dto.X1);

            dto.X1 = 2.0;
            Assert.Equal(2.0,
                         dto.X1);
        }

        [Fact]
        public void X1RoundtripTest()
        {
            m_Dto.X1 = 2.0;

            Assert.Equal(2.0,
                         m_Dto.X1);
        }

        [Fact]
        public void X2DefaultForFromLineTest()
        {
            Assert.Equal(4.0,
                         m_DtoFromLine.X2);
        }

        [Fact]
        public void X2DefaultTest()
        {
            Assert.Equal(4.0,
                         m_Dto.X2);
        }

        [Fact]
        public void X2RoundtripTest()
        {
            m_Dto.X2 = 20.0;

            Assert.Equal(20.0,
                         m_Dto.X2);
        }

        [Fact]
        public void Y1DefaultForFromLineTest()
        {
            Assert.Equal(3.0,
                         m_DtoFromLine.Y1);
        }

        [Fact]
        public void Y1DefaultTest()
        {
            Assert.Equal(3.0,
                         m_Dto.Y1);
        }

        [Fact]
        public void Y1RoundtripTest()
        {
            m_Dto.Y1 = 20.0;

            Assert.Equal(20.0,
                         m_Dto.Y1);
        }

        [Fact]
        public void Y2DefaultForFromLineTest()
        {
            Assert.Equal(5.0,
                         m_DtoFromLine.Y2);
        }

        [Fact]
        public void Y2DefaultTest()
        {
            Assert.Equal(5.0,
                         m_Dto.Y2);
        }

        [Fact]
        public void Y2RoundtripTest()
        {
            m_Dto.Y2 = 20.0;

            Assert.Equal(20.0,
                         m_Dto.Y2);
        }
    }
}