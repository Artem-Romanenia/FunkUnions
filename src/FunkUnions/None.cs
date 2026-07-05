namespace FunkUnions;

public class None
{
    private static readonly Lazy<None> lazy = new(() => new None());

    private None() { }

    public static None Value => lazy.Value;
}
