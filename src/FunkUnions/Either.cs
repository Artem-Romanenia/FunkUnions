using System.Runtime.CompilerServices;

namespace FunkUnions;

[Union]
public struct Either<TLeft, TRight> : IUnion
    where TLeft : notnull
    where TRight : notnull
{
    private readonly TLeft? _left;
    private readonly TRight? _right;
    private readonly bool _isLeft;

    public Either(TLeft value) => (_left, _isLeft) = (value, true);
    public Either(TRight value) => (_right, _isLeft) = (value, false);

    public bool HasValue => true;

    public bool TryGetValue(out TLeft value) 
    {
        value = _left!;
        return _isLeft;
    }
    public bool TryGetValue(out TRight value)
    {
        value = _right!;
        return !_isLeft;
    }

    public object? Value => _isLeft ? _left : _right;
}
