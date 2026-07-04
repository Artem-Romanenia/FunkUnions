namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly bool IsOk
        => this switch
        {
            T => true,
            TError => false,
        };

    public readonly bool IsOkAnd(Predicate<T> also)
        => this switch
        {
            T okVal => also(okVal),
            TError => false,
        };
}
