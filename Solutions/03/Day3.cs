namespace AdventOfCode2023;

public class Day3 : Solution
{
    protected override int DayNumber { get; init; } = 3;
    private readonly char[] _digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    private List<Part> _parts = [];

    protected override string LogicPart1()
    {
        var nums = new List<Part>();
        PadInputLines();

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string line = _inputLines[i];
            bool prevWasNumber = false;
            string partDigits = string.Empty;
            List<Position> partPositions = [];
            string partSurroundings = string.Empty;

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];

                if (_digits.Contains(character))
                {
                    partDigits += character;
                    partPositions.Add(new Position(i, j));

                    if (!prevWasNumber && j > 0)
                    {
                        partSurroundings += line[j - 1];
                        partSurroundings += _inputLines[i - 1][j - 1];
                        partSurroundings += _inputLines[i + 1][j - 1];
                    }

                    partSurroundings += _inputLines[i - 1][j];
                    partSurroundings += _inputLines[i + 1][j];

                    if (j == line.Length - 1)
                    {
                        var part = new Part(
                            partDigits,
                            partPositions,
                            [.. partSurroundings]);

                        if (part.HasSymbols)
                        {
                            _parts.Add(part);
                        }
                    }

                    prevWasNumber = true;
                }
                else if (prevWasNumber)
                {
                    partSurroundings += character;
                    partSurroundings += _inputLines[i - 1][j];
                    partSurroundings += _inputLines[i + 1][j];

                    var part = new Part(
                        partDigits,
                        partPositions,
                        [.. partSurroundings]);

                    if (part.HasSymbols)
                    {
                        _parts.Add(part);
                    }

                    prevWasNumber = false;
                    partDigits = string.Empty;
                    partSurroundings = string.Empty;
                    partPositions = [];
                }
            }
        }

        return _parts.Sum(part => part.Value).ToString();
    }

    protected override string LoginPart2()
    {
        List<int> gearRatios = [];

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string line = _inputLines[i];
            List<Part> surroundingNumbers = [];
            List<Position> surroundingPositions = [];

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];

                if (character == '*')
                {
                    surroundingPositions.Add(new Position(i - 1, j));
                    surroundingPositions.Add(new Position(i + 1, j));

                    if (j > 0)
                    {
                        surroundingPositions.Add(new Position(i - 1, j - 1));
                        surroundingPositions.Add(new Position(i, j - 1));
                        surroundingPositions.Add(new Position(i + 1, j - 1));
                    }

                    if (j < line.Length - 1)
                    {
                        surroundingPositions.Add(new Position(i - 1, j + 1));
                        surroundingPositions.Add(new Position(i, j + 1));
                        surroundingPositions.Add(new Position(i + 1, j + 1));
                    }

                    foreach (var pos in surroundingPositions)
                    {
                        surroundingNumbers.AddRange(_parts.Where(part => part.Positions.Contains(pos)));
                    }

                    if (surroundingNumbers.Count > 0)
                    {
                        surroundingNumbers = surroundingNumbers.Distinct().ToList();
                    }

                    if (surroundingNumbers.Count == 2)
                    {
                        gearRatios.Add(surroundingNumbers[0].Value * surroundingNumbers[1].Value);
                    }

                    surroundingNumbers = [];
                    surroundingPositions = [];
                }
            }
        }

        return gearRatios.Sum().ToString();
    }

    private void PadInputLines()
    {
        var blankLine = new char[_inputLines[0].Length];
        Array.Fill(blankLine, '.');
        _inputLines = [new string(blankLine), .. _inputLines, new string(blankLine)];
    }
}

public record Part
{
    public string Digits;
    public int Value;
    public char[] Surroundings;
    public bool HasSymbols = false;
    public List<Position> Positions = [];
    private readonly char[] _nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    public Part(string digits, List<Position> positions, char[] surroundings)
    {
        Digits = digits;
        Value = int.Parse(digits);
        Positions = positions;
        Surroundings = surroundings;
        HasSymbols = surroundings.Any(character => !_nums.Contains(character) && character != '.');
    }


}

public record struct Position(int Line, int Column);
