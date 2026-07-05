using NSubstitute;

namespace FunkUnions.UnitTests.Maybe;

public class InspectTests : TestBase<int, Exception>
{
    [Test]
    public void Inspect_SomeValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetSome().Inspect(v => logger.LogSuccess(v.ToString()));

        logger.Received(1).LogSuccess("2");
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public async Task Inspect_Async_SomeValue_Inspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetSome().Inspect(async v => logger.LogSuccess(v.ToString()));

        logger.Received(1).LogSuccess("2");
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public void Inspect_NoneValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        GetNone().Inspect(v => logger.LogSuccess(v.ToString()));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public async Task Inspect_Async_NoneValue_NotInspected()
    {
        var logger = Substitute.For<ISimpleLogger>();

        await GetNone().Inspect(async v => logger.LogSuccess(v.ToString()));

        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }
}
