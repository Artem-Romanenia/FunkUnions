namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Maybe<T2> Map<T2>(Func<T, T2> map)
        where T2 : notnull
        => this switch
        {
            T some => map(some),
            None => None.Value
        };

    public async readonly Task<Maybe<T2>> Map<T2>(Func<T, Task<T2>> map)
        where T2 : notnull
        => this switch
        {
            T some => await map(some),
            None => None.Value
        };
}
