public sealed record StatusFlags
{
    public const byte Carry = 1 << 0;
    internal const byte Zero = 1 << 1;
    internal const byte InterruptDisable = 1 << 2;
    internal const byte Break = 1 << 3;
    internal const byte Decimal = 1 << 4;
    internal const byte V = 1 << 5; // Overflow
    internal const byte Negative = 1 << 6;
}
