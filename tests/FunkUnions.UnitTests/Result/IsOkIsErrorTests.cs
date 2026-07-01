namespace FunkUnions.UnitTests.Result;

public class IsOkIsErrorTests : TestBase<int, Exception>
{
    [Test]
    public void OkValue_IsOk()
    {
        var result = GetOkValue();

        Assert.That(result.IsOk);
        Assert.That(result.IsOkAnd(v => v is 2));
        Assert.That(!result.IsError);
    }

    [Test]
    public void ErrorValue_IsError()
    {
        var result = GetErrorValue();

        Assert.That(!result.IsOk);
        Assert.That(result.IsError);
        Assert.That(result.IsErrorAnd(e => e is { Message: "Test Exception" }));
    }
}
