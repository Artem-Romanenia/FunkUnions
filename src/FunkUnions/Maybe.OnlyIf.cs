namespace FunkUnions;

public partial struct Maybe<T>
{
    public readonly Maybe<T> OnlyIf(Predicate<T> op)
        => this switch
        {
            T some => op(some) ? some : None.Value,
            None => None.Value,
        };
}
