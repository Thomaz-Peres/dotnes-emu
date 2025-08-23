using System.Runtime.CompilerServices;

namespace nes;

internal sealed class Bus
{
    public CPU Cpu;
    public PPU Ppu;
    public byte[] Ram;


    public Bus()
    {
        Ram = new byte[2048];
        Ppu = new PPU(this);
        Cpu = new CPU(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteByte(uint addr, byte val)
    {
        if (addr >= 0x0000 && addr <= 0xFFFF)
            Ram[addr] = val;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte(ushort address)
    {
        if (address >= 0x0000 && address <= 0xFFFF)
        {
            return Ram[address];
        }
        // else if ()

        return 0x00;
    }
}
