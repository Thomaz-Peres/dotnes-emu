namespace nes;

public static class StatusFlags
{
    public const byte Carry = 1 << 0; // C
    public const byte Zero = 1 << 1; // Z
    public const byte InterruptDisable = 1 << 2; // I
    public const byte Decimal = 1 << 3; // D
    public const byte Break = 1 << 4; // B
    public const byte U = 1 << 5; // Unused (no CPU effect; aways pushed as 1)
    public const byte V = 1 << 6; // Overflow
    public const byte Negative = 1 << 7; // N
}
