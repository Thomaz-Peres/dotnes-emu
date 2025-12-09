namespace nes;

internal sealed partial class CPU
{
    internal byte A;
    internal byte X;
    internal byte Y;
    internal byte Status; // Status Register (P)
    internal byte StackPointer;

    internal ushort PC;

    internal int Cycles;
    private Bus Bus;

    public CPU(Bus bus)
    {
        Bus = bus;
        A = X = Y = 0;
        PC = (ushort)(Bus.ReadByte(0xFFFC) | (Bus.ReadByte(0xFFFD) << 8));
        Status = 0x24;
        StackPointer = 0xFD; // 01A0-01FF
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

                0x85 => SetMemoryValue(Zp, 3, A),
                0x95 => SetMemoryValue(ZpX, 4, A),
                0x8D => SetMemoryValue(Abs, 4, A),
                0x9D => SetMemoryValue(AbsX, 5, A),
                0x99 => SetMemoryValue(AbsY, 5, A),
                0x81 => SetMemoryValue(IndX, 6, A),
                0x91 => SetMemoryValue(IndY, 6, A),
                0x86 => SetMemoryValue(Zp, 3, X),
                0x96 => SetMemoryValue(ZpY, 4, X),
                0x8E => SetMemoryValue(Abs, 4, X),
                0x84 => SetMemoryValue(Zp, 3, Y),
                0x94 => SetMemoryValue(ZpX, 4, Y),
                0x8C => SetMemoryValue(Abs, 4, Y),

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

                0x69 => Adc(Immediate, 2),
                0x65 => Adc(Zp, 3),
                0x75 => Adc(ZpX, 4),
                0x6D => Adc(Abs, 4),
                0x7D => Adc(AbsX, 4),
                0x79 => Adc(AbsY, 4),
                0x61 => Adc(IndX, 6),
                0x71 => Adc(IndY, 5),

                0xE9 => Sbc(Immediate, 2),
                0xE5 => Sbc(Zp, 3),
                0xF5 => Sbc(ZpX, 4),
                0xED => Sbc(Abs, 4),
                0xFD => Sbc(AbsX, 4),
                0xF9 => Sbc(AbsY, 4),
                0xE1 => Sbc(IndX, 6),
                0xF1 => Sbc(IndY, 5),

                0xE6 => Inc(Zp, 5),
                0xF6 => Inc(ZpX, 6),
                0xEE => Inc(Abs, 6),
                0xFE => Inc(AbsX, 7),
                0xE8 => IncXY(2, ref X),
                0xC8 => IncXY(2, ref Y),

                0xC6 => Dec(Zp, 5),
                0xD6 => Dec(ZpX, 6),
                0xCE => Dec(Abs, 6),
                0xDE => Dec(AbsX, 7),
                0xCA => DecXY(2, ref X),
                0x88 => DecXY(2, ref Y),

                0x0A => Asl(Accumulator, 2),
                0x06 => Asl(Zp, 5),
                0x16 => Asl(ZpX, 6),
                0x0E => Asl(Abs, 6),
                0x1E => Asl(AbsX, 7),

                0x4A => Lsr(Accumulator, 2),
                0x46 => Lsr(Zp, 5),
                0x56 => Lsr(ZpX, 6),
                0x4E => Lsr(Abs, 6),
                0x5E => Lsr(AbsX, 7),

                0x2A => Rol(Accumulator, 2),
                0x26 => Rol(Zp, 5),
                0x36 => Rol(ZpX, 6),
                0x2E => Rol(Abs, 6),
                0x3E => Rol(AbsX, 7),

                0x6A => Ror(Accumulator, 2),
                0x66 => Ror(Zp, 5),
                0x76 => Ror(ZpX, 6),
                0x6E => Ror(Abs, 6),
                0x7E => Ror(AbsX, 7),

                0x4C => Jmp(Abs, 3),
                0x6C => Jmp(Ind, 5),


                _ => throw new NotImplementedException($"OpCode {opCode:X2} not implemented.")
            };
        }
    }

    private void Reset()
    {
        A = X = Y = 0;
        Status = 0x24;
        PC = (ushort)(Bus.ReadByte(0XFFFC) | (Bus.ReadByte(0XFFFD) << 8));
        StackPointer -= 3;
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
