namespace FunkUnions;

public class QueryHelpers
{
    public static ValueTuple Void(Action op)
    {
        op();
        return ValueTuple.Create();
    }
}
