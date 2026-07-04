namespace FunkUnions;

public static partial class Result
{
    public interface IDefaultError<TError>
    {
        static abstract TError DefaultError { get; }
    }
}
