namespace FunkUnions;

public static partial class Result
{
    public static Result<T2, TError> Select<T, TError, T2>(this Result<T, TError> res, Func<T, T2> select)
        where T : notnull
        where TError : notnull
        where T2 : notnull
        => res.Map(select);

    public static Result<T, TError> Where<T, TError>(this Result<T, TError> res, Predicate<T> predicate)
        where T : notnull
        where TError : notnull, IDefaultError<TError>
        => res.OnlyIf(predicate, static _ => TError.DefaultError);

    public static Result<TResult, TError> SelectMany<T, TError, T2, TResult>(this Result<T, TError> res, Func<T, Result<T2, TError>> bind, Func<T, T2, TResult> select)
        where T : notnull
        where TError : notnull
        where T2 : notnull
        where TResult : notnull
        => res.AndIfOk(f => bind(f).Map(s => select(f, s)));
}
