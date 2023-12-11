namespace AdventOfCode2023;

public class Day6 : Solution
{
    protected override int DayNumber { get; init; } = 6;

    protected override string LogicPart1()
    {
        var times = _inputLines[0]
            .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var distances = _inputLines[1]
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
                _inputLines[0]
                    .Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)));

        var distance = long.Parse(
            string.Concat(
                _inputLines[1]
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
        var count = 0;
        foreach (var attempt in YieldAttempts())
        {
            if (_race.RecordBeaten(attempt.DistanceTraveled))
            {
                count++;
            }
        }

        return count;
    }

    public IEnumerable<Attempt> YieldAttempts()
    {
        for (int holdTime = 1; holdTime < _race.Time; holdTime++)
        {
            yield return new Attempt(holdTime, _race.Time);
        }

        yield break;
    }
}

public class Attempt(long holdTime, long maxTime)
{
    public long DistanceTraveled { get; } = holdTime * (maxTime - holdTime);
}

public record Race(long Time, long RecordDistance)
{
    public bool RecordBeaten(long distance)
    {
        return distance > RecordDistance;
    }
}
