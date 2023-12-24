using System.Runtime.InteropServices;
using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day12 : Solution
{
    private readonly List<SpringRow> _springsRows = [];

    protected override string LogicPart1()
    {
        var springsRows = new List<SpringRow>();
        foreach (var line in inputLines)
        {
            var springs = line.Split(' ')[0].ToCharArray().Select(x => new Spring(x)).ToList();
            var damagedGroups = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();
            springsRows.Add(new SpringRow(springs, damagedGroups));
        }

        long numOfCombs = 0;
        foreach (var row in springsRows)
        {
            numOfCombs += row.NumberOfCombinations();
        }

        return numOfCombs.ToString();
    }

    protected override string LogicPart2()
    {
        var numOfRepeats = 5;
        var springsRows = new List<SpringRow>();
        foreach (var line in inputLines)
        {
            string springsInput = line.Split(' ')[0];
            var multiplicatedSpringsString = string.Join('?', Enumerable.Repeat(springsInput, numOfRepeats));
            var springs = multiplicatedSpringsString.ToCharArray().Select(x => new Spring(x)).ToList();

            string groupsInput = line.Split(' ')[1];
            var multiplicatedGroupsString = string.Join(',', Enumerable.Repeat(groupsInput, numOfRepeats));
            var damagedGroups = multiplicatedGroupsString.Split(',').Select(int.Parse).ToList();
            springsRows.Add(new SpringRow(springs, damagedGroups));
        }

        long numOfCombs = 0;
        foreach (var row in springsRows)
        {
            checked
            {
                numOfCombs += row.NumberOfCombinations();
            }        
        }

        return numOfCombs.ToString();
    }
}

// Credits to cvttsd2si@programming.dev for the solution
public class SpringRow(List<Spring> springs, List<int> damagedGroups)
{
    private readonly List<Spring> _springs = springs;
    private readonly List<int> _damagedGroups = damagedGroups;
    private readonly Dictionary<(int, int), long> _cache = [];

    public long NumberOfCombinations()
    {
        return Go(_springs.Count + 1);
    }

    private long Go(int ai)
    {
        if (ai == 0)
        {
            return Test(ai, 0);
        }
        else
        {
            for (int bi = 0; bi <= _damagedGroups.Count; bi++)
            {
                _cache.Add((ai, bi), Test(ai, bi));
            }

            return Go(ai - 1);
        }
    }

    private long Test(int ai, int bi)
    {
        if (ai >= _springs.Count)
        {
            if (bi >= _damagedGroups.Count)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            long place = 0;


            if (
                bi < _damagedGroups.Count &&
                ai + _damagedGroups[bi] <= _springs.Count &&
                _springs[ai..(ai + _damagedGroups[bi])].All(x => x.State != SpringState.Operational) &&
                (ai + _damagedGroups[bi] == _springs.Count || _springs[ai + _damagedGroups[bi]].State != SpringState.Damaged)
            )
            {
                place = _cache.GetValueOrDefault((ai + _damagedGroups[bi] + 1, bi + 1), 0);
            }

            long skip = 0;

            if (_springs[ai].State != SpringState.Damaged)
            {
                skip = _cache.GetValueOrDefault((ai + 1, bi), 0);
            }
            
            return place + skip;
        }
    }
}

public class Spring
{
    public SpringState State { get; init; }

    public Spring(char state)
    {
        State = state switch
        {
            '.' => SpringState.Operational,
            '#' => SpringState.Damaged,
            '?' => SpringState.Unknown,
            _ => throw new Exception($"Unknown spring state: {state}"),
        };
    }
}

public enum SpringState
{
    Operational,
    Damaged,
    Unknown,
}