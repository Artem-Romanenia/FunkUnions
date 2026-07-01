namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly T ExpectOk(string? customMessage = null)
        => this switch
        {
            T okVal => okVal,
            TError => throw new OkResultExpectedException(customMessage),
        };

    public readonly T ExpectOk(Func<TError, Exception> customException)
         => this switch
         {
             T okVal => okVal,
             TError error => throw customException(error),
         };
}
