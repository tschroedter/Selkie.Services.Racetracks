using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Selkie.Services.Racetracks.Console
{
    [ExcludeFromCodeCoverage]
    internal class DummyUserForNewtonsoftJson
    {
        private DummyUserForNewtonsoftJson()
        {
            // DO NOT DELETE THIS CODE UNLESS WE NO LONGER REQUIRE ASSEMBLY Newtonsoft.dll
            Type dummy = typeof(JsonConverter);

            System.Console.WriteLine(dummy.FullName);
        }
    }
}