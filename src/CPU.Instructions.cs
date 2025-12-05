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

        var flag = GetFlag(StatusFlags.Carry) ? 1 : 0;
        var res = A + readAddr + flag;

        SetFlag(StatusFlags.Carry, res > 0xFF);
        SetFlag(StatusFlags.Zero, res == 0);
        SetFlag(StatusFlags.V, ((res ^ A) & (res ^ readAddr) & 0x80) != 0); // I'm bug with this now -> V - Overflow	(result ^ A) & (result ^ memory) & $80	If the result's sign is different from both A's and memory's, signed overflow (or underflow) occurred. Problably a negative, TODO: test later.
        SetFlag(StatusFlags.Negative, (res & 0x80) != 0);

        A = (byte)(res & 0xFF);

        return cycle + addr.ExtraCycles;
    }

    private uint And(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();
        var value = Bus.ReadByte(addr.Address);
        A &= value;

        SetFlag(StatusFlags.Zero, A == 0x00);
        SetFlag(StatusFlags.Negative, (A & 0x80) != 0);
        return cycle + addr.ExtraCycles;
    }

    private uint Asl(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();
        var value = (byte)addr.Address;
        SetFlag(StatusFlags.Carry, (value & 0x80) != 0);

        value <<= 1;
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) != 0);

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
        if (GetFlag(StatusFlags.Carry))
        {
            var addr = adrMode();

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

    // LDA, LDX, LDY
    private uint LoadMemoryValue(Func<OpCode> adrMode, ref byte dest, uint cycle)
    {
        var addr = adrMode();

        dest = Bus.ReadByte(addr.Address);

        SetFlag(StatusFlags.Zero, dest == 0);
        SetFlag(StatusFlags.Negative, (dest & 0x80) != 0);

        if (addr.ExtraCycles > 0)
            cycle++;

        return cycle;
    }

    // STA, STX, STY
    private uint SetMemoryValue(Func<OpCode> adrMode, uint cycle, byte src)
    {
        var addr = adrMode();

        Bus.WriteByte(addr.Address, src);

        return cycle;
    }

    private uint Jmp(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        PC = addr.Address;

        return cycle;
    }

    private uint Jsr(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        PC = addr.Address;

        return cycle;
    }

    private uint ClearSetFlag(uint cycle, ushort flag, bool state)
    {
        SetFlag(flag, state);
        return cycle;
    }

    private uint TransferRegister(ref byte dest, byte src, uint cycle, bool updateFlags = true)
    {
        dest = src;

        if (updateFlags)
        {
            SetFlag(StatusFlags.Zero, dest == 0);
            SetFlag(StatusFlags.Negative, (dest & 0x80) != 0);
        }

        return cycle;
    }

    private uint Sbc(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var readAddr = Bus.ReadByte(addr.Address);

        var flag = GetFlag(StatusFlags.Carry) ? 1 : 0;

        var res = A + (~readAddr) + flag;

        SetFlag(StatusFlags.Carry, res > 0xFF);
        SetFlag(StatusFlags.Zero, res == 0);
        SetFlag(StatusFlags.V, ((res ^ A) & (res ^ ~readAddr) & 0x80) != 0); // I'm bug with this now -> V - Overflow	(result ^ A) & (result ^ memory) & $80	If the result's sign is different from both A's and memory's, signed overflow (or underflow) occurred. Problably a negative, TODO: test later.
        SetFlag(StatusFlags.Negative, (res & 0x80) != 0);

        A = (byte)(res & 0xFF);

        return cycle + addr.ExtraCycles;
    }

    private uint Inc(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var value = (byte)(Bus.ReadByte(addr.Address) + 1);
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) != 0);

        return cycle + addr.ExtraCycles;
    }

    private uint IncXY(uint cycle, ref byte src)
    {
        src++;
        SetFlag(StatusFlags.Zero, src == 0x00);
        SetFlag(StatusFlags.Negative, (src & 0x80) != 0);
        return cycle;
    }

    private uint Dec(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var value = (byte)(Bus.ReadByte(addr.Address) - 1);
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) != 0);

        return cycle + addr.ExtraCycles;
    }

    private uint DecXY(uint cycle, ref byte src)
    {
        src--;
        SetFlag(StatusFlags.Zero, src == 0x00);
        SetFlag(StatusFlags.Negative, (src & 0x80) != 0);
        return cycle;
    }

    private uint Lsr(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var value = (byte)addr.Address;

        SetFlag(StatusFlags.Carry, (value & 0x01) != 0);

        value >>= 1;
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, false);

        return cycle + addr.ExtraCycles;
    }

    private uint Rol(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var value = Bus.ReadByte(addr.Address);
        bool oldCarry = GetFlag(StatusFlags.Carry);

        SetFlag(StatusFlags.Carry, (value & 0x80) != 0);

        value = (byte)((value << 1) | (oldCarry ? 1 : 0));
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Carry, (addr.Address & 0x80) != 0);
        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) != 0x80);

        return cycle + addr.ExtraCycles;
    }

    private uint Ror(Func<OpCode> adrMode, uint cycle)
    {
        var addr = adrMode();

        var value = (byte)(addr.Address >> 1 | (GetFlag(StatusFlags.Carry) ? 1 : 0));
        Bus.WriteByte(addr.Address, value);

        SetFlag(StatusFlags.Carry, (addr.Address & 0x80) != 0);
        SetFlag(StatusFlags.Zero, value == 0x00);
        SetFlag(StatusFlags.Negative, (value & 0x80) != 0x80);

        return cycle + addr.ExtraCycles;
    }
}
