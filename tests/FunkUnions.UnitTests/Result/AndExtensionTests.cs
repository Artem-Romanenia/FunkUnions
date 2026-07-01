namespace FunkUnions.UnitTests.Result;

public class AndExtensionTests : TestBaseMatch<long, Either<Exception, string>>
{
    [Test]
    public void And_OkValueOkContinuation_ValueMapped()
    {
        var squareResult = GetOkValue().And(GetNextValueOk());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
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

    private Result<long, string> GetNextValueOk() => long.MaxValue;

    private Result<long, string> GetNextValueError() => "Next Value Exception";

    protected override string MatchRes(Result<long, Either<Exception, string>> res)
    {
        return res switch
        {
            long l => l.ToString(),
            Either<Exception, string> error => error switch
            {
                Exception ex => ex.Message,
                string str => str
            }
        };
    }
}
