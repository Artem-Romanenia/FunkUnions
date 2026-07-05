using System.Diagnostics;

namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Maybe<T> Inspect(Action<T> some)
    {
        switch (this)
        {
            case T okVal:
                some(okVal);
                return this;
            case None:
                return this;
            default: throw new UnreachableException($"Maybe object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task<Maybe<T>> Inspect(Func<T, Task> some)
    {
        switch (this)
        {
            case T okVal:
                await some(okVal);
                return this;
            case None:
                return this;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }
}
