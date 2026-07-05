namespace FunkUnions.UnitTests.Result;

public class MapErrorAsUnionWithTests : TestBaseMatchResult<int, Either<Exception, string>>
{
    [Test]
    public void MapErrorAsUnionWith_OkValue_RemainsOriginal()
    {
        var squareResult = GetOkValue().MapErrorAsUnionWith<string>();

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void MapErrorAsUnionWith_ErrorValue_Mapped()
    {
        var squareResult = GetErrorValue().MapErrorAsUnionWith<string>();

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    protected override string MatchRes(Result<int, Either<Exception, string>> res)
    {
        return res switch
        {
            int i => i.ToString(),
            Either<Exception, string> error => error switch
            {
                Exception ex => ex.Message,
                string str => str
            }
        };
    }
}
