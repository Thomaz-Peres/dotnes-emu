namespace nes;

internal sealed partial class CPU
{
    private byte A;
    private byte X;
    private byte Y;
    private byte Status; // Status Register (P)
    private ushort StackPointer;

    private ushort PC;
    private int Cycles;
    private Bus Bus;

    public CPU(Bus bus)
    {
        Bus = bus;
        A = X = Y = 0;
        PC = 0XFFC;
        StackPointer = 0x24;
    }

    private byte Fetch() =>
        Bus.ReadByte(PC);

    public void EmulateCycle()
    {
        if (Cycles == 0)
        {
            var opCode = Fetch();
            PC++;

            switch (opCode)
            {
                case 0x96: Adc(AddressingModes.Accumulator, 5);
                    break;
            }
        }

    }

    private void Reset()
    {
        A = X = Y = 0;
        PC = 0XFFC;
        StackPointer = 0xFD;
    }

    private void IRQ()
    {
        if (GetFlag(StatusFlags.InterruptDisable) == 0)
        {
            StackPointer = (ushort)(PC + Status); // Should be a PUSH
            Status = (byte)StatusFlags.InterruptDisable;

            Status = StatusFlags.Break << 0;

            var high = Bus.ReadByte(0xFFFF);
            var low = Bus.ReadByte(0xFFFE);

            PC = (ushort)(high & low);
        }

    }

    private void NMI() { }


    internal byte GetFlag(ushort flag) { return 0; }
    internal void SetFlag(ushort flag, bool v)
    {
        if (v)
            Status |= (byte)flag;
        else
            Status &= (byte)~flag;
    }
}
