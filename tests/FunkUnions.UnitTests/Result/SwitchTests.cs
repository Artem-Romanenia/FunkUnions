using NSubstitute;

namespace FunkUnions.UnitTests.Result;

public abstract class SwitchTests : TestBase<int, Exception>
{
    public abstract void Switch(Result<int, Exception> res, ISimpleLogger logger);
    public abstract string Switch(Result<int, Exception> res);

    [Test]
    public void Switch_OkValue_MatchesOk()
    {
        var result = GetOkValue();

        var res = Switch(result);

        Assert.That(res, Is.EqualTo("2"));
    }

    [Test]
    public void Switch_OkValueNoReturn_MatchesOk()
    {
        var result = GetOkValue();

        var logger = Substitute.For<ISimpleLogger>();

        Switch(result, logger);

        logger.Received(1).LogSuccess("2");
        logger.DidNotReceive().LogError(Arg.Any<string>());
    }

    [Test]
    public void Switch_ErrorValue_MatchesError()
    {
        var result = GetErrorValue();

        var res = Switch(result);

        Assert.That(res, Is.EqualTo("Test Exception"));
    }

    [Test]
    public void Switch_ErrorValueNoReturn_MatchesError()
    {
        var result = GetErrorValue();

        var logger = Substitute.For<ISimpleLogger>();

        Switch(result, logger);

        logger.Received(1).LogError("Test Exception");
        logger.DidNotReceive().LogSuccess(Arg.Any<string>());
    }
}

public class SwitchTestsSyncOkSyncError : SwitchTests
{
    public override void Switch(Result<int, Exception> res, ISimpleLogger logger)
    {
        res.Switch(
            ok: i => logger.LogSuccess(i.ToString()),
            error: ex => logger.LogError(ex.Message));
    }

    public override string Switch(Result<int, Exception> res)
    {
        return res.Switch(
            ok: i => i.ToString(),
            error: ex => ex.Message);
    }
}

public class SwitchTestsAsyncOkSyncError : SwitchTests
{
    public override void Switch(Result<int, Exception> res, ISimpleLogger logger)
    {
        res.Switch(
            ok: async i => logger.LogSuccess(i.ToString()),
            error: ex => logger.LogError(ex.Message)).Wait();
    }

    public override string Switch(Result<int, Exception> res)
    {
        return res.Switch(
            ok: async i => i.ToString(),
            error: ex => ex.Message).Result;
    }
}

public class SwitchTestsSyncOkAsyncError : SwitchTests
{
    public override void Switch(Result<int, Exception> res, ISimpleLogger logger)
    {
        res.Switch(
            ok: i => logger.LogSuccess(i.ToString()),
            error: async ex => logger.LogError(ex.Message)).Wait();
    }

    public override string Switch(Result<int, Exception> res)
    {
        return res.Switch(
            ok: i => i.ToString(),
            error: async ex => ex.Message).Result;
    }
}

public class SwitchTestsAsyncOkAsyncError : SwitchTests
{
    public override void Switch(Result<int, Exception> res, ISimpleLogger logger)
    {
        res.Switch(
            ok: async i => logger.LogSuccess(i.ToString()),
            error: async ex => logger.LogError(ex.Message)).Wait();
    }

    public override string Switch(Result<int, Exception> res)
    {
        return res.Switch(
            ok: async i => i.ToString(),
            error: async ex => ex.Message).Result;
    }
}
