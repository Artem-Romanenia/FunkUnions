namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T2, TError> AndIfOk<T2>(Func<T, Result<T2, TError>> op)
        where T2 : notnull
        => this switch
        {
            T okVal => op(okVal),
            TError error => error,
        };

    public readonly Result<T2, TError> AndIfOk<T2>(Func<T, Maybe<T2>> op, Func<TError> err)
        where T2 : notnull
        => this switch
        {
            T okVal => op(okVal).AsResult(err),
            TError error => error,
        };

    public readonly Maybe<T2> AndIfOk<T2>(Func<T, Maybe<T2>> op)
        where T2 : notnull
        => this switch
        {
            T okVal => op(okVal),
            TError => None.Value,
        };

    public async readonly Task<Result<T2, TError>> AndIfOk<T2>(Func<T, Task<Result<T2, TError>>> op)
        where T2 : notnull
        => this switch
        {
            T okVal => await op(okVal),
            TError error => error,
        };

    public async readonly Task<Result<T2, TError>> AndIfOk<T2>(Func<T, Task<Maybe<T2>>> op, Func<TError> err)
        where T2 : notnull
        => this switch
        {
            T okVal => (await op(okVal)).AsResult(err),
            TError error => error,
        };

    public async readonly Task<Maybe<T2>> AndIfOk<T2>(Func<T, Task<Maybe<T2>>> op)
        where T2 : notnull
        => this switch
        {
            T okVal => await op(okVal),
            TError => None.Value,
        };
}

public static partial class Result
{
    extension<T, TError>(Result<T, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<T2, Either<TError, TError2>> AndIfOk<T2, TError2>(Func<T, Result<T2, TError2>> op)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => op(okVal) switch
                {
                    T2 ok => ok,
                    TError2 error2 => new Either<TError, TError2>(error2)
                },
                TError error => new Either<TError, TError2>(error),
            };

        public Result<T2, Either<TError, TError2>> AndIfOk<T2, TError2>(Func<T, Maybe<T2>> op, Func<TError2> err)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => op(okVal).AsResult(err) switch
                {
                    T2 ok => ok,
                    TError2 error2 => new Either<TError, TError2>(error2)
                },
                TError error => new Either<TError, TError2>(error),
            };

        public async Task<Result<T2, Either<TError, TError2>>> AndIfOk<T2, TError2>(Func<T, Task<Result<T2, TError2>>> op)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => await op(okVal) switch
                {
                    T2 ok => ok,
                    TError2 error2 => new Either<TError, TError2>(error2)
                },
                TError error => new Either<TError, TError2>(error),
            };

        public async Task<Result<T2, Either<TError, TError2>>> AndIfOk<T2, TError2>(Func<T, Task<Maybe<T2>>> op, Func<TError2> err)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => (await op(okVal)).AsResult(err) switch
                {
                    T2 ok => ok,
                    TError2 error2 => new Either<TError, TError2>(error2)
                },
                TError error => new Either<TError, TError2>(error),
            };
    }
}
