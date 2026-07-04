namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Result<T2, TError> Map<T2>(Func<T, T2> map)
        where T2 : notnull
        => this switch
        {
            T okVal => map(okVal),
            TError error => error,
        };

    public async readonly Task<Result<T2, TError>> Map<T2>(Func<T, Task<T2>> map)
        where T2 : notnull
        => this switch
        {
            T okVal => await map(okVal),
            TError error => error
        };
}
