namespace nes;

internal sealed class Cartridge
{
    private readonly byte[] Rom;

    public Cartridge(string romPaht)
    {
        Rom = File.ReadAllBytes(romPaht);
    }
}
