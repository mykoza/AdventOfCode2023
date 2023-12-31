using System.Collections.Specialized;
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
        var boxes = new List<Box>(256);

        for (int i = 0; i < 256; i++)
        {
            boxes.Add(new Box());
        }

        foreach (var step in _initializationSeq)
        {
            if (step.Contains('='))
            {
                var split = step.Split('=');
                var labelHash = ComputeHash(split[0]);
                boxes[labelHash].AddLens(new Lens(split[0], int.Parse(split[1])));
            }
            else if (step.Contains('-'))
            {
                var split = step.Split('-');
                var labelHash = ComputeHash(split[0]);
                boxes[labelHash].RemoveLens(split[0]);
            }
        }

        var res = 0;
        for (int i = 0; i < boxes.Count; i++)
        {
            res += (i + 1) * boxes[i].FocusingPower();
        }

        return res.ToString();
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

class Box
{
    private readonly OrderedDictionary _lenses = [];

    public void AddLens(Lens lens)
    {
        if (_lenses.Contains(lens.Label))
        {
            _lenses[lens.Label] = lens.FocalLength;
            return;
        }

        _lenses.Add(lens.Label, lens.FocalLength);
    }

    public void RemoveLens(string label)
    {
        _lenses.Remove(label);
    }

    public int FocusingPower()
    {
        var res = 0;

        for (int i = 0; i < _lenses.Count; i++)
        {
            res += (i + 1) * (int)_lenses[i];
        }

        return res;
    }
}

record Lens(string Label, int FocalLength);