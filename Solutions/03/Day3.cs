namespace AdventOfCode2023;

public class Day3 : Solution
{
    protected override int DayNumber { get; init; } = 3;

    private readonly char[] _nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    
    protected override string LogicPart1()
    {
        var nums = new List<Number>();
        var firstLine = new char[_inputLines[0].Length];
        Array.Fill(firstLine, '.');

        _inputLines = [new string(firstLine), .. _inputLines, new string(firstLine)];

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string line = _inputLines[i];
            bool prevWasNumber = false;
            string digits = string.Empty;
            string surroundings = string.Empty;
            bool beginningOfLine = true;

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];

                if (_nums.Contains(character))
                {
                    digits += character;

                    if (!prevWasNumber && !beginningOfLine)
                    {
                        surroundings += line[j-1];
                        surroundings += _inputLines[i-1][j-1];
                        surroundings += _inputLines[i+1][j-1];
                    }

                    surroundings += _inputLines[i-1][j];
                    surroundings += _inputLines[i+1][j];

                    if (j == 139)
                    {
                        var number = new Number(digits, [.. surroundings]); 

                        if (number.HasSymbols)
                        {
                            nums.Add(number);
                        }
                    }

                    prevWasNumber = true;
                }
                else if (prevWasNumber)
                {
                    surroundings += character;
                    surroundings += _inputLines[i-1][j];
                    surroundings += _inputLines[i+1][j];

                    var number = new Number(digits, [.. surroundings]); 

                    if (number.HasSymbols)
                    {
                        nums.Add(number);
                    }
                    
                    prevWasNumber = false;
                    digits = string.Empty;
                    surroundings = string.Empty;
                }

                beginningOfLine = false;
            }
        }

        return nums.Sum(number => number.Value).ToString();
    }

    protected override string LoginPart2()
    {
        throw new NotImplementedException();
    }
}

public record Number
{
    public string Digits;
    public int Value;
    public char[] Surroundings;
    public bool HasSymbols = false;
    private readonly char[] _nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    public Number(string digits, char[] surroundings)
    {
        Digits = digits;
        Value = int.Parse(digits);
        Surroundings = surroundings;
        HasSymbols = surroundings.Any(character => !_nums.Contains(character) && character != '.');
    }
}