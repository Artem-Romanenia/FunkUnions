namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Maybe<T> OrIfNone(Func<Maybe<T>> op)
        => this switch
        {
            T some => some,
            None => op()
        };

    public async readonly Task<Maybe<T>> OrIfNone(Func<Task<Maybe<T>>> op)
        => this switch
        {
            T some => some,
            None => await op()
        };
}

public static partial class Maybe
{
    extension<T>(Maybe<T> res)
        where T : notnull
    {
        public Maybe<Either<T, T2>> OrIfNone<T2>(Func<Maybe<T2>> op)
            where T2 : notnull
            => res switch
            {
                T some => new Either<T, T2>(some),
                None => op() switch
                {
                    T2 some2 => new Either<T, T2>(some2),
                    None => None.Value
                },
            };

        public async Task<Maybe<Either<T, T2>>> OrIfNone<T2>(Func<Task<Maybe<T2>>> op)
            where T2 : notnull
            => res switch
            {
                T some => new Either<T, T2>(some),
                None => await op() switch
                {
                    T2 some2 => new Either<T, T2>(some2),
                    None => None.Value
                },
            };
    }
}
