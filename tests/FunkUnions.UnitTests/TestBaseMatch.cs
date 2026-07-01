namespace FunkUnions.UnitTests;

public abstract class TestBaseMatch<T, TError> : TestBase<T, TError>
    where T : notnull
    where TError : notnull
{
    protected abstract string MatchRes(Result<T, TError> res);
}

public abstract class TestBase<T, TError>
    where T : notnull
    where TError : notnull
{
    protected Result<int, Exception> GetOkValue() => 2;

    protected Result<int, Exception> GetErrorValue() => new Exception("Test Exception");

    public interface ISimpleLogger
    {
        void LogSuccess(string message);
        void LogError(string message);
    }
}
