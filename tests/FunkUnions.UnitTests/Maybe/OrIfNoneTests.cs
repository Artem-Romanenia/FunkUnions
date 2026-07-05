namespace FunkUnions.UnitTests.Maybe;

public class OrIfNoneTests : TestBaseMatchMaybe<int>
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

        Assert.That(res, Is.EqualTo("42"));
    }

    [Test]
    public async Task OrIfNone_Async_NoneValueSomeContinuation_HasNewValue()
    {
        var squareResult = await GetNone().OrIfNone(FallbackSomeAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("42"));
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

    private Maybe<int> FallbackSome() => 42;

    private Maybe<int> FallbackNone() => None.Value;

    private async Task<Maybe<int>> FallbackSomeAsync()
    {
        await Task.Delay(1);
        return 42;
    }

    private async Task<Maybe<int>> FallbackNoneAsync()
    {
        await Task.Delay(1);
        return None.Value;
    }

    protected override string MatchMaybe(Maybe<int> res) => res switch
    {
        int i => i.ToString(),
        None => "None"
    };
}
