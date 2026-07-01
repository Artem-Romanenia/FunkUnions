namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly T Unwrap() => ExpectOk();

    public readonly T UnwrapOr(T fallback)
        => this switch
        {
            T okVal => okVal,
            TError => fallback
        };

    public readonly T UnwrapOrIfError(Func<TError, T> op)
        => this switch
        {
            T okVal => okVal,
            TError error => op(error)
        };

    // Async UnwrapOrIfError is intentionally skipped, since it is hard to imagine async operation being infallible.
    // And if you define `op` as `Func<TError, Task<Result<T, TError2>>>` it basically turns into plain `OrIfError`.
    // public async readonly Task<T> UnwrapOrIfError(Func<TError, Task<T>> op) => throw new NotImplementedException();
}
