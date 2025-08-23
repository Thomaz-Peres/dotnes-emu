namespace nes;

internal sealed class AddressingModes
{
    // internal struct OpCode
    // {
    //     public byte Address { get; }
    //     public int Cycles { get; }

    //     public OpCode(byte address, int cycles)
    //     {
    //         Address = address;
    //         Cycles = cycles;
    //     }
    // }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    internal sealed class OpCodeAttribute : Attribute
    {
        public byte Address { get; }
        public int Cycles { get; }
        public AddressingMode Mode { get; }

        public OpCodeAttribute(byte address, int cycles, AddressingMode mode)
        {
            Address = address;
            Cycles = cycles;
            Mode = mode;
        }
    }

    internal enum AddressingMode
    {
        None,
        Direct,
        Immediate,
        Relative,
        Implied,
        Accumulator,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Absolute,
        AbsoluteX,
        AbsoluteY,
        IndirectX,
        IndirectY,
    }


    // internal OpCode Direct() => new OpCode(0, 0);
    // internal OpCode Immediate() => new OpCode(0, 0);
    // internal OpCode Relative() => new OpCode(0, 0);
    // internal OpCode Implied() => new OpCode(0, 0);
    // internal OpCode Accumulator() => new OpCode(0, 0);
    // internal OpCode Zp() => new OpCode(0, 0);
    // internal OpCode ZpX() => new OpCode(0, 0);
    // internal OpCode ZpY() => new OpCode(0, 0);
    // internal OpCode Abs() => new OpCode(0, 0);
    // internal OpCode AbsX() => new OpCode(0, 0);
    // internal OpCode AbsY() => new OpCode(0, 0);
    // internal OpCode Ind() => new OpCode(0, 0);
    // internal OpCode IndX() => new OpCode(0, 0);
    // internal OpCode IndY() => new OpCode(0, 0);

}
