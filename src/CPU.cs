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
                0xA9 => Lda(Immediate, 2),
                0xA5 => Lda(Zp, 3),
                0xB5 => Lda(ZpX, 4),
                0xAD => Lda(Abs, 4),
                0xBD => Lda(AbsX, 4),
                0xB9 => Lda(AbsY, 4),
                0xA1 => Lda(IndX, 6),
                0xB1 => Lda(IndY, 5),
                0xA2 => Ldx(Immediate, 2),
                0xA6 => Ldx(Zp, 3),
                0xB6 => Ldx(ZpY, 4),
                0xAE => Ldx(Abs, 4),
                0xBE => Ldx(AbsY, 4),
                0xA0 => Ldy(Immediate, 2),
                0xA4 => Ldy(Zp, 3),
                0xB4 => Ldy(ZpX, 4),
                0xAC => Ldy(Abs, 4),
                0xBC => Ldy(AbsX, 4),
                0x18 => ClearSetFlag(2, StatusFlags.Carry, false), // Implied,
                0xD8 => ClearSetFlag(2, StatusFlags.Decimal, false), // Implied,
                0x58 => ClearSetFlag(2, StatusFlags.InterruptDisable, false), // Implied,
                0xB8 => ClearSetFlag(2, StatusFlags.V, false), // Implied,
                0x38 => ClearSetFlag(2, StatusFlags.Carry, true), // Implied,
                0xF8 => ClearSetFlag(2, StatusFlags.Decimal, true), // Implied,
                0x78 => ClearSetFlag(2, StatusFlags.InterruptDisable, true), // Implied,


                0xAA => Tax(2),
                0xA8 => Tay(2),
                0xBA => Tsx(2),
                0x8A => Txa(2),
                0x9A => Txs(2),
                0x98 => Tya(2),
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
