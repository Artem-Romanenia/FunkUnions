namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly Result<T, TError2> MapError<TError2>(Func<TError, TError2> map)
        where TError2 : notnull
        => this switch
        {
            T okVal => okVal,
            TError error => map(error),
        };

    public readonly Result<T, Either<TError, TError2>> MapErrorAsUnionWith<TError2>()
        where TError2 : notnull
        => this switch
        {
            T okVal => okVal,
            TError error => new Either<TError, TError2>(error),
        };
}
