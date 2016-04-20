using JetBrains.Annotations;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.TypedFactories
{
    public interface ICostMatrixFactory : ITypedFactory
    {
        [NotNull]
        ICostMatrix Create();

        void Release([NotNull] ICostMatrix costMatrix);
    }
}