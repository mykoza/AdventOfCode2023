using AdventOfCode2023.Common;
using System.Diagnostics;

namespace AdventOfCode2023;

public class Day20 : Solution
{
    protected override bool UseExample => false;

    private readonly Dictionary<string, Module> _modules = [];
    private readonly Queue<(Pulse pulse, Module from, Module to)> _pulseQueue = [];

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        for (int i = 0; i < inputLines.Length; i++)
        {
            string? line = inputLines[i] ?? throw new Exception("");

            var split = line.Split(" -> ", 2);
            var name = split[0];

            string type;
            if (name == "broadcaster")
            {
                type = "broadcast";
            }
            else
            {
                type = name[0].ToString();
                name = name[1..];
            }

            _modules.Add(name, CreateModule(type, name));
        }

        _modules["output"] = new Output("output", _pulseQueue);

        var button = new Button("button", _pulseQueue);
        button.AddDestination(_modules["broadcaster"]);
        _modules["button"] = button;

        for (int i = 0; i < inputLines.Length; i++)
        {
            string? line = inputLines[i] ?? throw new Exception("");

            var split = line.Split(" -> ", 2);
            var name = split[0];
            var destinations = split[1].Split(", ");

            if (name != "broadcaster")
            {
                name = name[1..];
            }

            foreach (var destination in destinations)
            {
                var destinationModule = _modules.GetValueOrDefault(destination, new Output(destination, _pulseQueue));
                _modules.TryAdd(destinationModule.Name, destinationModule);
                _modules[name].AddDestination(destinationModule);
            }
        }
    }


    protected override string LogicPart1()
    {
        Button button = (Button)_modules["button"];

        long lowPulseCount = 0;
        long highPulseCount = 0;
        for (var i = 0; i < 1000; i++)
        {
            button.Push();

            while (_pulseQueue.Count > 0)
            {
                var (pulse, from, to) = _pulseQueue.Dequeue();

                if (pulse == Pulse.Low)
                {
                    lowPulseCount++;
                }
                else
                {
                    highPulseCount++;
                }

                to.ProcessPulse(from, pulse);
            }
        }

        return (lowPulseCount * highPulseCount).ToString();
    }

    protected override string LogicPart2()
    {
        _modules.Clear();
        _pulseQueue.Clear();

        BeforeLogic();

        Button button = (Button)_modules["button"];

        // original solution by HyperNeutrino
        // if pulses from vd inputs are all high, then we have found the solution
        // find cycle length for each rx input
        // find least common multiple
        var rxInput = _modules["rx"].Inputs.Single();
        var seen = rxInput.Inputs.ToDictionary(module => module, module => 0L);
        var cycleLengths = new Dictionary<Module, long>();

        var pressCount = 0L;
        while (true)
        {
            button.Push();
            pressCount += 1;

            while (_pulseQueue.Count > 0)
            {
                var (pulse, from, to) = _pulseQueue.Dequeue();

                if (to == rxInput && pulse == Pulse.High)
                {
                    seen[from] += 1;

                    if (!cycleLengths.TryGetValue(from, out long cycleLength))
                    {
                        cycleLengths[from] = pressCount;
                    }
                    else
                    {
                        // check if pulse is high according to cycle length
                        Debug.Assert(pressCount == seen[from] * cycleLength);
                    }

                    if (seen.Values.All(x => x > 0))
                    {
                        return Maths.LeastCommonMultiple(cycleLengths.Values).ToString();
                    }
                }

                to.ProcessPulse(from, pulse);
            }
        }

        throw new Exception("Not found");
    }

    private Module CreateModule(string type, string name)
    {
        return type switch
        {
            "broadcast" => new Broadcast(name, _pulseQueue),
            "%" => new FlipFlop(name, _pulseQueue),
            "&" => new Conjunction(name, _pulseQueue),
            _ => throw new NotImplementedException(),
        };
    }

    enum Pulse
    {
        Low,
        High,
    }

    abstract class Module(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue)
    {
        protected readonly Queue<(Pulse pulse, Module from, Module to)> pulseQueue = pulseQueue;
        protected readonly Dictionary<Module, Pulse> _lastPulses = [];

        public bool State { get; protected set; } = false;
        public List<Module> Destinations { get; set; } = [];
        public string Name { get; set; } = name;
        public List<Module> Inputs => [.. _lastPulses.Keys];

        public abstract void ProcessPulse(Module from, Pulse pulse);

        public virtual void AddDestination(Module module)
        {
            Destinations.Add(module);
            module._lastPulses.Add(this, Pulse.Low);
        }
    }

    class Button(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue) : Module(name, pulseQueue)
    {
        public void Push()
        {
            ProcessPulse(this, Pulse.Low);
        }

        public override void ProcessPulse(Module from, Pulse pulse)
        {
            foreach (var module in Destinations)
            {
                pulseQueue.Enqueue((Pulse.Low, this, module));
            }
        }
    }

    class Broadcast(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue) : Module(name, pulseQueue)
    {
        public override void ProcessPulse(Module from, Pulse pulse)
        {
            foreach (var module in Destinations)
            {
                pulseQueue.Enqueue((pulse, this, module));
            }
        }
    }

    class FlipFlop(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue) : Module(name, pulseQueue)
    {
        public override void ProcessPulse(Module from, Pulse pulse)
        {
            if (pulse == Pulse.High)
            {
                return;
            }

            State = !State;
            var newPulse = State ? Pulse.High : Pulse.Low;

            foreach (var module in Destinations)
            {
                pulseQueue.Enqueue((newPulse, this, module));
            }
        }
    }

    class Conjunction(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue) : Module(name, pulseQueue)
    {
        public override void ProcessPulse(Module from, Pulse pulse)
        {
            _lastPulses[from] = pulse;

            // if all were high send low
            var newPulse = _lastPulses.Any(pulse => pulse.Value == Pulse.Low)
                ? Pulse.High
                : Pulse.Low;

            foreach (var module in Destinations)
            {
                pulseQueue.Enqueue((newPulse, this, module));
            }
        }
    }

    class Output(string name, Queue<(Pulse pulse, Module from, Module to)> pulseQueue) : Module(name, pulseQueue)
    {
        public override void ProcessPulse(Module from, Pulse pulse)
        {
            //
        }
    }
}
