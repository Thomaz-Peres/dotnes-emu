namespace nes;

public static class StatusFlags
{
    public const ushort Carry = 1 << 0; // C
    public const ushort Zero = 1 << 1; // Z
    public const ushort InterruptDisable = 1 << 2; // I
    public const ushort Decimal = 1 << 3; // D
    public const ushort Break = 1 << 4; // B
    public const ushort U = 1 << 4; // Unused (no CPU effect; aways pushed as 1)
    public const ushort V = 1 << 6; // Overflow
    public const ushort Negative = 1 << 7; // N
}
