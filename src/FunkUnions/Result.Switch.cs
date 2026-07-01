using System.Diagnostics;

namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly void Switch(Action<T> ok, Action<TError> error)
    {
        switch (this)
        {
            case T okVal:
                ok(okVal);
                break;
            case TError err:
                error(err);
                break;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task Switch(Func<T, Task> ok, Action<TError> error)
    {
        switch (this)
        {
            case T okVal:
                await ok(okVal);
                break;
            case TError err:
                error(err);
                break;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task Switch(Action<T> ok, Func<TError, Task> error)
    {
        switch (this)
        {
            case T okVal:
                ok(okVal);
                break;
            case TError err:
                await error(err);
                break;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly async Task Switch(Func<T, Task> ok, Func<TError, Task> error)
    {
        switch (this)
        {
            case T okVal:
                await ok(okVal);
                break;
            case TError err:
                await error(err);
                break;
            default: throw new UnreachableException($"Result object is not expected to have value: '{Value}'");
        }
    }

    public readonly TResult Switch<TResult>(Func<T, TResult> ok, Func<TError, TResult> error)
        => this switch
        {
            T okVal => ok(okVal),
            TError err => error(err),
        };

    public readonly async Task<TResult> Switch<TResult>(Func<T, Task<TResult>> ok, Func<TError, TResult> error)
        => this switch
        {
            T okVal => await ok(okVal),
            TError err => error(err),
        };

    public readonly async Task<TResult> Switch<TResult>(Func<T, TResult> ok, Func<TError, Task<TResult>> error)
        => this switch
        {
            T okVal => ok(okVal),
            TError err => await error(err),
        };

    public readonly async Task<TResult> Switch<TResult>(Func<T, Task<TResult>> ok, Func<TError, Task<TResult>> error)
        => this switch
        {
            T okVal => await ok(okVal),
            TError err => await error(err),
        };
}
