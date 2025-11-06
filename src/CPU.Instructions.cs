using System.Reflection;

namespace nes;

internal sealed partial class CPU
{
    //TODO: Test the cycles, looks like I need just 1. In adrMode or on this method.
    // problably all this cycles receiving here and in adrMode don't need.
    private uint Adc(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var readAddr = Bus.ReadByte(addr.Address);

        var res = A + readAddr + StatusFlags.Carry;

        SetFlag(StatusFlags.Carry, res > 0xFF);
        SetFlag(StatusFlags.Zero, res == 0);
        SetFlag(StatusFlags.V, (res ^ A & res ^ readAddr & 0x80) != 0); // I'm bug with this now -> V - Overflow	(result ^ A) & (result ^ memory) & $80	If the result's sign is different from both A's and memory's, signed overflow (or underflow) occurred. Problably a negative, TODO: test later.
        SetFlag(StatusFlags.Negative, res != 7);

        A = (byte)(res & 0xFF);

        return cycle + addr.ExtraCycles;
    }

    private uint And(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();
        A &= (byte)addr.Address;

        SetFlag(StatusFlags.Zero, A == 0x00);
        SetFlag(StatusFlags.Negative, (A & 0x80) == 7);
        return cycle + addr.ExtraCycles;
    }

    private uint Asl(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();
        var value = (byte)(addr.Address << 1);
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Carry, (addr.Address & 0x80) != 0);
        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) == 0);

        return cycle + addr.ExtraCycles;
    }

    private uint Bcc(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        if (!GetFlag(StatusFlags.Carry))
        {
            PC += addr.Address;
            cycle++;

            if (addr.ExtraCycles > 0)
                cycle++;
        }

        return cycle;
    }

    private uint Bcs(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        if (GetFlag(StatusFlags.Carry))
        {
            PC += addr.Address;
            cycle++;

            if (addr.ExtraCycles > 0)
                cycle++;
        }

        return cycle;
    }

    private uint Beq(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        if (GetFlag(StatusFlags.Zero))
        {
            PC += addr.Address;

        }

        return cycle;
    }

    private uint Lda(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        A = Bus.ReadByte(addr.Address);

        SetFlag(StatusFlags.Zero, A == 0);
        SetFlag(StatusFlags.Negative, (A & 0x80) != 0);

        if (addr.ExtraCycles > 0)
            cycle++;

        return cycle;
    }

    private uint Ldx(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        X = Bus.ReadByte(addr.Address);

        SetFlag(StatusFlags.Zero, X == 0);
        SetFlag(StatusFlags.Negative, (X & 0x80) != 0);

        if (addr.ExtraCycles > 0)
            cycle++;

        return cycle;
    }

    private uint Ldy(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        Y = Bus.ReadByte(addr.Address);

        SetFlag(StatusFlags.Zero, Y == 0);
        SetFlag(StatusFlags.Negative, (Y & 0x80) != 0);

        if (addr.ExtraCycles > 0)
            cycle++;

        return cycle;
    }

    private uint Sta(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        Bus.WriteByte(addr.Address, A);

        return cycle;
    }

    private uint Stx(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        Bus.WriteByte(addr.Address, X);

        return cycle;
    }

    private uint Sty(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        Bus.WriteByte(addr.Address, Y);

        return cycle;
    }

    private uint Jmp(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        PC = Bus.ReadByte(addr.Address);

        return cycle;
    }

    private uint Jsr(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        PC = Bus.ReadByte(addr.Address);

        return cycle;
    }

    private uint ClearFlag(Func<OpCode> adrMode, uint cycle, ushort flag, bool state)
    {
        SetFlag(flag, state);
        return cycle;
    }

    private uint Rts(Func<OpCode> adrMode, uint cycle)
    {

        return cycle;
    }
}
