namespace nes;

internal sealed class PPU
{
    private readonly Bus bus;

    public PPU(Bus bus)
    {
        this.bus = bus;
    }

    private byte[] vram;

    private const ushort ScreenWidht = 256;
    private const ushort ScreenHeight = 240;

    private ushort R, W, x2;

    private ushort PPUCTRL = 0x2000; // $2000
    private ushort PPUMASK = 0x2001; // $2001
    private ushort PPUSTATUS = 0x2002; // $2002
    private ushort OAMADDR = 0x2003; // $2003
    private ushort OAMDATA = 0x2004; // $2004
    private ushort PPUSCROLL = 0x2005; // $2005
    private ushort PPUADDR = 0x2006; // $2006
    private ushort PPUDATA = 0x2007; // $2007
    private ushort OAMDMA = 0x4014; // $4014
}
