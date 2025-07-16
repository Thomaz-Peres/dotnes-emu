using System.Runtime.CompilerServices;

public abstract class Bus
{
    // public delegate uint Read(uint addr);
    // public delegate void Write(uint addr, byte val);

    public byte[] Ram;

    public Bus(byte[] memory)
    {
        Ram = memory;
    }

    protected void WriteByte(uint addr, byte val)
    {
        if (addr >= 0x0000 && addr <= 0xFFFF)
            Ram[addr] = val;
    }

    protected uint ReadByte(uint addr)
    {
        if (addr >= 0x0000 && addr <= 0xFFFF)
            return Ram[addr];

        return 0x00;
    }
}
