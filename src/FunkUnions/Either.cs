namespace FunkUnions;

public union Either<T, T2>(T, T2)
    where T : notnull
    where T2 : notnull;
