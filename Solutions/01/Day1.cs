using System.Buffers;

namespace AdventOfCode2023;

public class Day1 : Solution
{
    private readonly char[] _numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

    protected override int DayNumber { get; init; } = 1;

    protected override string Logic()
    {
        int[] numbers = new int[_inputLines.Length];
        var searchDigits = SearchValues.Create(_numbers);

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string? line = _inputLines[i];
            var chars = new char[2];

            foreach (var character in line)
            {
                if (searchDigits.Contains(character))
                {
                    chars[0] = character;
                    break;
                }
            }

            foreach (var character in line.Reverse())
            {
                if (searchDigits.Contains(character))
                {
                    chars[1] = character;
                    break;
                }
            }

            numbers[i] = int.Parse(new string(chars));
        }

        return numbers.Sum().ToString();
    }
}
