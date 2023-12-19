using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day11 : Solution
{
    private readonly List<string> _expandedImage = [];
    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        // expand lines
        for (int i = 0; i < inputLines.Length; i++)
        {
            string? line = inputLines[i];
            _expandedImage.Add(line);
            
            if (!line.Contains('#'))
            {
                _expandedImage.Add(line);
            }
        }

        // expand columns
        for (int i = 0; i < _expandedImage[0].Length; i++)
        {
            var columnIsEmpty = !_expandedImage
                .Select(x => x[i])
                .Any(x => x == '#');
            
            if (columnIsEmpty)
            {
                for (int j = 0; j < _expandedImage.Count; j++)
                {
                    string? line = _expandedImage[j];
                    _expandedImage[j] = line.Insert(i + 1, ".");
                }

                i++;
            }
        }
    }

    protected override string LogicPart1()
    {
        var galaxies = new List<(int X, int Y)>();

        for (int i = 0; i < _expandedImage.Count; i++)
        {
            for (int j = 0; j < _expandedImage[i].Length; j++)
            {
                if (_expandedImage[i][j] == '#')
                {
                    galaxies.Add((j, i));
                }
            }
        }

        var length = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            (int X, int Y) galaxy = galaxies[i];

            for (int j = i + 1; j < galaxies.Count; j++)
            {
                length += CalculateDistance(galaxies[i], galaxies[j]);
            }
        }

        return length.ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private static int CalculateDistance((int X, int Y) galaxy, (int X, int Y) target)
    {
        return Math.Abs(galaxy.X - target.X) + Math.Abs(galaxy.Y - target.Y);
    }
}

