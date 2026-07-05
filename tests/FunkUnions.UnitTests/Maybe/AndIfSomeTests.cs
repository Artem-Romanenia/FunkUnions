namespace FunkUnions.UnitTests.Maybe;

public class AndIfSomeTests : TestBaseMatchMaybe<int>
{
    [Test]
    public void AndIfSome_SomeValueSomeContinuation_ValueMapped()
    {
        var squareResult = GetSome().AndIfSome(SquareSome);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public async Task AndIfSome_Async_SomeValueSomeContinuation_ValueMapped()
    {
        var squareResult = await GetSome().AndIfSome(SquareSomeAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public void AndIfSome_NoneValueSomeContinuation_RemainsNone()
    {
        var squareResult = GetNone().AndIfSome(SquareSome);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public async Task AndIfSome_Async_NoneValueSomeContinuation_RemainsNone()
    {
        var squareResult = await GetNone().AndIfSome(SquareSomeAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public void AndIfSome_NoneValueNoneContinuation_RemainsNone()
    {
        var squareResult = GetNone().AndIfSome(SquareNone);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public async Task AndIfSome_Async_NoneValueNoneContinuation_RemainsNone()
    {
        var squareResult = await GetNone().AndIfSome(SquareNoneAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public void AndIfSome_SomeValueNoneContinuation_ContinuationNone()
    {
        var squareResult = GetSome().AndIfSome(SquareNone);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public async Task AndIfSome_Async_SomeValueNoneContinuation_ContinuationNone()
    {
        var squareResult = await GetSome().AndIfSome(SquareNoneAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    private Maybe<int> SquareSome(int num) => num * num;

    private Maybe<int> SquareNone(int num) => None.Value;

    private async Task<Maybe<int>> SquareSomeAsync(int num)
    {
        await Task.Delay(1);
        return num * num;
    }

    private async Task<Maybe<int>> SquareNoneAsync(int num)
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
