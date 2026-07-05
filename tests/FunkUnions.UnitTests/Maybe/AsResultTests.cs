namespace FunkUnions.UnitTests.Maybe;

public class AsResultTests : TestBaseMatchResult<int, Exception>
{
    [Test]
    public void SomeValue_MatchesSome()
    {
        var result = GetSome().AsResult(() => new Exception("Exception"));

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void NoneValue_MatchesNone()
    {
       var result = GetNone().AsResult(() => new Exception("Exception"));

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("Exception"));
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
