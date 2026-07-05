namespace FunkUnions;

public static partial class Maybe
{
    public static Maybe<T2> Select<T, T2>(this Maybe<T> maybe, Func<T, T2> select)
        where T : notnull
        where T2 : notnull
        => maybe.Map(select);

    public static Maybe<T> Where<T>(this Maybe<T> res, Predicate<T> predicate)
        where T : notnull
        => res.OnlyIf(predicate);

    public static Maybe<TResult> SelectMany<T, T2, TResult>(this Maybe<T> res, Func<T, Maybe<T2>> bind, Func<T, T2, TResult> select)
        where T : notnull
        where T2 : notnull
        where TResult : notnull
        => res.AndIfSome(f => bind(f).Map(s => select(f, s)));
}
