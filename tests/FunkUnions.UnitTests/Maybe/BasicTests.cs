namespace FunkUnions.UnitTests.Maybe;

public class BasicTests : TestBaseMatchMaybe<int>
{
    [Test]
    public void SomeValue_MatchesSome()
    {
        var result = GetSome();

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void NoneValue_MatchesNone()
    {
        var result = GetNone();

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("None"));
    }

    protected override string MatchMaybe(Maybe<int> res) => res switch
    {
        int i => i.ToString(),
        None => "None"
    };
}
