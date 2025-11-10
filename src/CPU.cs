namespace nes;

internal sealed partial class CPU
{
    private byte A;
    private byte X;
    private byte Y;
    private byte Status; // Status Register (P)
    private byte StackPointer;

    private ushort PC;

    private int Cycles;
    private Bus Bus;

    public CPU(Bus bus)
    {
        Bus = bus;
        A = X = Y = 0;
        PC = (ushort)(Bus.ReadByte(0XFFFC) | (Bus.ReadByte(0XFFFD) << 8));
        StackPointer = 0x24; // 01A0-01FF
    }

    public void EmulateCycle()
    {
        if (Cycles == 0)
        {
            var opCode = NextByte();

            var x = opCode switch
            {
                0xA9 => LoadMemoryValue(Immediate, ref A, 2),
                0xA5 => LoadMemoryValue(Zp, ref A, 3),
                0xB5 => LoadMemoryValue(ZpX, ref A, 4),
                0xAD => LoadMemoryValue(Abs, ref A, 4),
                0xBD => LoadMemoryValue(AbsX, ref A, 4),
                0xB9 => LoadMemoryValue(AbsY, ref A, 4),
                0xA1 => LoadMemoryValue(IndX, ref A, 6),
                0xB1 => LoadMemoryValue(IndY, ref A, 5),
                0xA2 => LoadMemoryValue(Immediate, ref X, 2),
                0xA6 => LoadMemoryValue(Zp, ref X, 3),
                0xB6 => LoadMemoryValue(ZpY, ref X, 4),
                0xAE => LoadMemoryValue(Abs, ref X, 4),
                0xBE => LoadMemoryValue(AbsY, ref X, 4),
                0xA0 => LoadMemoryValue(Immediate, ref Y, 2),
                0xA4 => LoadMemoryValue(Zp, ref Y, 3),
                0xB4 => LoadMemoryValue(ZpX, ref Y, 4),
                0xAC => LoadMemoryValue(Abs, ref Y, 4),
                0xBC => LoadMemoryValue(AbsX, ref Y, 4),

                0x18 => ClearSetFlag(2, StatusFlags.Carry, false), // Implied,
                0xD8 => ClearSetFlag(2, StatusFlags.Decimal, false), // Implied,
                0x58 => ClearSetFlag(2, StatusFlags.InterruptDisable, false), // Implied,
                0xB8 => ClearSetFlag(2, StatusFlags.V, false), // Implied,
                0x38 => ClearSetFlag(2, StatusFlags.Carry, true), // Implied,
                0xF8 => ClearSetFlag(2, StatusFlags.Decimal, true), // Implied,
                0x78 => ClearSetFlag(2, StatusFlags.InterruptDisable, true), // Implied,

                0xAA => TransferRegister(ref X, A, 2),
                0xA8 => TransferRegister(ref Y, A, 2),
                0xBA => TransferRegister(ref X, StackPointer, 2),
                0x8A => TransferRegister(ref A, X, 2),
                0x9A => TransferRegister(ref StackPointer, X, 2, false),
                0x98 => TransferRegister(ref A, Y, 2),
            };
        }
    }

    private void Reset()
    {
        A = X = Y = 0;
        PC = (ushort)(Bus.ReadByte(0XFFFC) | (Bus.ReadByte(0XFFFD) << 8));
        StackPointer = 0xFD;
    }

    private void IRQ()
    {
        if (GetFlag(StatusFlags.InterruptDisable))
        {
            StackPointer = (byte)(PC + Status); // Should be a PUSH
            Status = (byte)StatusFlags.InterruptDisable;

            Status = StatusFlags.Break << 0;

            var high = Bus.ReadByte(0xFFFF);
            var low = Bus.ReadByte(0xFFFE);

            PC = (ushort)(high & low);
        }

    }

    private void NMI() { }


    internal bool GetFlag(ushort flag) =>
        (Status & flag) != 0;

    internal void SetFlag(ushort flag, bool v)
    {
        if (v)
            Status |= (byte)flag;
        else
            Status &= (byte)~flag;
    }
}
