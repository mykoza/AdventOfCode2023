using System.Buffers;

namespace AdventOfCode2023;

public class Day1 : Solution
{
    protected override int DayNumber { get; init; } = 1;

    private readonly char[] _numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];

    private readonly WordNumber[] _wordNumbersArray = [
        new WordNumber("one", '1'),
        new WordNumber("two", '2'),
        new WordNumber("three", '3'),
        new WordNumber("four", '4'),
        new WordNumber("five", '5'),
        new WordNumber("six", '6'),
        new WordNumber("seven", '7'),
        new WordNumber("eight", '8'),
        new WordNumber("nine", '9'),
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

    protected override string LoginPart2()
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
                char character = line[j];

                if (searchDigits.Contains(character))
                {
                    hits[j] = character;
                    goto AfterFirstDigit;
                }

                if (length - j > 2)
                {
                    foreach (var wordNumber in _wordNumbersArray)
                    {
                        if (line.Substring(j, Math.Min(wordNumber.Length, length - j)) == wordNumber.Word)
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
                char character = line[j];

                if (searchDigits.Contains(character))
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

            var chars = new char[2];
            chars[0] = hits.Values.First();
            chars[1] = hits.Values.Last();

            numbers[i] = int.Parse(new string(chars));
        }

        return numbers.Sum().ToString();
    }
}


public struct WordNumber
{
    public string Word { get; init;}
    public char Digit {get; init;}
    public char FirstLetter {get; init;}
    public int Length {get; init;}

    public WordNumber(string word, char digit)
    {
        Word = word;
        Digit = digit;
        FirstLetter = word[0];
        Length = word.Length;
    }
}