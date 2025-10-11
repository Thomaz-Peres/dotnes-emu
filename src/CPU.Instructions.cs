using System.Reflection;

namespace nes;

internal sealed partial class CPU
{
    public uint ReadAddress(Func<OpCode> adrMode)
    {
        if (adrMode == Implicit)
            return A;

        return 0;
    }

    private uint Adc(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var readAddr = Bus.ReadByte(addr.Address);

        var res = A + readAddr + StatusFlags.Carry;

        if (res > 0xFF)
            Status = (byte)StatusFlags.Carry;
        else if (res == 0)
            Status = (byte)StatusFlags.Zero;
        else if ((res ^ A & res ^ readAddr & 0x80) != 0) // I'm bug with this now -> V - Overflow	(result ^ A) & (result ^ memory) & $80	If the result's sign is different from both A's and memory's, signed overflow (or underflow) occurred. Problably a negative, TODO: test later.
            Status = (byte)StatusFlags.V;
        else if (res != 7)
            Status = (byte)StatusFlags.Negative;

        A = (byte)(res & 0xFF);

        return cycle + addr.Cycles;
    }

    private uint And(Func<OpCode> adrMode, uint cycle)
    {
        A &= ReadAddress(adrMode);
        return cycle;
    }
}
