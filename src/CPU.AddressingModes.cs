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

    internal OpCode Immediate()
    {
        var val = Bus.ReadByte(PC++);
        return new OpCode(val, 0);
    }

    internal OpCode Relative()
    {
        return new OpCode(0, 0);
    }

    internal OpCode Implicit() =>
        new OpCode(0, 0);

    internal OpCode Accumulator()
    {

        return new OpCode();
    }

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

    internal OpCode AbsX()
    {
        var addr = (ushort)(NextByte() | (NextByte() << 8));
        var fullAddr = (ushort)(addr + X);

        return new OpCode(fullAddr, 0);
    }

    internal OpCode AbsY()
    {
        var addr = (ushort)(NextByte() | (NextByte() << 8));
        var fullAddr = (ushort)(addr + Y);

        return new OpCode(fullAddr, 0);
    }

    internal OpCode Ind()
    {
        var l = NextByte();
        var h = NextByte();
        var addr = (ushort)(l | (h << 8));

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
        var baseAddr = NextByte();
        var l = Bus.ReadByte(baseAddr);
        var h = Bus.ReadByte((ushort)(baseAddr + 1)) & 0xFF;
        return new OpCode((ushort)((l | (h << 8)) + Y), 0);
    }
}
