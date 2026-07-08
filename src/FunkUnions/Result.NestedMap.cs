namespace FunkUnions;

public static partial class Result
{
    extension<T, TError>(Result<IEnumerable<T>, TError> res)
        where T : notnull
        where TError : notnull
    {
        public Result<IEnumerable<T2>, TError> NestedMap<T2>(Func<T, T2> map)
            => res.Map(e => e.Select(map));
    }
}
