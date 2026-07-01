namespace FunkUnions.UnitTests.Result;

public class AndTests : TestBaseMatch<int, Exception>
{
    [Test]
    public void And_OkValueOkContinuation_ValueMapped()
    {
        var squareResult = GetOkValue().And(GetNextValueOk());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("42"));
    }

    [Test]
    public void And_ErrorValueOkContinuation_RemainsOriginalError()
    {
        var squareResult = GetErrorValue().And(GetNextValueOk());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void AndIfOk_ErrorValueErrorContinuation_RemainsOriginalError()
    {
        var squareResult = GetErrorValue().And(GetNextValueError());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void AndIfOk_OkValueErrorContinuation_ContinuationError()
    {
        var squareResult = GetOkValue().And(GetNextValueError());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Next Value Exception"));
    }

    private Result<int, Exception> GetNextValueOk() => 42;

    private Result<int, Exception> GetNextValueError() => new Exception("Next Value Exception");

    protected override string MatchRes(Result<int, Exception> res)
    {
        return res switch
        {
            int i => i.ToString(),
            Exception ex => ex.Message
        };
    }
}
