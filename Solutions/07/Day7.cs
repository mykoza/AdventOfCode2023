using AdventOfCode2023.Common;
using AdventOfCode2023.Day7Solution;

namespace AdventOfCode2023;

public class Day7 : Solution
{
    protected override int DayNumber { get; init; } = 7;

    protected override string LogicPart1()
    {
        var listOfHands = ListOfHands.Parse([.. inputLines], 1);

        return listOfHands.Value().ToString();
    }

    protected override string LogicPart2()
    {
        var listOfHands = ListOfHands.Parse([.. inputLines], 2);

        return listOfHands.Value().ToString();
    }
}
