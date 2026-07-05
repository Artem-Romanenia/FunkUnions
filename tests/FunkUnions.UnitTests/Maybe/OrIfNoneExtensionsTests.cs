namespace FunkUnions.UnitTests.Maybe;

public class OrIfNoneExtensionTests : TestBaseMatchMaybe<Either<int, long>>
{
    [Test]
    public void OrIfNone_SomeValueSomeContinuation_RemainsOriginalValue()
    {
        var squareResult = GetSome().OrIfNone(FallbackSome);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public async Task OrIfNone_Async_SomeValueSomeContinuation_RemainsOriginalValue()
    {
        var squareResult = await GetSome().OrIfNone(FallbackSomeAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void OrIfNone_NoneValueSomeContinuation_HasNewValue()
    {
        var squareResult = GetNone().OrIfNone(FallbackSome);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public async Task OrIfNone_Async_NoneValueSomeContinuation_HasNewValue()
    {
        var squareResult = await GetNone().OrIfNone(FallbackSomeAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public void OrIfNone_NoneValueNoneContinuation_StillNone()
    {
        var squareResult = GetNone().OrIfNone(FallbackNone);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public async Task OrIfNone_Async_NoneValueNoneContinuation_StillNone()
    {
        var squareResult = await GetNone().OrIfNone(FallbackNoneAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public void OrIfNone_SomeValueNoneContinuation_RemainsOriginalValue()
    {
        var squareResult = GetSome().OrIfNone(FallbackNone);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public async Task OrIfNone_Async_SomeValueNoneContinuation_RemainsOriginalValue()
    {
        var squareResult = await GetSome().OrIfNone(FallbackNoneAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    private Maybe<long> FallbackSome() => long.MaxValue;

    private Maybe<long> FallbackNone() => None.Value;

    private async Task<Maybe<long>> FallbackSomeAsync()
    {
        await Task.Delay(1);
        return long.MaxValue;
    }

    private async Task<Maybe<long>> FallbackNoneAsync()
    {
        await Task.Delay(1);
        return None.Value;
    }

    protected override string MatchMaybe(Maybe<Either<int, long>> res) => res switch
    {
        Either<int, long> v => v switch
        {
            int i => i.ToString(),
            long l => l.ToString()
        },
        None => "None"
    };
}
