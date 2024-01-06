using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day3 : Solution
{
    protected override int DayNumber { get; init; } = 3;
    private readonly char[] _digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    private List<Number> _parts = [];

    protected override string LogicPart1()
    {
        var nums = new List<Number>();
        PadInputLines();

        for (int i = 0; i < inputLines.Length; i++)
        {
            string line = inputLines[i];
            bool prevWasDigit = false;
            var number = new Number();

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];

                if (_digits.Contains(character))
                {
                    number.AddToDigits(character);

                    number.AddToPositions(new Coordinates(i, j));

                    if (!prevWasDigit && j > 0)
                    {
                        number.CheckAndSetHasSymbols(line[j - 1]);
                        number.CheckAndSetHasSymbols(inputLines[i - 1][j - 1]);
                        number.CheckAndSetHasSymbols(inputLines[i + 1][j - 1]);
                    }

                    number.CheckAndSetHasSymbols(inputLines[i - 1][j]);
                    number.CheckAndSetHasSymbols(inputLines[i + 1][j]);

                    if (j == line.Length - 1)
                    {
                        if (number.HasSymbols)
                        {
                            _parts.Add(number);
                        }

                        number = new Number();
                    }

                    prevWasDigit = true;
                }
                else if (prevWasDigit)
                {
                    number.CheckAndSetHasSymbols(character);
                    number.CheckAndSetHasSymbols(inputLines[i - 1][j]);
                    number.CheckAndSetHasSymbols(inputLines[i + 1][j]);

                    if (number.HasSymbols)
                    {
                        _parts.Add(number);
                    }

                    number = new Number();
                    prevWasDigit = false;
                }
            }
        }

        return _parts.Sum(part => part.Value).ToString();
    }

    protected override string LogicPart2()
    {
        List<int> gearRatios = [];

        for (int i = 0; i < inputLines.Length; i++)
        {
            string line = inputLines[i];
            List<Number> surroundingParts = [];
            List<Coordinates> surroundingPositions = [];

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];

                if (character == '*')
                {
                    surroundingPositions.Add(new Coordinates(i - 1, j));
                    surroundingPositions.Add(new Coordinates(i + 1, j));

                    if (j > 0)
                    {
                        surroundingPositions.Add(new Coordinates(i - 1, j - 1));
                        surroundingPositions.Add(new Coordinates(i, j - 1));
                        surroundingPositions.Add(new Coordinates(i + 1, j - 1));
                    }

                    if (j < line.Length - 1)
                    {
                        surroundingPositions.Add(new Coordinates(i - 1, j + 1));
                        surroundingPositions.Add(new Coordinates(i, j + 1));
                        surroundingPositions.Add(new Coordinates(i + 1, j + 1));
                    }

                    foreach (var pos in surroundingPositions)
                    {
                        surroundingParts.AddRange(_parts.Where(part => part.Positions.Contains(pos)));
                    }

                    if (surroundingParts.Count > 0)
                    {
                        surroundingParts = surroundingParts.Distinct().ToList();
                    }

                    if (surroundingParts.Count == 2)
                    {
                        gearRatios.Add(surroundingParts[0].Value * surroundingParts[1].Value);
                    }

                    surroundingParts = [];
                    surroundingPositions = [];
                }
            }
        }

        return gearRatios.Sum().ToString();
    }

    private void PadInputLines()
    {
        var blankLine = new char[inputLines[0].Length];
        Array.Fill(blankLine, '.');
        inputLines = [new string(blankLine), .. inputLines, new string(blankLine)];
    }


    public record Number
    {
        public string Digits { get; private set; } = "";
        public int Value { get; private set; } = 0;
        public bool HasSymbols = false;
        public List<Coordinates> Positions { get; init; } = [];
        private readonly char[] _nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

        public Number()
        {

        }

        public Number(string digits, List<Coordinates> positions, List<char> surroundings)
        {
            Digits = digits;
            Value = int.Parse(digits);
            Positions = positions;
            HasSymbols = surroundings.Any(character => character != '.' && !_nums.Contains(character));
        }

        public void CheckAndSetHasSymbols(char character)
        {
            if (HasSymbols == false && character != '.' && !_nums.Contains(character))
            {
                HasSymbols = true;
            }
        }

        public void AddToDigits(char character)
        {
            Digits += character;
            Value = int.Parse(Digits);
        }

        public void AddToPositions(Coordinates position)
        {
            Positions.Add(position);
        }
    }
}

