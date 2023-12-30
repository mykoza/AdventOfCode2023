using System.Collections.Immutable;
using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day13 : Solution
{
    private IList<IList<string>> _patterns = [];
    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        _patterns = inputLines.Split("").ToImmutableList();
    }

    protected override string LogicPart1()
    {
        var res = 0;

        for (int i = 0; i < _patterns.Count; i++)
        {
            IList<string>? pattern = _patterns[i];

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
            IList<string>? pattern = _patterns[i];

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

    private static int CountRowsAboveForPattern(IList<string> pattern, int acceptableErrors)
    {
        for (int i = 0; i < pattern.Count; i++)
        {
            if (CheckIfLinesReflected(pattern, i, acceptableErrors))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private static int CountColumnsBeforeForPattern(IList<string> pattern, int acceptableErrors)
    {
        for (int i = 0; i < pattern[0].Length; i++)
        {
            if (ColumnsReflected(pattern, i, acceptableErrors))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private static bool CheckIfLinesReflected(IList<string> pattern, int lineStartIndex, int acceptableErrors = 0)
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
            return true;
        }

        return false;
    }

    private static bool ColumnsReflected(IList<string> pattern, int columnStartIndex, int acceptableErrors = 0)
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
            return true;
        }

        return false;
    }
}
