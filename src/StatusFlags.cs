namespace nes;

public struct StatusFlags
{
    public const ushort Carry = 0; // C
    public const ushort Zero = 1; // Z
    public const ushort InterruptDisable = 2; // I
    public const ushort Decimal = 3; // D
    public const ushort Break = 4; // B
    public const ushort U = 4; // Unused (no CPU effect; aways pushed as 1)
    public const ushort V = 6; // Overflow
    public const ushort Negative = 7; // N
}
