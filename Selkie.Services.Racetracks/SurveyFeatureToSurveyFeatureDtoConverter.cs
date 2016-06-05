using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks
{
    public class SurveyFeatureToSurveyFeatureDtoConverter
    {
        [NotNull]
        public static SurveyFeatureDto ConvertToDto([NotNull] ISurveyFeature feature)
        {
            var dto = new SurveyFeatureDto
                      {
                          Id = feature.Id,
                          IsUnknown = feature.IsUnknown,
                          StartPoint = CreatePointDtoForPoint(feature.StartPoint),
                          EndPoint = CreatePointDtoForPoint(feature.EndPoint),
                          AngleToXAxisAtStartPoint = feature.AngleToXAxisAtStartPoint.Degrees,
                          AngleToXAxisAtEndPoint = feature.AngleToXAxisAtEndPoint.Degrees,
                          RunDirection = feature.RunDirection.ToString(),
                          Length = feature.Length
                      };

            return dto;
        }

        [NotNull]
        public static ISurveyFeature ConvertToSurveyFeature([NotNull] SurveyFeatureDto dto)
        {
            Constants.LineDirection direction; // todo rename to SurveyDirection

            Enum.TryParse(dto.RunDirection,
                          out direction);

            var line = new SurveyFeature(dto.Id,
                                         CreatePointForPointDto(dto.StartPoint),
                                         CreatePointForPointDto(dto.EndPoint),
                                         Angle.FromDegrees(dto.AngleToXAxisAtStartPoint),
                                         Angle.FromDegrees(dto.AngleToXAxisAtEndPoint),
                                         direction,
                                         dto.Length,
                                         dto.IsUnknown);

            return line;
        }

        private static PointDto CreatePointDtoForPoint(Point point)
        {
            return new PointDto
                   {
                       X = point.X,
                       Y = point.Y
                   };
        }

        private static Point CreatePointForPointDto([NotNull] PointDto dto)
        {
            return new Point(dto.X,
                             dto.Y);
        }
    }
}