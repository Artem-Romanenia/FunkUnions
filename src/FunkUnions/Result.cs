using System.Runtime.CompilerServices;

namespace FunkUnions;

[Union]
public partial struct Result<T, TError> : IUnion
    where T : notnull
    where TError : notnull
{
    private readonly T? _ok;
    private readonly TError? _error;
    private readonly bool _isOk;

    public Result(T value) => (_ok, _isOk) = (value, true);
    public Result(TError value) => (_error, _isOk) = (value, false);

    public bool HasValue => true;

    public bool TryGetValue(out T value)
    {
        value = _ok!;
        return _isOk;
    }
    public bool TryGetValue(out TError value)
    {
        value = _error!;
        return !_isOk;
    }

    public object? Value => _isOk ? _ok : _error;
}

public static partial class Result;
