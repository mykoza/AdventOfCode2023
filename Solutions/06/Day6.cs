using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day6 : Solution
{
    protected override int DayNumber { get; init; } = 6;

    protected override string LogicPart1()
    {
        var times = inputLines[0]
            .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var distances = inputLines[1]
            .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var races = new List<Race>();
        for (int i = 0; i < times.Length; i++)
        {
            races.Add(new Race(times[i], distances[i]));
        }

        var res = 1;
        foreach (var race in races)
        {
            var attempts = new Attempts(race);
            res *= attempts.NumberOfWinningAttempts();
        }

        return res.ToString();
    }

    protected override string LogicPart2()
    {
        var time = long.Parse(
            string.Concat(
                inputLines[0]
                    .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));

        var distance = long.Parse(
            string.Concat(
                inputLines[1]
                    .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));

        var race = new Race(time, distance);
        var attempts = new Attempts(race);

        return attempts.NumberOfWinningAttempts().ToString();
    }
}

public class Attempts(Race race)
{
    private readonly Race _race = race;

    public int NumberOfWinningAttempts()
    {
        var sqrtDelta = Math.Sqrt(Math.Pow(_race.Time, 2) - 4 * _race.RecordDistance);
        var left = Math.Floor(((-_race.Time + sqrtDelta) / -2) + 1);
        var right = Math.Ceiling(((-_race.Time - sqrtDelta) / -2) - 1);

        return (int)(right - left + 1);
    }
}

public record Race(long Time, long RecordDistance);
