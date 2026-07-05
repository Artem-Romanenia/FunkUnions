namespace FunkUnions.UnitTests.Maybe;

public class MapTests : TestBaseMatchMaybe<int>
{
    [Test]
    public void Map_SomeValue_Mapped()
    {
        var squareResult = GetSome().Map(Square);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public async Task Map_Async_SomeValue_Mapped()
    {
        var squareResult = await GetSome().Map(SquareAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public void Map_NoneValue_RemainsNone()
    {
        var squareResult = GetNone().Map(Square);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public async Task Map_Async_NoneValue_RemainsNone()
    {
        var squareResult = await GetNone().Map(SquareAsync);

        var res = MatchMaybe(squareResult);

        Assert.That(res, Is.EqualTo("None"));
    }

    private int Square(int num) => num * num;

    private async Task<int> SquareAsync(int num)
    {
        await Task.Delay(1);
        return num * num;
    }

    protected override string MatchMaybe(Maybe<int> res) => res switch
    {
        int i => i.ToString(),
        None => "None"
    };
}
