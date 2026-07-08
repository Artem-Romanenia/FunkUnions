using NSubstitute;

namespace FunkUnions.UnitTests.ShowCaseTests;

public abstract class ResourcesScenario : TestBase
{
    protected readonly DateTime _today = new DateTime(2026, 06, 15);
    protected ISimpleLogger _logger;
    protected IResourceProvider _resourceProvider;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ISimpleLogger>();
        _resourceProvider = Substitute.For<IResourceProvider>();
    }

    [Test]
    public void FluentSyntax()
    {
        var distinctResourceTypes = GetResourceIds().Select(_resourceProvider.GetById)
            .AsResult().MapError(err => new ResourceError(err))
            .OnlyIf(e => e.Any(), _ => new NoResourcesFound())
            .NestedMap(r => r.Type)
            .Map(e => string.Join(", ", e.Distinct()));

        var matched = MatchRes(distinctResourceTypes);

        AssertMatched(matched);
    }

    [Test]
    public void ComprehensionSyntax()
    {
        var distinctResourceTypes =
            from b in GetResourceIds().Select(_resourceProvider.GetById).AsResult().MapError(err => new ResourceError(err))
            where b.Any()
            let distinctResources = b.Select(r => r.Type).Distinct()
            select string.Join(", ", distinctResources);

        var matched = MatchRes(distinctResourceTypes);

        AssertMatched(matched);
    }

    public union ResourceError(DbError, NoResourcesFound) : FunkUnions.Result.IDefaultError<ResourceError>
    {
        public static ResourceError DefaultError => new NoResourcesFound();
    }

    public interface IResourceProvider
    {
        Result<Resource, DbError> GetById(int id);
    }

    protected abstract int[] GetResourceIds();
    protected abstract void AssertMatched(string matched);

    protected string MatchRes(Result<string, ResourceError> res) => res switch
    {
        string resources => resources,
        ResourceError error => error switch
        {
            DbError => nameof(DbError),
            NoResourcesFound => nameof(NoResourcesFound),
        }
    };

    public record Resource(int Id, string Type);
    public class DbError();
    public class NoResourcesFound();
}

public class ResourcesScenarioResourcesFound : ResourcesScenario
{
    protected override int[] GetResourceIds() => [1, 2, 3];

    [SetUp]
    public void SetupInner()
    {
        _resourceProvider.GetById(1).Returns(new Resource(1, "Resource_1"));
        _resourceProvider.GetById(2).Returns(new Resource(2, "Resource_2"));
        _resourceProvider.GetById(3).Returns(new Resource(3, "Resource_3"));
    }

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("Resource_1, Resource_2, Resource_3"));

        _resourceProvider.Received(1).GetById(1);
        _resourceProvider.Received(1).GetById(2);
        _resourceProvider.Received(1).GetById(3);
    }
}

public class ResourcesScenarioResourcesFoundDistinct : ResourcesScenario
{
    protected override int[] GetResourceIds() => [1, 2, 3];

    [SetUp]
    public void SetupInner()
    {
        _resourceProvider.GetById(1).Returns(new Resource(1, "Resource_1"));
        _resourceProvider.GetById(2).Returns(new Resource(2, "Resource_2"));
        _resourceProvider.GetById(3).Returns(new Resource(3, "Resource_1"));
    }

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("Resource_1, Resource_2"));

        _resourceProvider.Received(1).GetById(1);
        _resourceProvider.Received(1).GetById(2);
        _resourceProvider.Received(1).GetById(3);
    }
}

public class ResourcesScenarioResourceNotFound : ResourcesScenario
{
    protected override int[] GetResourceIds() => [1, 2, 3];

    [SetUp]
    public void SetupInner()
    {
        _resourceProvider.GetById(1).Returns(new Resource(1, "Resource_1"));
        _resourceProvider.GetById(2).Returns(new DbError());
        _resourceProvider.GetById(3).Returns(new Resource(3, "Resource_3"));
    }

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("DbError"));

        _resourceProvider.Received(1).GetById(1);
        _resourceProvider.Received(1).GetById(2);
        _resourceProvider.DidNotReceive().GetById(3);
    }
}

public class ResourcesScenarioNoResourcesFound : ResourcesScenario
{
    protected override int[] GetResourceIds() => [];

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("NoResourcesFound"));
    }
}
