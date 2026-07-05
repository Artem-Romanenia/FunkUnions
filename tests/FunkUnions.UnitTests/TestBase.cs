namespace FunkUnions.UnitTests;

public abstract class TestBaseMatchResult<T, TError> : TestBase<T, TError>
    where T : notnull
    where TError : notnull
{
    protected abstract string MatchRes(Result<T, TError> res);
}

public abstract class TestBaseMatchMaybe<T> : TestBase<T, object>
    where T : notnull
{
    protected abstract string MatchMaybe(Maybe<T> res);
}

public abstract class TestBase<T, TError> : TestBase
    where T : notnull
    where TError : notnull
{
    protected Result<int, Exception> GetOkValue() => 2;

    protected Result<int, Exception> GetErrorValue() => new Exception("Test Exception");

    protected Maybe<int> GetSome() => 2;

    protected Maybe<int> GetNone() => None.Value;
}

public abstract class TestBase
{
    public interface ISimpleLogger
    {
        void LogSuccess(string message);
        void LogError(string message);
    }
}
