namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T, TError> OnlyIf(Predicate<T> op, Func<T, TError> error)
        => this switch
        {
            T okVal => op(okVal) ? okVal : error(okVal),
            TError err => err,
        };
}

public static partial class Result
{
    extension<T, TError>(Result<T, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<T, Either<TError, TError2>> OnlyIf<TError2>(Predicate<T> op, Func<T, TError2> error)
            where TError2 : notnull
            => res switch
            {
                T okVal => op(okVal) ? okVal : new Either<TError, TError2>(error(okVal)),
                TError err => new Either<TError, TError2>(err),
            };
    }
}
