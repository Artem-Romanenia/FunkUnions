using NSubstitute;

namespace FunkUnions.UnitTests.Result;

public class InspectOkTests : TestBase<int, Exception>
{
    [Test]
    public void InspectOk_OkValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetOkValue().InspectOk(v => logger.LogSuccess(v.ToString()));

        logger.Received(1).LogSuccess("2");
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public async Task InspectOk_Async_OkValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetOkValue().InspectOk(async v => logger.LogSuccess(v.ToString()));

        logger.Received(1).LogSuccess("2");
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public void InspectOk_ErrorValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetErrorValue().InspectOk(v => logger.LogSuccess(v.ToString()));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public async Task InspectOk_Async_ErrorValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetErrorValue().InspectOk(async v => logger.LogSuccess(v.ToString()));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }
}
