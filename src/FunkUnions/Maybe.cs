using System.Runtime.CompilerServices;

namespace FunkUnions;

[Union]
public partial struct Maybe<T> : IUnion
    where T : notnull
{
    private readonly T? _some;
    private readonly bool _isSome;

    public Maybe(T value) => (_some, _isSome) = (value, true);
    public Maybe(None value) => _isSome = false;

    public bool HasValue => true;

    public bool TryGetValue(out T value) 
    {
        value = _some!;
        return _isSome;
    }
    public bool TryGetValue(out None value)
    {
        value = None.Value;
        return !_isSome;
    }

    public readonly object? Value => _isSome ? _some : None.Value;
}

public static partial class Maybe;
