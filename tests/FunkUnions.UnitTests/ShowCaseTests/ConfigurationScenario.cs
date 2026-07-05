using NSubstitute;

namespace FunkUnions.UnitTests.ShowCaseTests;

public abstract class ConfigurationScenario : TestBase
{
    protected readonly DateTime _today = new DateTime(2026, 06, 15);
    protected ISimpleLogger _logger;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ISimpleLogger>();
    }

    [Test]
    public void FluentSyntax()
    {
        var config = new Configuration();

        var maybeNotification = GetConfigurationSection(config, "NotificationSchedule")
            .AndIfSome(s => GetConfigurationValue(s, "DayOfMonth")
            .Map(int.Parse))
            .OnlyIf(d => d == _today.Day)
            .Inspect(_ => _logger.LogSuccess("Notification day detected."))
            .AndIfSome(d => GetLastNotification()
                .Map(n => n.SentOn)
                .OrIfNone(() => DateTime.MinValue))
            .OnlyIf(d => d < _today)
            .Inspect(_ => _logger.LogSuccess($"Notification will be sent for date '{_today}'"))
            .Map(_ => GetNotificationBody(_today));

        var matched = MatchRes(maybeNotification);

        AssertMatched(matched);
    }

    [Test]
    public void ComprehensionSyntax()
    {
        var config = new Configuration();

        var maybeNotification =
            from section in GetConfigurationSection(config, "NotificationSchedule")
            from dayOfMonth in GetConfigurationValue(section, "DayOfMonth").Map(int.Parse)
            where dayOfMonth == _today.Day
            let _1 = QueryHelpers.Void(() => _logger.LogSuccess("Notification day detected."))
            from d in GetLastNotification().Map(n => n.SentOn).OrIfNone(() => DateTime.MinValue)
            where d < _today
            let _2 = QueryHelpers.Void(() => _logger.LogSuccess($"Notification will be sent for date '{_today}'"))
            select GetNotificationBody(_today);

        var matched = MatchRes(maybeNotification);

        AssertMatched(matched);
    }

    protected abstract Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName);
    protected abstract Maybe<string> GetConfigurationValue(Configuration config, string settingName);
    protected abstract Maybe<Notification> GetLastNotification();
    protected string GetNotificationBody(DateTime date) => "Notification body";

    protected abstract void AssertMatched(string matched);

    protected string MatchRes(Maybe<string> res) => res switch
    {
        string body => body,
        None => "None"
    };

    public class Configuration;
    public record Notification(int Id, DateTime SentOn);
}

public class ConfigurationScenarioNotificationInPast : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => new Configuration();

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => "15";

    protected override Maybe<Notification> GetLastNotification() => new Notification(1, new DateTime(2026, 1, 1));

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("Notification body"));
        _logger.Received(1).LogSuccess("Notification day detected.");
        _logger.Received(1).LogSuccess($"Notification will be sent for date '{_today}'");
    }
}

public class ConfigurationScenarioNoSection : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => None.Value;

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => "15";

    protected override Maybe<Notification> GetLastNotification() => new Notification(1, new DateTime(2026, 1, 1));

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("None"));
        _logger.DidNotReceive().LogSuccess("Notification day detected.");
        _logger.DidNotReceive().LogSuccess($"Notification will be sent for date '{_today}'");
    }
}

public class ConfigurationScenarioNoValue : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => new Configuration();

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => None.Value;

    protected override Maybe<Notification> GetLastNotification() => new Notification(1, new DateTime(2026, 1, 1));

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("None"));
        _logger.DidNotReceive().LogSuccess("Notification day detected.");
        _logger.DidNotReceive().LogSuccess($"Notification will be sent for date '{_today}'");
    }
}

public class ConfigurationScenarioNoNotification : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => new Configuration();

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => "15";

    protected override Maybe<Notification> GetLastNotification() => None.Value;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("Notification body"));
        _logger.Received(1).LogSuccess("Notification day detected.");
        _logger.Received(1).LogSuccess($"Notification will be sent for date '{_today}'");
    }
}

public class ConfigurationScenarioNotToday : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => new Configuration();

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => "25";

    protected override Maybe<Notification> GetLastNotification() => new Notification(1, new DateTime(2026, 1, 1));

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("None"));
        _logger.DidNotReceive().LogSuccess("Notification day detected.");
        _logger.DidNotReceive().LogSuccess($"Notification will be sent for date '{_today}'");
    }
}

public class ConfigurationScenarioAlreadySent : ConfigurationScenario
{
    protected override Maybe<Configuration> GetConfigurationSection(Configuration config, string sectionName) => new Configuration();

    protected override Maybe<string> GetConfigurationValue(Configuration config, string settingName) => "15";

    protected override Maybe<Notification> GetLastNotification() => new Notification(1, new DateTime(2026, 6, 15, 10, 0, 0));

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("None"));
        _logger.Received(1).LogSuccess("Notification day detected.");
        _logger.DidNotReceive().LogSuccess($"Notification will be sent for date '{_today}'");
    }
}
