namespace AdventOfCode2023;

public abstract class Solution
{
    public void Run()
    {
        ReadInput("Inputs/" + DayNumber.ToString("D2") + "/input.txt");
        var res = Logic();
        PrintResult(res);
    }

    protected string[] _inputLines = [];
    protected abstract int DayNumber {get; init;}

    protected void ReadInput(string fileName)
    {
        _inputLines = File.ReadAllLines(fileName);
    }

    protected abstract string Logic();

    protected void PrintResult(string res)
    {
        Console.WriteLine($"Answer for day {DayNumber} is " + res);
    }
}
