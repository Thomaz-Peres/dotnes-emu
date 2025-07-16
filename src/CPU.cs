internal sealed class CPU : Bus
{
    private byte StackPointer = 0x00;

    private byte A;
    private byte X;
    private byte Y;
    private byte Status;

    private ushort PC;


    public CPU(byte[] memory) : base(memory)
    {

    }


    void ADC()
    {
        A += (byte)(ReadByte(0) + StatusFlags.Carry);
    }

}
