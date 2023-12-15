namespace AdventOfCode2023;

public class Day4 : Solution
{
    protected override int DayNumber { get; init; } = 4;

    protected override string LogicPart1()
    {
        var res = 0;

        foreach (var line in inputLines)
        {
            int intersect = FindNumberOfMatchingCards(line);
            res += (int)Math.Pow(2, intersect - 1);
        }

        return res.ToString();
    }

    protected override string LogicPart2()
    {
        var numOfMatchesByLine = inputLines
            .Select(FindNumberOfMatchingCards)
            .ToArray();

        var counts = new int[inputLines.Length];
        Array.Fill(counts, 1);

        for (int i = 0; i < inputLines.Length; i++)
        {
            for (int j = i + 1; j < i + numOfMatchesByLine[i] + 1; j++)
            {
                counts[j] += counts[i];
            }
        }

        return counts.Sum().ToString();
    }

    private static int FindNumberOfMatchingCards(string line)
    {
        var lineSplit = line.Split(':')[1].Split('|');
        var winningNumbers = lineSplit[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var ownNumbers = lineSplit[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return ownNumbers.Intersect(winningNumbers).Count();
    }
}
