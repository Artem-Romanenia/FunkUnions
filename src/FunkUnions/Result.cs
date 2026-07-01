namespace FunkUnions;

public partial union Result<T, TError>(T, TError)
    where T : notnull
    where TError : notnull;

public static partial class ResultExtensions;
