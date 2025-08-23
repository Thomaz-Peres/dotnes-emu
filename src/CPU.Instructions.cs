using System.Reflection;
using static nes.AddressingModes;

namespace nes;

internal sealed partial class CPU
{
    public delegate void OpCodeAction();

    private sealed record OpCodes(OpCodeAction Action, string MethodName, IEnumerable<OpCodeAttribute> CodeAttributes);


    private void InitializeInstruction()
    {
        _opcodeMap = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.IsDefined(typeof(OpCodeAttribute), false))
            .SelectMany(m => m.GetCustomAttributes<OpCodeAttribute>(false)
                .Select(attr => (
                    attr.Address,
                    new OpCodes(
                        (OpCodeAction)Delegate.CreateDelegate(typeof(OpCodeAction), this, m),
                        m.Name,
                        new List<OpCodeAttribute> { attr }
                    )
                ))).ToDictionary(t => t.Address, t => t.Item2);

        // opCodes.First().Action();
    }

    // private int Adc(Func<OpCode> adrMode, int cycle)
    // {
    //     var addr = adrMode();

    //     var readAddr = Bus.ReadByte(addr.Address);

    //     var res = A + readAddr + StatusFlags.Carry;

    //     if (res > 0xFF)
    //         Status = (byte)StatusFlags.Carry;
    //     else if (res == 0)
    //         Status = (byte)StatusFlags.Zero;
    //     else if ((res ^ A & res ^ readAddr & 0x80) != 0) // I'm bug with this now -> V - Overflow	(result ^ A) & (result ^ memory) & $80	If the result's sign is different from both A's and memory's, signed overflow (or underflow) occurred. Problably a negative, TODO: test later.
    //         Status = (byte)StatusFlags.V;
    //     else if (res != 7)
    //         Status = (byte)StatusFlags.Negative;

    //     A = (byte)(res & 0xFF);

    //     return cycle + addr.Cycles;
    // }
    [OpCode(0x69, 2, AddressingMode.Immediate)]
    [OpCode(0x65, 3, AddressingMode.ZeroPage)]
    [OpCode(0x75, 4, AddressingMode.ZeroPageX)]
    private void Adc()
    {

    }

}
