namespace FunkUnions.UnitTests.Result;

public class BasicTests : TestBaseMatch<int, Exception>
{
    [Test]
    public void OkValue_MatchesOk()
    {
        var result = GetOkValue();

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void ErrorValue_MatchesError()
    {
        var result = GetErrorValue();

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("Test Exception"));
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
