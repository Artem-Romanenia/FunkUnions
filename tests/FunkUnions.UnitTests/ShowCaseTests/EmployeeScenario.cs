using NSubstitute;

namespace FunkUnions.UnitTests.ShowCaseTests;

public abstract class EmployeeScenario : TestBase
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
        var employeeId = 123;

        var replacementResult = GetEmployeeById(employeeId).MapError(err => new EquipmentReplacementError(err))
            .AndIfOk(e => GetEmployeeEquipment(e.Id, "laptop").Map(eq => (employee: e, equipment: eq)), () => new EquipmentNotFoundError())
            .InspectOk(d => _logger.LogSuccess($"Equipment {d.equipment.Id} found."))
            .OnlyIf(d => _today - d.equipment.SuppliedOn > TimeSpan.FromDays(365), _ => new EquipmentReplacementUnnecessaryError())
            .AndIfOk(d => InitiateEquipmentReplacement(d.employee.Id, d.equipment.Id).MapError(err => new EquipmentReplacementError(err)))
            .InspectOk(d => _logger.LogSuccess($"Equipment {123} replacement initiated."));

        var matched = MatchRes(replacementResult);

        AssertMatched(matched);
    }

    [Test]
    public void ComprehensionSyntax()
    {
        var employeeId = 123;

        var replacementResult =
            from employee in GetEmployeeById(employeeId).MapError(err => new EquipmentReplacementError(err))
            from equipment in GetEmployeeEquipment(employee.Id, "laptop").AsResult<EquipmentReplacementError>(() => new EquipmentNotFoundError())
            let _1 = QueryHelpers.Void(() => _logger.LogSuccess($"Equipment {equipment.Id} found."))
            where _today - equipment.SuppliedOn > TimeSpan.FromDays(365)
            from r in InitiateEquipmentReplacement(employee.Id, equipment.Id).MapError(err => new EquipmentReplacementError(err))
            let _2 = QueryHelpers.Void(() => _logger.LogSuccess($"Equipment {123} replacement initiated."))
            select r;

        var matched = MatchRes(replacementResult);

        AssertMatched(matched);
    }

    public union EquipmentReplacementError(DbError, EquipmentNotFoundError, EquipmentReplacementUnnecessaryError) : FunkUnions.Result.IDefaultError<EquipmentReplacementError>
    {
        public static EquipmentReplacementError DefaultError => new EquipmentReplacementUnnecessaryError();
    }

    protected abstract Result<Employee, DbError> GetEmployeeById(int Id);
    protected abstract Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType);
    protected abstract Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId);

    protected abstract void AssertMatched(string matched);

    protected string MatchRes(Result<None, EquipmentReplacementError> res) => res switch
    {
        None body => "replaced",
        EquipmentReplacementError error => error switch
        {
            DbError => nameof(DbError),
            EquipmentNotFoundError => nameof(EquipmentNotFoundError),
            EquipmentReplacementUnnecessaryError => nameof(EquipmentReplacementUnnecessaryError)
        }
    };

    public record Employee(int Id);
    public record Equipment(int Id, string Type, DateTime SuppliedOn);
    public class DbError();
    public class EquipmentNotFoundError();
    public class EquipmentReplacementUnnecessaryError();
}

public class EmployeeScenarioEquipmentReplaced : EmployeeScenario
{
    protected override Result<Employee, DbError> GetEmployeeById(int Id) => new Employee(1);

    protected override Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType) => new Equipment(123, "laptop", new DateTime(2024, 10, 01));

    protected override Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId) => None.Value;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("replaced"));

        _logger.Received(1).LogSuccess("Equipment 123 found.");
        _logger.Received(1).LogSuccess("Equipment 123 replacement initiated.");
    }
}

public class EmployeeScenarioReplacementUnnecessary : EmployeeScenario
{
    protected override Result<Employee, DbError> GetEmployeeById(int Id) => new Employee(1);

    protected override Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType) => new Equipment(123, "laptop", new DateTime(2025, 10, 01));

    protected override Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId) => None.Value;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("EquipmentReplacementUnnecessaryError"));

        _logger.Received(1).LogSuccess("Equipment 123 found.");
        _logger.DidNotReceive().LogSuccess("Equipment 123 replacement initiated.");
    }
}

public class EmployeeScenarioEmployeeNotFound : EmployeeScenario
{
    protected override Result<Employee, DbError> GetEmployeeById(int Id) => new DbError();

    protected override Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType) => new Equipment(123, "laptop", new DateTime(2024, 10, 01));

    protected override Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId) => None.Value;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("DbError"));

        _logger.DidNotReceive().LogSuccess("Equipment 123 found.");
        _logger.DidNotReceive().LogSuccess("Equipment 123 replacement initiated.");
    }
}

public class EmployeeScenarioEquipmentNotFound : EmployeeScenario
{
    protected override Result<Employee, DbError> GetEmployeeById(int Id) => new Employee(1);

    protected override Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType) => None.Value;

    protected override Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId) => None.Value;

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("EquipmentNotFoundError"));

        _logger.DidNotReceive().LogSuccess("Equipment 123 found.");
        _logger.DidNotReceive().LogSuccess("Equipment 123 replacement initiated.");
    }
}

public class EmployeeScenarioReplacementNoSuccess : EmployeeScenario
{
    protected override Result<Employee, DbError> GetEmployeeById(int Id) => new Employee(1);

    protected override Maybe<Equipment> GetEmployeeEquipment(int employeeId, string equipmentType) => new Equipment(123, "laptop", new DateTime(2024, 10, 01));

    protected override Result<None, DbError> InitiateEquipmentReplacement(int employeeId, int equipmentId) => new DbError();

    protected override void AssertMatched(string matched)
    {
        Assert.That(matched, Is.EqualTo("DbError"));

        _logger.Received(1).LogSuccess("Equipment 123 found.");
        _logger.DidNotReceive().LogSuccess("Equipment 123 replacement initiated.");
    }
}
