namespace FunkUnions.UnitTests.Result;

public class OrTests : TestBaseMatch<int, string>
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

        Assert.That(res, Is.EqualTo("42"));
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

    private Result<int, string> FallbackOk() => 42;

    private Result<int, string> FallbackError() => "Fallback Exception";

    protected override string MatchRes(Result<int, string> res)
    {
        return res switch
        {
            int i => i.ToString(),
            string str => str
        };
    }
}
