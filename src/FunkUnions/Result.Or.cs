namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T, TError2> Or<TError2>(Result<T, TError2> res)
        where TError2 : notnull
        => this switch
        {
            T okVal => okVal,
            TError error => res,
        };
}

public static partial class Result
{
    extension<T, TError>(Result<T, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<Either<T, T2>, TError2> Or<T2, TError2>(Result<T2, TError2> res2)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => new Either<T, T2>(okVal),
                TError error => res2 switch
                {
                    T2 ok => new Either<T, T2>(ok),
                    TError2 error2 => error2
                },
            };
    }
}
