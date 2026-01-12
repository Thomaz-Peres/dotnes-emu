namespace nes;

internal sealed class PPU
{
    private readonly Bus Bus;

    private const ushort ScreenWidht = 256;
    private const ushort ScreenHeight = 240;

    private ushort R, W, x2;

    private const ushort PPUCTRL = 0x2000; // $2000
    private const ushort PPUMASK = 0x2001; // $2001
    private const ushort PPUSTATUS = 0x2002; // $2002
    private const ushort OAMADDR = 0x2003; // $2003
    private const ushort OAMDATA = 0x2004; // $2004
    private const ushort PPUSCROLL = 0x2005; // $2005
    private const ushort PPUADDR = 0x2006; // $2006
    private const ushort PPUDATA = 0x2007; // $2007
    private const ushort OAMDMA = 0x4014; // $4014

    public PPU(Bus bus)
    {
        Bus = bus;
    }

    public void BusWriteByte(ushort addr, byte val)
    {
        var x = addr switch
        {
            0x0000 => 1,
            0x0001 => 2,
            0x0003 => 2,
            0x0004 => 2,
            0x0005 => 2,
            0x0006 => 2,
            0x0007 => 2,
            _ => 0
        };
    }

    public byte BusReadByte(ushort addr)
    {
        return addr switch
        {
            0x0000 => 1,
            0x0001 => 2,
            0x0003 => 2,
            0x0004 => 2,
            0x0005 => 2,
            0x0006 => 2,
            0x0007 => 2,
            _ => 0
        };
    }

    public void PPUWriteByte(ushort addr, byte val)
    {
        addr &= 0x3FFF;

    }

    public byte PPUReadByte(ushort addr)
    {
        addr &= 0x3FFF;
        throw new NotImplementedException();
    }
}
