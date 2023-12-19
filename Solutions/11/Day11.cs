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
        var expandedGalaxies = ExpandGalaxies(2);
        var length = 0;
        for (int i = 0; i < expandedGalaxies.Count; i++)
        {
            for (int j = i + 1; j < expandedGalaxies.Count; j++)
            {
                length += CalculateDistance(expandedGalaxies[i], expandedGalaxies[j]);
            }
        }

        return length.ToString();
    }

    protected override string LogicPart2()
    {
        var expandedGalaxies = ExpandGalaxies(1_000_000);
        long length = 0;
        for (int i = 0; i < expandedGalaxies.Count; i++)
        {
            for (int j = i + 1; j < expandedGalaxies.Count; j++)
            {
                checked
                {
                    length += CalculateDistance(expandedGalaxies[i], expandedGalaxies[j]);
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

    private List<(int X, int Y)> ExpandGalaxies(int expansionFactor)
    {
        expansionFactor -= 1;
        var expanded = new List<(int X, int Y)>(_galaxies.Count);

        for (int i = 0; i < _galaxies.Count; i++)
        {
            var numOfEmptyLinesCrossed = _emptyLines
                .Count(lineNum => lineNum < _galaxies[i].Y);

            var numOfEmptyColumnsCrossed = _emptyColumns
                .Count(lineNum => lineNum < _galaxies[i].X);

            expanded.Add((
                _galaxies[i].X + numOfEmptyColumnsCrossed * expansionFactor,
                _galaxies[i].Y + numOfEmptyLinesCrossed * expansionFactor
            ));
        }

        return expanded;
    }

    private int CalculateDistance((int X, int Y) galaxy, (int X, int Y) target)
    {
        return Math.Abs(galaxy.X - target.X) + Math.Abs(galaxy.Y - target.Y);
    }
}

