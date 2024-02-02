using TestCommon.Common.Interfaces;

namespace TestCommon.Builders;
public class CancellationTokenBuilder : IBuilder<CancellationToken>
{
    public CancellationToken Build() => new(false);
}
