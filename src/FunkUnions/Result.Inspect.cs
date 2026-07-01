namespace FunkUnions;

public partial union Result<T, TError>
{
    public readonly void InspectOk(Action<T> ok)
        => Switch(ok, static _ => { });

    public readonly async Task InspectOk(Func<T, Task> ok)
        => await Switch(ok, static _ => { });

    public readonly void InspectError(Action<TError> error)
        => Switch(static _ => { }, error);

    public readonly async Task InspectError(Func<TError, Task> error)
        => await Switch(static _ => { }, error);
}
