using AdventOfCode2023.Common;

namespace AdventOfCode2023;

class Day15 : Solution
{
    private readonly List<string> _initializationSeq = [];

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        foreach (var step in inputLines[0].Split(','))
        {
            _initializationSeq.Add(step);
        }
    }

    protected override string LogicPart1()
    {
        return _initializationSeq.Select(x => ComputeHash(x)).Sum().ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private static int ComputeHash(string step)
    {
        var currentVal = 0;

        foreach (var character in step)
        {
            currentVal += character;
            currentVal *= 17;
            currentVal %= 256;
        }

        return currentVal;
    }
}