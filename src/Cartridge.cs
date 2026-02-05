namespace nes;

internal sealed class Cartridge
{
    private readonly byte[] Rom;

    public byte[] PrgROM;
    public byte[] ChrROM;

    public int PrgBanks;
    public int ChrBanks;
    private int MapperId;

    public Cartridge(string romPath)
    {
        Rom = File.ReadAllBytes(romPath);

        if (Rom[0] != 0x4E || Rom[1] != 0x45 || Rom[2] != 0x53 || Rom[3] != 0x1A)
        {
            Console.WriteLine("Invalid iNES file");
            Environment.Exit(1);
        }

        MapperId = Rom[6] >> 4 | ((Rom[7] >> 4) << 4);

        int prgSize = PrgBanks * 16 * 1024;
    }
}
