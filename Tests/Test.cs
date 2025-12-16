using System.Diagnostics;
using nes;
using Newtonsoft.Json;

namespace Tests;

public sealed class UnitTest1
{

    public class ProcessorState
    {
        public int pc { get; set; }
        public int s { get; set; }
        public int a { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int p { get; set; }

        public List<List<int>> ram { get; set; } = new List<List<int>>();
    }
    public class Test
    {
        public string name { get; set; } = "";
        public ProcessorState initial { get; set; } = new ProcessorState();
        public ProcessorState final { get; set; } = new ProcessorState();
        public List<List<object>> cycles { get; set; } = new List<List<object>>();
    }

    private static readonly string filePath = @"v1/ad.json";


    Bus? _bus;
    CPU? _cpu;

    void Initialize()
    {
        _bus = new Bus();
        _cpu = new CPU(_bus);
    }

    [Fact()]
    public void Test00()
    {
        Initialize();
        ArgumentNullException.ThrowIfNull(_cpu);
        ArgumentNullException.ThrowIfNull(_bus);

        string json = string.Empty;
        if (File.Exists(filePath))
            json = File.ReadAllText(filePath);

        var tests = JsonConvert.DeserializeObject<List<Test>>(json) ?? new List<Test>();

        foreach (var test in tests)
        {
            _cpu.PC = (ushort)test.initial.pc;
            _cpu.StackPointer = (byte)test.initial.s;
            _cpu.A = (byte)test.initial.a;
            _cpu.X = (byte)test.initial.x;
            _cpu.Y = (byte)test.initial.y;
            _cpu.Status = (byte)test.initial.p;

            foreach (var item in test.initial.ram)
            {
                _bus.WriteByte((ushort)item[0], (byte)item[1]);
            }

            if (test.name == "65 cd bc")
                Debug.WriteLine("Debug");

            uint x = _cpu.EmulateCycle();

            if (uint.MaxValue == x)
                Debug.WriteLine("Debug");

            Assert.Equal(test.final.a, _cpu.A);
            Assert.Equal(test.final.x, _cpu.X);
            Assert.Equal(test.final.y, _cpu.Y);
            Assert.Equal(test.final.p, _cpu.Status);
            Assert.Equal(test.final.pc, _cpu.PC);
            Assert.Equal(test.final.s, _cpu.StackPointer);
        }
    }
}
