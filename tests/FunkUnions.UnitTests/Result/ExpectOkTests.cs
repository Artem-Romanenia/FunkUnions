namespace FunkUnions.UnitTests.Result;

public class ExpectOkTests : TestBaseMatch<int, Exception>
{
    [Test]
    public void ExpectOk_OkValue_ReturnsOk()
    {
        var result = GetOkValue();

        Assert.That(result.ExpectOk(), Is.EqualTo(2));
    }

    [Test]
    public void ExpectOk_ErrorValue_ThrowsException()
    {
        var result = GetErrorValue();

        Assert.Throws<OkResultExpectedException>(() => result.ExpectOk("Expected Exception"), "Expected Exception");
    }

    [Test]
    public void ExpectOk_ErrorValue_ThrowsExceptionWithDefaultMessage()
    {
        var result = GetErrorValue();

        try
        {
            _ = result.ExpectOk();
        }
        catch (OkResultExpectedException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Result object was expected to have Ok value, but has Error value instead."));
            Assert.Pass();
            return;
        }

        Assert.Fail();
    }

    [Test]
    public void ExpectOk_ErrorValue_ThrowsCustomException()
    {
        var result = GetErrorValue();

        try
        {
            _ = result.ExpectOk(e => new CustomException(e.Message));
        }
        catch (CustomException ex)
        {
            Assert.That(ex.StackTrace, Does.Match("at FunkUnions.Result`2.ExpectOk\\(Func`2 customException\\) in .*?Result.ExpectOk.cs:line [0-9]*"));
            Assert.That(ex.StackTrace, Does.Match("at FunkUnions.UnitTests.Result.ExpectOkTests.ExpectOk_ErrorValue_ThrowsCustomException\\(\\) in .*?ExpectOkTests.cs:line [0-9]*"));
            Assert.Pass();
            return;
        }

        Assert.Fail();
    }

    protected override string MatchRes(Result<int, Exception> res)
    {
        return res switch
        {
            int i => i.ToString(),
            Exception ex => ex.Message
        };
    }

    private class CustomException(string message) : Exception(message);
}
