namespace FunkUnions.UnitTests.Maybe;

public class OnlyIfTests : TestBaseMatchMaybe<int>
{
    [Test]
    public void OnlyIf_OkValuePredicateTrue_OriginalValueRemains()
    {
        var result = GetSome().OnlyIf(_ => true);

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void OnlyIf_OkValuePredicateFalse_NewError()
    {
        var result = GetSome().OnlyIf(_ => false);

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public void OnlyIf_ErrorValuePredicateTrue_StillNone()
    {
        var result = GetNone().OnlyIf(_ => true);

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("None"));
    }

    [Test]
    public void OnlyIf_ErrorValuePredicateFalse_StillNone()
    {
        var result = GetNone().OnlyIf(_ => false);

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("None"));
    }

    protected override string MatchMaybe(Maybe<int> res) => res switch
    {
        int i => i.ToString(),
        None => "None"
    };
}
