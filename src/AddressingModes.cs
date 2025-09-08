namespace nes;

internal sealed class AddressingModes
{
    internal struct OpCode
    {
        public byte Address { get; }
        public int Cycles { get; }

        public OpCode(byte address, int cycles)
        {
            Address = address;
            Cycles = cycles;
        }
    }

    internal static OpCode Direct() => new OpCode(0, 0);
    internal static OpCode Immediate() => new OpCode(0, 0);
    internal static OpCode Relative() => new OpCode(0, 0);
    internal static OpCode Implied() => new OpCode(0, 0);
    internal static OpCode Accumulator() => new OpCode(0, 0);
    internal static OpCode Zp() => new OpCode(0, 0);
    internal static OpCode ZpX() => new OpCode(0, 0);
    internal static OpCode ZpY() => new OpCode(0, 0);
    internal static OpCode Abs() => new OpCode(0, 0);
    internal static OpCode AbsX() => new OpCode(0, 0);
    internal static OpCode AbsY() => new OpCode(0, 0);
    internal static OpCode Ind() => new OpCode(0, 0);
    internal static OpCode IndX() => new OpCode(0, 0);
    internal static OpCode IndY() => new OpCode(0, 0);

}
