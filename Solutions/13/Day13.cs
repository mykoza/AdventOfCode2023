using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day13 : Solution
{
    private readonly List<List<string>> _patterns = [];
    private List<string> _linesOfReflection = [];
    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        var pattern = new List<string>();
        for (int i = 0; i < inputLines.Length; i++)
        {
            string? line = inputLines[i];
            if (string.IsNullOrEmpty(line))
            {
                _patterns.Add(pattern);
                pattern = [];
                continue;
            }

            pattern.Add(line);
        }

        _patterns.Add(pattern);
    }

    protected override string LogicPart1()
    {
        var res = 0;

        for (int i = 0; i < _patterns.Count; i++)
        {
            List<string>? pattern = _patterns[i];

            var totalRowsAbove = CountRowsAboveForPattern(pattern, 0);

            if (totalRowsAbove > 0)
            {
                res += 100 * totalRowsAbove;
                continue;
            }

            var totalColumnsBefore = CountColumnsBeforeForPattern(pattern, 0);
            res += totalColumnsBefore;
        }

        return res.ToString();
    }

    protected override string LogicPart2()
    {
        var res = 0;

        for (int i = 0; i < _patterns.Count; i++)
        {
            List<string>? pattern = _patterns[i];

            var totalRowsAbove = CountRowsAboveForPattern(pattern, 1);

            if (totalRowsAbove > 0)
            {
                res += 100 * totalRowsAbove;
                continue;
            }

            var totalColumnsBefore = CountColumnsBeforeForPattern(pattern, 1);
            res += totalColumnsBefore;
        }

        return res.ToString();
    }

    private int CountRowsAboveForPattern(List<string> pattern, int acceptableErrors)
    {
        for (int i = 0; i < pattern.Count; i++)
        {
            if (CheckIfLinesReflected(pattern, i, acceptableErrors) > 0)
            {
                return i + 1;
            }
        }

        return 0;
    }

    private int CountColumnsBeforeForPattern(List<string> pattern, int acceptableErrors)
    {
        for (int i = 0; i < pattern[0].Length; i++)
        {
            if (CheckIfColumnsReflected(pattern, i, acceptableErrors) > 0)
            {
                return i + 1;
            }
        }

        return 0;
    }

    private int CheckIfLinesReflected(List<string> pattern, int lineStartIndex, int acceptableErrors = 0)
    {
        var numOfLinesToCompare = Math.Min(lineStartIndex + 1, pattern.Count - lineStartIndex - 1);
        var errors = 0;

        for (int i = 0; i < numOfLinesToCompare; i++)
        {
            for (int j = 0; j < pattern[0].Length; j++)
            {
                if (pattern[lineStartIndex - i][j] != pattern[lineStartIndex + i + 1][j])
                {
                    errors += 1;
                }
            }
        }

        if (errors == acceptableErrors)
        {
            return numOfLinesToCompare;
        }

        return -1;
    }

    private int CheckIfColumnsReflected(List<string> pattern, int columnStartIndex, int acceptableErrors = 0)
    {
        var numOfColumnsToCompare = Math.Min(columnStartIndex + 1, pattern[0].Length - columnStartIndex - 1);
        var errors = 0;

        for (int i = 0; i < numOfColumnsToCompare; i++)
        {
            var column1 = new string(pattern.Select(line => line[columnStartIndex - i]).ToArray());
            var column2 = new string(pattern.Select(line => line[columnStartIndex + i + 1]).ToArray());

            for (int j = 0; j < column1.Length; j++)
            {
                if (column1[j] != column2[j])
                {
                    errors += 1;
                }
            }
        }

        if (errors == acceptableErrors)
        {
            return numOfColumnsToCompare;
        }

        return -1;
    }
}
