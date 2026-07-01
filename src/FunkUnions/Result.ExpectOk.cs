namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly T ExpectOk(string? customMessage = null)
        => this switch
        {
            T okVal => okVal,
            TError => throw new OkResultExpectedException(
                customMessage ??
                "Result object was expected to have Ok value, but has Error value instead."),
        };

    public readonly T ExpectOk(Func<TError, Exception> customException)
         => this switch
         {
             T okVal => okVal,
             TError error => throw customException(error),
         };
}
