namespace AdventOfCode2023.Common;

public abstract class Solution
{
    protected string[] inputLines = [];
    protected virtual int DayNumber { get; init; }
    protected virtual bool UseExample => false;

    public Solution()
    {
        DayNumber = int.Parse(GetType().Name[3..]);
    }

    public void Run()
    {
        if (!ReadInputFile())
        {
            Console.WriteLine($"File for day {DayNumber} not found");
            return;
        }

        BeforeLogic();

        PrintResult(1, LogicPart1());
        PrintResult(2, LogicPart2());
    }

    protected bool ReadInputFile()
    {
        string fileName = GetInputFileName();
        if (!UseExample && File.Exists(fileName))
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

    protected void ReadInput(string fileName)
    {
        inputLines = File.ReadAllLines(fileName);
    }

    protected virtual void BeforeLogic()
    {

    }

    protected void PrintResult(int part, string res)
    {
        Console.WriteLine($"Answer for day {DayNumber}, part {part} is " + res);
    }

    protected abstract string LogicPart1();

    protected abstract string LogicPart2();
}
