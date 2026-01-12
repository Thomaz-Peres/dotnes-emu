using System.Runtime.CompilerServices;

namespace nes;

internal sealed class Bus
{
    public CPU Cpu;
    public PPU Ppu;
    public byte[] Ram;
    private readonly Cartridge cartridge;


    public Bus(Cartridge cartridge)
    {
        this.cartridge = cartridge;
        Ram = new byte[2048];
        Ppu = new PPU(this);
        Cpu = new CPU(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteByte(ushort addr, byte val)
    {
        if (addr <= 0x1FFF)
        {
            Ram[addr & 0x07FF] = val;
            return;
        }
        else if (addr >= 0x2000 && addr <= 0x3FFF)
        {
            Ppu.PPUWriteByte((ushort)(0x2000 + (addr & 0x0007)), val);
            return;
        }


        // Ram[addr & 0x07FF] = val;

        // if (address >= 0x4000 && address <= 0x401F)
        // {
        //     Cartridge.Write(address);
        //     return;
        // }

        // if (address >= 0x4020)
        // {
        //     Cartridge.Write(address);
        //     return;
        // }
    }

    /*
    * Address range 	Size 	Device
    * $0000-$07FF 	    $0800 	2KB internal RAM
    * $0800-$0FFF 	    $0800 	|
    * $1000-$17FF 	    $0800   | Mirrors of $0000-$07FF
    * $1800-$1FFF 	    $0800   |
    * $2000-$2007 	    $0008 	NES PPU registers
    * $2008-$3FFF 	    $1FF8 	Mirrors of $2000-2007 (repeats every 8 bytes)
    * $4000-$4017 	    $0018 	NES APU and I/O registers
    * $4018-$401F 	    $0008 	APU and I/O functionality that is normally disabled. See CPU Test Mode.
    * $4020-$FFFF 	    $BFE0 	Cartridge space: PRG ROM, PRG RAM, and mapper registers (See Note)
    *
    * https://www.nesdev.org/wiki/CPU_memory_map
    */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte(ushort address)
    {
        if (address <= 0x1FFF)
        {
            return Ram[address & 0x07FF];
        }

        if (address >= 0x2000 && address <= 0x3FFF)
        {
            return Ppu.PPUReadByte((ushort)(address & 0x0007));
        }

        // if (address >= 0x4000 && address <= 0x401F)
        // {
        //     return Cartridge.Read(address);
        // }

        // if (address >= 0x4020)
        // {
        //     return Cartridge.Read(address);
        // }

        return Ram[address & 0x07FF];

        // return 0x00;
    }
}
