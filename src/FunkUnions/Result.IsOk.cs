namespace FunkUnions;

public partial union Result<T, TError>
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
