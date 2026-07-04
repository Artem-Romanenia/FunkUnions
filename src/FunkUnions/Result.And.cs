namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T2, TError> And<T2>(Result<T2, TError> res)
        where T2 : notnull
        => this switch
        {
            T okVal => res,
            TError error => error,
        };
}

public static partial class Result
{
    extension<T, TError>(Result<T, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<T2, Either<TError, TError2>> And<T2, TError2>(Result<T2, TError2> res2)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => res2 switch
                {
                    T2 ok => ok,
                    TError2 error2 => new Either<TError, TError2>(error2)
                },
                TError error => new Either<TError, TError2>(error),
            };
    }
}
