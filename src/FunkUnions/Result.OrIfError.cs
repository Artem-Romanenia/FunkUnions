namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly Result<T, TError2> OrIfError<TError2>(Func<TError, Result<T, TError2>> op)
        where TError2 : notnull
        => this switch
        {
            T okVal => okVal,
            TError error => op(error)
        };

    public async readonly Task<Result<T, TError2>> OrIfError<TError2>(Func<TError, Task<Result<T, TError2>>> op)
        where TError2 : notnull
        => this switch
        {
            T okVal => okVal,
            TError error => await op(error)
        };
}

public static partial class ResultExtensions
{
    extension<T, TError>(Result<T, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<Either<T, T2>, TError2> OrIfError<T2, TError2>(Func<TError, Result<T2, TError2>> op)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => new Either<T, T2>(okVal),
                TError error => op(error) switch
                {
                    T2 okVal2 => new Either<T, T2>(okVal2),
                    TError2 err => err
                },
            };

        public async Task<Result<Either<T, T2>, TError2>> OrIfError<T2, TError2>(Func<TError, Task<Result<T2, TError2>>> op)
            where T2 : notnull
            where TError2 : notnull
            => res switch
            {
                T okVal => new Either<T, T2>(okVal),
                TError error => await op(error) switch
                {
                    T2 okVal2 => new Either<T, T2>(okVal2),
                    TError2 err => err
                },
            };
    }
    
}
