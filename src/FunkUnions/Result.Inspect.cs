using System.Diagnostics;

namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T, TError> InspectOk(Action<T> ok)
    {
        switch (this)
        {
            case T okVal:
                ok(okVal);
                return this;
            case TError err:
                return this;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task<Result<T, TError>> InspectOk(Func<T, Task> ok)
    {
        switch (this)
        {
            case T okVal:
                await ok(okVal);
                return this;
            case TError err:
                return this;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly Result<T, TError> InspectError(Action<TError> error)
    {
        switch (this)
        {
            case T okVal:
                return this;
            case TError err:
                error(err);
                return this;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task<Result<T, TError>> InspectError(Func<TError, Task> error)
    {
        switch (this)
        {
            case T okVal:
                return this;
            case TError err:
                await error(err);
                return this;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }
}
