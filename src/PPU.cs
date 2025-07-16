internal sealed class PPU
{
    private byte[] vram; // 2KB vram

    private const ushort ScreenWidht = 256;
    private const ushort ScreenHeight = 240;

    private ushort R, W, x2;

    private byte PPUCTRL = 0xFFFF; // $2000
    private byte PPUMASK = ; // $2001
    private byte PPUSTATUS = ; // $2002
    private byte OAMADDR = ; // $2003
    private byte OAMDATA = ; // $2004
    private byte PPUSCROLL = ; // $2005
    private byte PPUADDR = ; // $2006
    private byte PPUDATA = ; // $2007
    private byte OAMDMA = ; // $4014
}
