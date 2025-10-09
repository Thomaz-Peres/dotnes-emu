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
    private byte NextByte() => Bus.ReadByte(PC++);

    internal OpCode Direct() => new OpCode(0, 0);

    internal OpCode Immediate() =>
        new OpCode(NextByte(), 0);

    internal OpCode Relative()
    {
        var addr = (ushort)NextByte();

        if ((addr & 0x80) == 1)
            addr |= 0xFF00;

        return new OpCode(addr, 0);
    }

    // Implied
    internal OpCode Implicit()
    {
        return new OpCode(0, 0);
    }

    internal OpCode Accumulator()
    {
        A <<= 1;
        return new OpCode(A, 0);
    }

    internal OpCode Zp()
    {
        ushort addr = (ushort)(NextByte() & 0xFF);

        return new OpCode(addr, 4);
    }

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

    internal OpCode AbsX()
    {
        var l = NextByte();
        var h = NextByte();
        ushort addr = (ushort)(l | (h << 8));
        addr += X;

        if ((addr & 0xFF00) != (h << 8))
            return new OpCode(addr, 1);
        else
            return new OpCode(addr, 0);
    }

    internal OpCode AbsY()
    {
        var l = NextByte();
        var h = NextByte();
        ushort addr = (ushort)(l | (h << 8));
        addr += Y;

        if ((addr & 0xFF00) != (h << 8))
            return new OpCode(addr, 1);
        else
            return new OpCode(addr, 0);
    }

    internal OpCode Ind()
    {
        var l = NextByte();
        var h = NextByte();
        var addr = (ushort)(l | (h << 8));

        addr = (ushort)((Bus.ReadByte((ushort)(addr + 1)) << 8) | Bus.ReadByte((ushort)(addr + 0)));

        return new OpCode(addr, 0);
    }

    internal OpCode IndX()
    {
        var baseAddr = (ushort)((NextByte() + X) & 0xFF);
        var l = Bus.ReadByte(baseAddr);
        var h = (ushort)(Bus.ReadByte((ushort)(baseAddr + 1)) & 0xFF);
        return new OpCode((ushort)(l | (h << 8)), 0);
    }

    internal OpCode IndY()
    {
        var baseAddr = (ushort)(NextByte() & 0xFF);
        var l = Bus.ReadByte(baseAddr);
        var h = Bus.ReadByte((ushort)(baseAddr + 1)) & 0xFF;

        var addr = (ushort)((l | (h << 8)) + Y);

        if ((addr & 0xFF00) != (h << 8))
            return new OpCode(addr, 1);
        else
            return new OpCode(addr, 0);
    }
}
