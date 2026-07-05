namespace FunkUnions.UnitTests.Result;

public class OnlyIfTests : TestBaseMatchResult<int, Exception>
{
    [Test]
    public void OnlyIf_OkValuePredicateTrue_OriginalValueRemains()
    {
        var result = GetOkValue().OnlyIf(_ => true, _ => new Exception("Filtered"));

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void OnlyIf_OkValuePredicateFalse_NewError()
    {
        var result = GetOkValue().OnlyIf(_ => false, _ => new Exception("Filtered"));

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("Filtered"));
    }

    [Test]
    public void OnlyIf_ErrorValuePredicateTrue_OriginalErrorRemains()
    {
        var result = GetErrorValue().OnlyIf(_ => true, _ => new Exception("Filtered"));

        var res = MatchRes(result);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void OnlyIf_ErrorValuePredicateFalse_OriginalErrorRemains()
    {
        var result = GetErrorValue().OnlyIf(_ => false, _ => new Exception("Filtered"));

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
