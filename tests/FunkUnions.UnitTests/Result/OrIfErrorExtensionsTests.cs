namespace FunkUnions.UnitTests.Result;

public class OrIfErrorExtensionTests : TestBaseMatchResult<Either<int, long>, string>
{
    [Test]
    public void OrIfError_OkValueOkContinuation_RemainsOriginalValue()
    {
        var squareResult = GetOkValue().OrIfError(FallbackOk);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public async Task OrIfError_Async_OkValueOkContinuation_RemainsOriginalValue()
    {
        var squareResult = await GetOkValue().OrIfError(FallbackOkAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void OrIfError_ErrorValueOkContinuation_HasNewValue()
    {
        var squareResult = GetErrorValue().OrIfError(FallbackOk);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public async Task OrIfError_Async_ErrorValueOkContinuation_HasNewValue()
    {
        var squareResult = await GetErrorValue().OrIfError(FallbackOkAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public void OrIfError_ErrorValueErrorContinuation_IsNewError()
    {
        var squareResult = GetErrorValue().OrIfError(FallbackError);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Fallback Exception"));
    }

    [Test]
    public async Task OrIfError_Async_ErrorValueErrorContinuation_IsNewError()
    {
        var squareResult = await GetErrorValue().OrIfError(FallbackErrorAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Fallback Exception"));
    }

    [Test]
    public void OrIfError_OkValueErrorContinuation_RemainsOriginalValue()
    {
        var squareResult = GetOkValue().OrIfError(FallbackError);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public async Task OrIfError_Async_OkValueErrorContinuation_RemainsOriginalValue()
    {
        var squareResult = await GetOkValue().OrIfError(FallbackErrorAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    private Result<long, string> FallbackOk(Exception ex) => long.MaxValue;

    private Result<long, string> FallbackError(Exception ex) => "Fallback Exception";

    private async Task<Result<long, string>> FallbackOkAsync(Exception ex)
    {
        await Task.Delay(1);
        return long.MaxValue;
    }

    private async Task<Result<long, string>> FallbackErrorAsync(Exception ex)
    {
        await Task.Delay(1);
        return "Fallback Exception";
    }

    protected override string MatchRes(Result<Either<int, long>, string> res)
    {
        return res switch
        {
            Either<int, long> v => v switch
            {
                int i => i.ToString(),
                long l => l.ToString()
            },
            string str => str
        };
    }
}
