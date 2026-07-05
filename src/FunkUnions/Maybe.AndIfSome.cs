namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Maybe<T2> AndIfSome<T2>(Func<T, Maybe<T2>> op)
        where T2 : notnull
        => this switch
        {
            T some => op(some),
            None => None.Value,
        };

    public async readonly Task<Maybe<T2>> AndIfSome<T2>(Func<T, Task<Maybe<T2>>> op)
        where T2 : notnull
        => this switch
        {
            T some => await op(some),
            None => None.Value,
        };
}
