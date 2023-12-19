using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day11 : Solution
{
    private readonly List<int> _emptyLines = [];
    private readonly List<int> _emptyColumns = [];
    private readonly List<(int X, int Y)> _galaxies = [];

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        FindEmptyLines();
        FindEmptyColumns();
        FindGalaxies();
    }

    protected override string LogicPart1()
    {
        var length = 0;
        for (int i = 0; i < _galaxies.Count; i++)
        {
            for (int j = i + 1; j < _galaxies.Count; j++)
            {
                length += CalculateDistance(_galaxies[i], _galaxies[j], 2);
            }
        }

        return length.ToString();
    }

    protected override string LogicPart2()
    {
        long length = 0;
        for (int i = 0; i < _galaxies.Count; i++)
        {
            for (int j = i + 1; j < _galaxies.Count; j++)
            {
                checked
                {
                    length += CalculateDistance(_galaxies[i], _galaxies[j], 1_000_000);
                }
            }
        }

        return length.ToString();
    }

    private void FindEmptyLines()
    {
        for (int i = 0; i < inputLines.Length; i++)
        {
            if (!inputLines[i].Contains('#'))
            {
                _emptyLines.Add(i);
            }
        }
    }
    
    private void FindEmptyColumns()
    {
        for (int i = 0; i < inputLines[0].Length; i++)
        {
            var columnIsEmpty = !inputLines
                .Select(x => x[i])
                .Any(x => x == '#');

            if (columnIsEmpty)
            {
                _emptyColumns.Add(i);
            }
        }
    }
    
    private void FindGalaxies()
    {
        for (int i = 0; i < inputLines.Length; i++)
        {
            for (int j = 0; j < inputLines[i].Length; j++)
            {
                if (inputLines[i][j] == '#')
                {
                    _galaxies.Add((j, i));
                }
            }
        }
    }

    private int CalculateDistance((int X, int Y) galaxy, (int X, int Y) target, int expansionFactor)
    {
        expansionFactor -= 1;

        var numOfEmptyLinesCrossed = _emptyLines
            .Count(lineNum => lineNum > Math.Min(galaxy.Y, target.Y) && lineNum < Math.Max(galaxy.Y, target.Y));

        var numOfEmptyColumnsCrossed = _emptyColumns
            .Count(columnNum => columnNum > Math.Min(galaxy.X, target.X) && columnNum < Math.Max(galaxy.X, target.X));

        return Math.Abs(galaxy.X - target.X)
               + Math.Abs(galaxy.Y - target.Y)
               + (numOfEmptyLinesCrossed * expansionFactor)
               + (numOfEmptyColumnsCrossed * expansionFactor);
    }
}

