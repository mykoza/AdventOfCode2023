using AdventOfCode2023.Common;
using AdventOfCode2023.Day12Solution;

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
