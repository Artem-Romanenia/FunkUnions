namespace FunkUnions.UnitTests.Result;

public class MapTests : TestBaseMatchResult<int, Exception>
{
    [Test]
    public void Map_OkValue_Mapped()
    {
        var squareResult = GetOkValue().Map(Square);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public async Task Map_Async_OkValue_Mapped()
    {
        var squareResult = await GetOkValue().Map(SquareAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("4"));
    }

    [Test]
    public void Map_ErrorValue_RemainsOriginal()
    {
        var squareResult = GetErrorValue().Map(Square);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public async Task Map_Async_ErrorValue_RemainsOriginal()
    {
        var squareResult = await GetErrorValue().Map(SquareAsync);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    private int Square(int num) => num * num;

    private async Task<int> SquareAsync(int num)
    {
        await Task.Delay(1);
        return num * num;
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
