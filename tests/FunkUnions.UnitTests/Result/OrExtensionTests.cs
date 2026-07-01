namespace FunkUnions.UnitTests.Result;

public class OrExtensionTests : TestBaseMatch<Either<int, long>, string>
{
    [Test]
    public void Or_OkValueOkContinuation_RemainsOriginalValue()
    {
        var squareResult = GetOkValue().Or(FallbackOk());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void Or_ErrorValueOkContinuation_HasNewValue()
    {
        var squareResult = GetErrorValue().Or(FallbackOk());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public void Or_ErrorValueErrorContinuation_IsNewError()
    {
        var squareResult = GetErrorValue().Or(FallbackError());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Fallback Exception"));
    }

    [Test]
    public void Or_OkValueErrorContinuation_RemainsOriginalValue()
    {
        var squareResult = GetOkValue().Or(FallbackError());

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    private Result<long, string> FallbackOk() => long.MaxValue;

    private Result<long, string> FallbackError() => "Fallback Exception";

    protected override string MatchRes(Result<Either<int, long>, string> res)
    {
        return res switch
        {
            Either<int, long> okVal => okVal switch
            {
                int i => i.ToString(),
                long l => l.ToString()
            },
            string str => str
        };
    }
}
