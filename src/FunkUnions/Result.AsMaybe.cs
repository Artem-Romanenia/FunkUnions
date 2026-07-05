namespace FunkUnions;

public partial struct Result<T, TError>
{
    public readonly Maybe<T> AsMaybe()
        => this switch
        {
            T okVal => okVal,
            TError error => None.Value,
        };
}
