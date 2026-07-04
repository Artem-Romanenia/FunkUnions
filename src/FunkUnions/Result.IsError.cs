namespace FunkUnions;

public partial struct Result<T, TError>
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
