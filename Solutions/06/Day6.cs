namespace AdventOfCode2023;

public class Day6 : Solution
{
    protected override int DayNumber { get; init; } = 6;

    private readonly List<Race> _races = [];

    protected override void BeforeLogic()
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

        for (int i = 0; i < times.Length; i++)
        {
            _races.Add(new Race(times[i], distances[i]));
        }
    }

    protected override string LogicPart1()
    {
        var numberOfWaysToWin = 1;

        foreach (var race in _races)
        {
            var attempts = new Attempts(race);
            numberOfWaysToWin *= attempts.WinningAttempts().Count;
        }

        return numberOfWaysToWin.ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }
}

public class Attempts
{
    private readonly Race _race;
    private readonly List<Attempt> _attempts = [];

    public Attempts(Race race)
    {
        _race = race;

        for (int holdTime = 1; holdTime < race.Time; holdTime++)
        {
            _attempts.Add(new Attempt(holdTime, race.Time));
        }
    }

    public List<Attempt> WinningAttempts()
    {
        return _attempts.Where(a => a.DistanceTraveled > _race.RecordDistance).ToList();
    }
}

public class Attempt(int holdTime, int maxTime)
{
    private readonly int _holdTime = holdTime;
    private readonly int _maxTime = maxTime;
    private readonly int _distanceTraveled = holdTime * (maxTime - holdTime);

    public int DistanceTraveled => _distanceTraveled;
}

public record Race(int Time, int RecordDistance)
{
    public bool RecordBeaten(int distance)
    {
        return distance < RecordDistance;
    }
}

public class Boat(int buttonHoldTime)
{
    private readonly int _speed = buttonHoldTime;

    public int DistanceTraveledInTime(int time)
    {
        return _speed * time;
    }
}