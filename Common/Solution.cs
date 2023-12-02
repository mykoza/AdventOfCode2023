namespace AdventOfCode2023;

public abstract class Solution
{
    public void Run()
    {
        ReadInput("Inputs/" + DayNumber.ToString("D2") + "/input.txt");
        PrintResult(1, LogicPart1());
        PrintResult(2, LoginPart2());
    }

    protected string[] _inputLines = [];
    protected abstract int DayNumber {get; init;}

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
