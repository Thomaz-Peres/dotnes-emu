using System.Runtime.CompilerServices;

namespace nes;

internal sealed partial class CPU
{
    internal struct OpCode
    {
        public ushort Address { get; }
        public int Cycles { get; }

        public OpCode(ushort address, int cycles)
        {
            Address = address;
            Cycles = cycles;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ushort NextByte() => Bus.ReadByte(PC++);

    internal OpCode Direct() => new OpCode(0, 0);

    internal OpCode Immediate() =>
        new OpCode(NextByte(), 0);

    internal OpCode Relative()
    {
        return new OpCode(0, 0);
    }

    internal OpCode Implied() =>
        new OpCode(A, 0);

    internal OpCode Accumulator() => new OpCode(0, 0);

    internal OpCode Zp() =>
        new OpCode(NextByte(), 0);

    internal OpCode ZpX()
    {
        ushort addr = (ushort)((NextByte() + X) & 0xFF);

        return new OpCode(addr, 4);
    }

    internal OpCode ZpY()
    {
        ushort addr = (ushort)((NextByte() + Y) & 0xFF);

        return new OpCode(addr, 4);
    }

    internal OpCode Abs()
    {
        var addr = (ushort)(NextByte() | (NextByte() << 8));
        return new OpCode(addr, 0);
    }

    internal OpCode AbsX() => new OpCode(0, 0);
    internal OpCode AbsY() => new OpCode(0, 0);
    internal OpCode Ind() => new OpCode(0, 0);
    internal OpCode IndX() => new OpCode(0, 0);
    internal OpCode IndY() => new OpCode(0, 0);
}
