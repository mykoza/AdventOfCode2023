namespace AdventOfCode2023;

public class Day5 : Solution
{
    private Map _seedToSoil = new();
    private Map _soilToFertilizer = new();
    private Map _fertilizerToWater = new();
    private Map _waterToLight = new();
    private Map _lightToTemperature = new();
    private Map _temperatureToHumidity = new();
    private Map _humidityToLocation = new();

    protected override int DayNumber { get; init; } = 5;

    protected override void BeforeLogic()
    {
        ParseInput();
    }

    protected override string LogicPart1()
    {
        var seeds = _inputLines[0][7..].Split(' ').Select(long.Parse).ToList();
        var smallestLocation = long.MaxValue;
        foreach (var seed in seeds)
        {
            var location = FindLocationForSeed(seed);

            if (location < smallestLocation)
            {
                smallestLocation = location;
            }
        }

        return smallestLocation.ToString();
    }

    protected override string LogicPart2()
    {
        var values = _inputLines[0][7..].Split(' ').Select(long.Parse).ToList();
        var smallestLocation = long.MaxValue;
        for (int i = 0; i < values.Count; i += 2)
        {
            for (long seed = values[i]; seed < values[i] + values[i + 1]; seed++)
            {
                var location = FindLocationForSeed(seed);

                if (location < smallestLocation)
                {
                    smallestLocation = location;
                }
            }
        }

        return smallestLocation.ToString();
    }

    private void ParseInput()
    {
        var seedToSoilLines = new List<string>();
        var soilToFertilizerLines = new List<string>();
        var fertilizerToWaterLines = new List<string>();
        var waterToLightLines = new List<string>();
        var lightToTemperatureLines = new List<string>();
        var temperatureToHumidityLines = new List<string>();
        var humidityToLocationLines = new List<string>();

        for (int i = 2; i < _inputLines.Length; i++)
        {
            string? line = _inputLines[i];

            if (line == "seed-to-soil map:")
            {
                while (_inputLines[++i] != "")
                {
                    seedToSoilLines.Add(_inputLines[i]);
                }
                _seedToSoil = Map.Parse(seedToSoilLines);
            }
            else if (line == "soil-to-fertilizer map:")
            {
                while (_inputLines[++i] != "")
                {
                    soilToFertilizerLines.Add(_inputLines[i]);
                }
                _soilToFertilizer = Map.Parse(soilToFertilizerLines);
            }
            else if (line == "fertilizer-to-water map:")
            {
                while (_inputLines[++i] != "")
                {
                    fertilizerToWaterLines.Add(_inputLines[i]);
                }
                _fertilizerToWater = Map.Parse(fertilizerToWaterLines);
            }
            else if (line == "water-to-light map:")
            {
                while (_inputLines[++i] != "")
                {
                    waterToLightLines.Add(_inputLines[i]);
                }
                _waterToLight = Map.Parse(waterToLightLines);
            }
            else if (line == "light-to-temperature map:")
            {
                while (_inputLines[++i] != "")
                {
                    lightToTemperatureLines.Add(_inputLines[i]);
                }
                _lightToTemperature = Map.Parse(lightToTemperatureLines);
            }
            else if (line == "temperature-to-humidity map:")
            {
                while (_inputLines[++i] != "")
                {
                    temperatureToHumidityLines.Add(_inputLines[i]);
                }
                _temperatureToHumidity = Map.Parse(temperatureToHumidityLines);
            }
            else if (line == "humidity-to-location map:")
            {
                while (i < _inputLines.Length - 1 && _inputLines[++i] != "")
                {
                    humidityToLocationLines.Add(_inputLines[i]);
                }
                _humidityToLocation = Map.Parse(humidityToLocationLines);
            }
        }
    }
    
    private long FindLocationForSeed(long seed)
    {
        var soil = _seedToSoil.GetDestination(seed);
        var fertilizer = _soilToFertilizer.GetDestination(soil);
        var water = _fertilizerToWater.GetDestination(fertilizer);
        var light = _waterToLight.GetDestination(water);
        var temperature = _lightToTemperature.GetDestination(light);
        var humidity = _temperatureToHumidity.GetDestination(temperature);
        var location = _humidityToLocation.GetDestination(humidity);
        return location;
    }
}

public class Map
{
    private readonly List<Range> _ranges = [];

    public Map() { }

    public Map(List<Range> ranges)
    {
        _ranges = ranges;
    }

    public static Map Parse(List<string> input)
    {
        var map = new Map();

        foreach (var line in input)
        {
            var split = line.Split(' ');
            var destinationStart = long.Parse(split[0]);
            var sourceStart = long.Parse(split[1]);
            var range = int.Parse(split[2]);

            map.AddRange(new Range(sourceStart, destinationStart, range));
        }

        return map;
    }

    public void AddRange(Range range)
    {
        _ranges.Add(range);
    }

    public long GetDestination(long source)
    {
        long destination = default;
        foreach (var range in _ranges)
        {
            if (range.TryGetDestination(source, out destination))
            {
                return destination;
            }
        }

        if (destination == -1)
        {
            destination = source;
        }

        return destination;
    }
}

public class Range(long sourceStart, long destinationStart, long range)
{
    private readonly long _sourceStart = sourceStart;
    private readonly long _sourceEnd = sourceStart + range - 1;
    private readonly long _destinationStart = destinationStart;
    private readonly long _destinationEnd = destinationStart + range - 1;
    private readonly long _range = range;

    public bool TryGetDestination(long source, out long destination)
    {
        if (source >= _sourceStart && source <= _sourceEnd)
        {
            destination = _destinationStart + (source - _sourceStart);
            return true;
        }

        destination = -1;
        return false;
    }
}