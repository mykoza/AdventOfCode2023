using System.Buffers;

namespace AdventOfCode2023;

public class Day1 : Solution
{
    protected override int DayNumber { get; init; } = 1;

    private readonly char[] _numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

    private readonly WordNumber[] _wordNumbersArray = [
        new("one", '1'),
        new("two", '2'),
        new("three", '3'),
        new("four", '4'),
        new("five", '5'),
        new("six", '6'),
        new("seven", '7'),
        new("eight", '8'),
        new("nine", '9'),
    ];

    protected override string LogicPart1()
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

    protected override string LogicPart2()
    {
        int[] numbers = new int[_inputLines.Length];
        var searchDigits = SearchValues.Create(_numbers);

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string line = _inputLines[i];
            int length = line.Length;
            var hits = new Dictionary<int, char>();

            // find first digit and break
            for (int j = 0; j < length; j++)
            {
                if (searchDigits.Contains(line[j]))
                {
                    hits[j] = line[j];
                    goto AfterFirstDigit;
                }

                if (length - j > 2)
                {
                    foreach (var wordNumber in _wordNumbersArray)
                    {
                        if (length - j >= wordNumber.Length && line.Substring(j, wordNumber.Length) == wordNumber.Word)
                        {
                            hits[j] = wordNumber.Digit;
                            goto AfterFirstDigit;
                        }
                    }
                }
            }

            AfterFirstDigit:

            // find last digit and break
            for (int j = length - 1; j >= 0; j--)
            {
                if (searchDigits.Contains(line[j]))
                {
                    hits[j] = line[j];
                    goto AfterLastDigit;
                }

                if (j < length - 2 && j > 0)
                {
                    foreach (var wordNumber in _wordNumbersArray)
                    {
                        if (length - j >= wordNumber.Length && line.Substring(j, wordNumber.Length) == wordNumber.Word)
                        {
                            hits[j] = wordNumber.Digit;
                            goto AfterLastDigit;
                        }
                    }
                }
            }

            AfterLastDigit:

            numbers[i] = int.Parse([hits.Values.First(), hits.Values.Last()]);
        }

        return numbers.Sum().ToString();
    }
}


public readonly struct WordNumber(string word, char digit)
{
    public string Word { get; init; } = word;
    public char Digit { get; init; } = digit;
    public char FirstLetter { get; init; } = word[0];
    public int Length { get; init; } = word.Length;
}