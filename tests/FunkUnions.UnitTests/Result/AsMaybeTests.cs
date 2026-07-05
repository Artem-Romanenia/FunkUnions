namespace FunkUnions.UnitTests.Result;

public class AsMaybeTests : TestBaseMatchMaybe<int>
{
    [Test]
    public void OkValue_MatchesOk()
    {
        var result = GetOkValue().AsMaybe();

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void ErrorValue_MatchesError()
    {
        var result = GetErrorValue().AsMaybe();

        var res = MatchMaybe(result);

        Assert.That(res, Is.EqualTo("None"));
    }

    protected override string MatchMaybe(Maybe<int> res) => res switch
    {
        int i => i.ToString(),
        None => "None"
    };
}
