namespace FunkUnions.UnitTests.Result;

public class AndIfOkTests : TestBaseMatch<int, Exception>
{
    [Test]
    public void AndIfOk_OkValueOkContinuation_ValueMapped()
    {
        var squareResult = GetOkValue().AndIfOk(SquareOk);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public async Task AndIfOk_Async_OkValueOkContinuation_ValueMapped()
    {
        var squareResult = await GetOkValue().AndIfOk(SquareOkAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public void AndIfOk_ErrorValueOkContinuation_RemainsOriginalError()
    {
        var squareResult = GetErrorValue().AndIfOk(SquareOk);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public async Task AndIfOk_Async_ErrorValueOkContinuation_RemainsOriginalError()
    {
        var squareResult = await GetErrorValue().AndIfOk(SquareOkAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void AndIfOk_ErrorValueErrorContinuation_RemainsOriginalError()
    {
        var squareResult = GetErrorValue().AndIfOk(SquareError);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public async Task AndIfOk_Async_ErrorValueErrorContinuation_RemainsOriginalError()
    {
        var squareResult = await GetErrorValue().AndIfOk(SquareErrorAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void AndIfOk_OkValueErrorContinuation_ContinuationError()
    {
        var squareResult = GetOkValue().AndIfOk(SquareError);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Square Exception"));
    }

    [Test]
    public async Task AndIfOk_Async_OkValueErrorContinuation_ContinuationError()
    {
        var squareResult = await GetOkValue().AndIfOk(SquareErrorAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Square Exception"));
    }

    private Result<int, Exception> SquareOk(int num) => num * num;

    private Result<int, Exception> SquareError(int num) => new Exception("Square Exception");

    private async Task<Result<int, Exception>> SquareOkAsync(int num)
    {
        await Task.Delay(1);
        return num * num;
    }

    private async Task<Result<int, Exception>> SquareErrorAsync(int num)
    {
        await Task.Delay(1);
        return new Exception("Square Exception");
    }

    protected override string MatchRes(Result<int, Exception> res)
    {
        return res switch
        {
            int i => i.ToString(),
            Exception ex => ex.Message
        };
    }
}
