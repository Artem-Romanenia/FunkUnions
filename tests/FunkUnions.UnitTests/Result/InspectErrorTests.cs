using NSubstitute;

namespace FunkUnions.UnitTests.Result;

public class InspectErrorTests : TestBase<int, Exception>
{
    [Test]
    public void InspectError_OkValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetOkValue().InspectError(ex => logger.LogError(ex.Message));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public async Task InspectError_Async_OkValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetOkValue().InspectError(async ex => logger.LogError(ex.Message));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public void InspectError_ErrorValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetErrorValue().InspectError(ex => logger.LogError(ex.Message));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.Received(1).LogError("Test Exception");
    }

    [Test]
    public async Task InspectError_Async_ErrorValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetErrorValue().InspectError(async ex => logger.LogError(ex.Message));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.Received(1).LogError("Test Exception");
    }
}
