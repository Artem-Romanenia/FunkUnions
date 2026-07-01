namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly bool IsError
        => this switch
        {
            T => false,
            TError => true,
        };

    public readonly bool IsErrorAnd(Predicate<TError> also)
        => this switch
        {
            T => false,
            TError error => also(error),
        };
}
