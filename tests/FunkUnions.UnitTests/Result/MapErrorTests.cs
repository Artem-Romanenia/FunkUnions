namespace FunkUnions.UnitTests.Result;

public class MapErrorTests : TestBaseMatchResult<int, string>
{
    [Test]
    public void MapErr_OkValue_RemainsOriginal()
    {
        var squareResult = GetOkValue().MapError(GetExceptionMessage);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void MapError_ErrorValue_Mapped()
    {
        var squareResult = GetErrorValue().MapError(GetExceptionMessage);

        var res = MatchRes(squareResult);

        Assert.That(res, Is.EqualTo("Mapped Test Exception"));
    }

    private string GetExceptionMessage(Exception ex) => $"Mapped {ex.Message}";

    protected override string MatchRes(Result<int, string> res)
    {
        return res switch
        {
            int i => i.ToString(),
            string str => str
        };
    }
}
