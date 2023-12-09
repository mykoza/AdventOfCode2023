namespace AdventOfCode2023;

public abstract class Solution
{
    public void Run()
    {
        if (!ReadInputFile())
        {
            Console.WriteLine($"File for day {DayNumber} not found");
            return;
        }

        PrintResult(1, LogicPart1());
        PrintResult(2, LoginPart2());
    }

    protected bool ReadInputFile()
    {
        string fileName = GetInputFileName();
        if (File.Exists(fileName))
        {
            ReadInput(fileName);
            return true;
        }

        string exampleFileName = GetExampleFileName();
        if (File.Exists(exampleFileName))
        {
            ReadInput(exampleFileName);
            return true;
        }

        return false;
    }

    protected string GetInputFileName()
    {
        return $"Inputs/{DayNumber:D2}/input.txt";
    }

    protected string GetExampleFileName()
    {
        return $"Inputs/{DayNumber:D2}/example.txt";
    }

    protected string[] _inputLines = [];
    protected abstract int DayNumber { get; init; }

    protected void ReadInput(string fileName)
    {
        _inputLines = File.ReadAllLines(fileName);
    }

    protected abstract string LogicPart1();

    protected abstract string LoginPart2();

    protected void PrintResult(int part, string res)
    {
        Console.WriteLine($"Answer for day {DayNumber}, part {part} is " + res);
    }
}
