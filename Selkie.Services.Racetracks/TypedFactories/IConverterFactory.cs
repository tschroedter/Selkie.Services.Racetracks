using JetBrains.Annotations;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.TypedFactories
{
    public interface IConverterFactory : ITypedFactory
    {
        T Create <T>() where T : IConverter;
        void Release([NotNull] IConverter converter);
    }
}