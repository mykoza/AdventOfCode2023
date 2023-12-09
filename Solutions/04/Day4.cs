namespace AdventOfCode2023;

public class Day4 : Solution
{
    protected override int DayNumber { get; init; } = 4;

    protected override string LogicPart1()
    {
        var res = 0;

        foreach (var line in _inputLines)
        {
            var lineSplit1 = line.Split(':');
            var lineSplit2 = lineSplit1[1].Split('|');
            var winningNumbersString = lineSplit2[0];
            var ownNumbersString = lineSplit2[1];
            var winningNumbers = winningNumbersString.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var ownNumbers = ownNumbersString.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var intersect = ownNumbers.Intersect(winningNumbers).Count();
            res += (int)Math.Pow(2, intersect - 1);
        }

        return res.ToString();
    }

    protected override string LoginPart2()
    {
        throw new NotImplementedException();
    }
}
