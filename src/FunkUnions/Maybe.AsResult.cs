namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Result<T, TError> AsResult<TError>(Func<TError> op)
        where TError : notnull
        => this switch
        {
            T some => some,
            None => op(),
        };
}
