using NSubstitute;

namespace FunkUnions.UnitTests.ShowCaseTests;

public abstract class OrderScenario : TestBase
{
    protected ISimpleLogger _logger;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ISimpleLogger>();
    }

    [Test]
    public void FluentSyntax()
    {
        var orderModel = new OrderModel(321);

        var orderPlacementResult = ValidateInput(orderModel)
            .Map(MapToDomain).MapError(err => new OrderPlacementError(err))
            .AndIfOk(o => GetItemFromCache(o.ItemId)
                .OrIfError(_ => GetItemFromDb(o.ItemId))
                .Map(i => (order: o, item: i)).MapError(err => new OrderPlacementError(err)))
            .AndIfOk(d => GetUser()
                .Map(u => (d.order, d.item, user: u)).MapError(err => new OrderPlacementError(err)))
            .OnlyIf(d => d.user.Age >= d.item.AgeLimit, _ => new AgeValidationError())
            .InspectOk(d => _logger.LogSuccess($"Order on Item {d.order.ItemId} for User {d.user.Id} was received."))
            .AndIfOk(d => PlaceOrder(d.order)
                .MapError(err => new OrderPlacementError(err)))
            .InspectOk(orderId => _logger.LogSuccess($"Order with Id {orderId} was created."));

        var matched = MatchRes(orderPlacementResult);

        AssertMatched(matched);
    }

    [Test]
    public void ComprehensionSyntax()
    {
        var orderModel = new OrderModel(321);

        var orderPlacementResult =
            from order in ValidateInput(orderModel).Map(MapToDomain).MapError(err => new OrderPlacementError(err))
            from item in GetItemFromCache(order.ItemId).OrIfError(_ => GetItemFromDb(order.ItemId)).MapError(err => new OrderPlacementError(err))
            from user in GetUser().MapError(err => new OrderPlacementError(err))
            where user.Age >= item.AgeLimit
            let _1 = QueryHelpers.Void(() => _logger.LogSuccess($"Order on Item {order.ItemId} for User {user.Id} was received."))
            from orderId in PlaceOrder(order).MapError(err => new OrderPlacementError(err))
            let _2 = QueryHelpers.Void(() => _logger.LogSuccess($"Order with Id {orderId} was created."))
            select orderId;

        var matched = MatchRes(orderPlacementResult);

        AssertMatched(matched);
    }

    public union OrderPlacementError(ModelValidationError, CacheRetrievalError, DbError, AgeValidationError) : FunkUnions.Result.IDefaultError<OrderPlacementError>
    {
        public static OrderPlacementError DefaultError => new AgeValidationError();
    }

    protected abstract Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order);
    protected Order MapToDomain(OrderModel model) => new Order(model.ItemId);
    protected abstract Result<Item, CacheRetrievalError> GetItemFromCache(int itemId);
    protected abstract Result<Item, DbError> GetItemFromDb(int itemId);
    protected abstract Result<User, CacheRetrievalError> GetUser();
    protected abstract Result<long, DbError> PlaceOrder(Order order);
    protected abstract void AssertMatched(string matched);

    protected string MatchRes(Result<long, OrderPlacementError> res) => res switch
    {
        long orderId => orderId.ToString(),
        OrderPlacementError error => error switch
        {
            ModelValidationError => nameof(ModelValidationError),
            CacheRetrievalError => nameof(CacheRetrievalError),
            DbError => nameof(DbError),
            AgeValidationError => nameof(AgeValidationError),
        }
    };

    public record OrderModel(int ItemId);
    public record Order(int ItemId);
    public record Item(int Id, int AgeLimit);
    public record User(int Id, int Age);
    public record ValidationResult(bool IsValid, ICollection<string>? Errors);
    public class ModelValidationError();
    public class CacheRetrievalError();
    public class DbError();
    public class AgeValidationError();
}

public class OrderScenarioItemFromCache : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new Item(itemId, 18);
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new User(1, 20);
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("123"));
        _logger.Received(1).LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.Received(1).LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioItemFromDb : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new CacheRetrievalError();
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new Item(itemId, 18);
    protected override Result<User, CacheRetrievalError> GetUser() => new User(1, 20);
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("123"));
        _logger.Received(1).LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.Received(1).LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioInvalidModel : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => new ModelValidationError();
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new Item(itemId, 18);
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new User(1, 20);
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("ModelValidationError"));
        _logger.DidNotReceive().LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.DidNotReceive().LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioCantGetItem : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new CacheRetrievalError();
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new CacheRetrievalError();
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("DbError"));
        _logger.DidNotReceive().LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.DidNotReceive().LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioCantGetUser : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new Item(itemId, 18);
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new CacheRetrievalError();
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("CacheRetrievalError"));
        _logger.DidNotReceive().LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.DidNotReceive().LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioAgeValidation : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new Item(itemId, 18);
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new User(1, 13);
    protected override Result<long, DbError> PlaceOrder(Order order) => 123;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("AgeValidationError"));
        _logger.DidNotReceive().LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.DidNotReceive().LogSuccess("Order with Id 123 was created.");
    }
}

public class OrderScenarioCantPlaceOrder : OrderScenario
{
    protected override Result<OrderModel, ModelValidationError> ValidateInput(OrderModel order) => order;
    protected override Result<Item, CacheRetrievalError> GetItemFromCache(int itemId) => new Item(itemId, 18);
    protected override Result<Item, DbError> GetItemFromDb(int itemId) => new DbError();
    protected override Result<User, CacheRetrievalError> GetUser() => new User(1, 20);
    protected override Result<long, DbError> PlaceOrder(Order order) => new DbError();

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("DbError"));
        _logger.Received(1).LogSuccess("Order on Item 321 for User 1 was received.");
        _logger.DidNotReceive().LogSuccess("Order with Id 123 was created.");
    }
}
