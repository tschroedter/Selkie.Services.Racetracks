﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Selkie.Geometry.Surveying;
using Selkie.NUnit.Extensions;
using Selkie.Services.Common.Dto;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class SurveyFeaturesSourceManagerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void SetSurveyFeaturesIfValid_LogsError_ForSurveyFeaturesDtosIsNull(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] SurveyFeaturesSourceManager manager)
        {
            // assemble
            // act
            // ReSharper disable once AssignNullToNotNullAttribute
            manager.SetSurveyFeaturesIfValid(null);

            // assert
            logger.Received().Error(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SetSurveyFeaturesIfValid_ReplacesNullWithEmpty_ForFeatureDtos(
            [NotNull] SurveyFeaturesSourceManager manager)
        {
            // assemble
            // act
            // ReSharper disable once AssignNullToNotNullAttribute
            manager.SetSurveyFeaturesIfValid(null);

            // assert
            Assert.AreEqual(0,
                            manager.Features.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SetSurveyFeaturesIfValid_UpdatesSourceCount_ForSurveyFeatureDtos(
            [NotNull] SurveyFeaturesSourceManager manager)
        {
            // assemble
            SurveyFeatureDto[] dtos = CreateSurveyFeatureDtos();

            // act
            manager.SetSurveyFeaturesIfValid(dtos);

            // assert
            IEnumerable <ISurveyFeature> actual = manager.Features;

            Assert.AreEqual(dtos.Length,
                            actual.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] SurveyFeaturesSourceManager manager)
        {
            // assemble
            // act
            manager.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] SurveyFeaturesSourceManager manager)
        {
            // assemble
            // act
            manager.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [NotNull]
        private static SurveyFeatureDto[] CreateSurveyFeatureDtos()
        {
            var dtoOne = new SurveyFeatureDto
                         {
                             Id = 0
                         };
            var dtoTwo = new SurveyFeatureDto
                         {
                             Id = 1
                         };

            SurveyFeatureDto[] dtos =
            {
                dtoOne,
                dtoTwo
            };

            return dtos;
        }
    }
}