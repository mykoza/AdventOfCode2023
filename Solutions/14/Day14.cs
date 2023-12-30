using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day14 : Solution
{
    protected override string LogicPart1()
    {
        var tilted = TiltPlatformNorth(inputLines);
        var load = CalculateTotalLoad(tilted);
        return load.ToString();
    }

    protected override string LogicPart2()
    {
        var states = new Dictionary<int, long>();
        var tilted = new List<string>(inputLines);

        for (long i = 1; i <= 1_000_000_000; i++)
        {
            var hash = tilted.GetHashCode();

            if (states.TryGetValue(hash, out long value))
            {
                var diff = i - value;
                var count = (long)Math.Floor((decimal)(1_000_000_000 - i) / diff);
                i += count * diff;
                states = [];
                continue;
            }

            states[hash] = i;

            tilted = TiltPlatformNorth(tilted);
            tilted = TiltPlatformWest(tilted);
            tilted = TiltPlatformSouth(tilted);
            tilted = TiltPlatformEast(tilted);
        }

        var load = CalculateTotalLoad(tilted);
        return load.ToString();
    }

    private static List<string> TiltPlatformNorth(IList<string> platform)
    {
        var newPlatform = new List<string>(platform);
        int rowLength = newPlatform[0].Length;
        int rowCount = newPlatform.Count;

        for (int rowIdx = 1; rowIdx < rowCount; rowIdx++)
        {
            for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
            {
                if (newPlatform[rowIdx][columnIdx] == 'O')
                {
                    var tempRowIdx = rowIdx;
                    while (tempRowIdx > 0 && newPlatform[tempRowIdx - 1][columnIdx] == '.')
                    {
                        tempRowIdx--;
                    }

                    if (tempRowIdx != rowIdx)
                    {
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        newPlatform[tempRowIdx] = newPlatform[tempRowIdx].Remove(columnIdx, 1).Insert(columnIdx, "O");
                    }
                }
            }
        }

        return newPlatform;
    }

    private static List<string> TiltPlatformWest(IList<string> platform)
    {
        var newPlatform = new List<string>(platform);
        int rowCount = newPlatform.Count;
        int rowLength = newPlatform[0].Length;

        for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
        {
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (newPlatform[rowIdx][columnIdx] == 'O')
                {
                    var tempColumnIdx = columnIdx;
                    while (tempColumnIdx > 0 && newPlatform[rowIdx][tempColumnIdx - 1] == '.')
                    {
                        tempColumnIdx--;
                    }

                    if (tempColumnIdx != columnIdx)
                    {
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(tempColumnIdx, 1).Insert(tempColumnIdx, "O");
                    }
                }
            }
        }

        return newPlatform;
    }

    private static List<string> TiltPlatformSouth(IList<string> platform)
    {
        var newPlatform = new List<string>(platform);
        int rowLength = newPlatform[0].Length;
        int rowCount = newPlatform.Count;

        for (int rowIdx = rowCount - 1; rowIdx >= 0; rowIdx--)
        {
            for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
            {
                if (newPlatform[rowIdx][columnIdx] == 'O')
                {
                    var tempRowIdx = rowIdx;
                    while (tempRowIdx < rowCount - 1 && newPlatform[tempRowIdx + 1][columnIdx] == '.')
                    {
                        tempRowIdx++;
                    }

                    if (tempRowIdx != rowIdx)
                    {
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        newPlatform[tempRowIdx] = newPlatform[tempRowIdx].Remove(columnIdx, 1).Insert(columnIdx, "O");
                    }
                }
            }
        }

        return newPlatform;
    }

    private static List<string> TiltPlatformEast(IList<string> platform)
    {
        var newPlatform = new List<string>(platform);
        int rowCount = newPlatform.Count;
        int rowLength = newPlatform[0].Length;

        for (int columnIdx = rowLength - 1; columnIdx >= 0; columnIdx--)
        {
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (newPlatform[rowIdx][columnIdx] == 'O')
                {
                    var tempColumnIdx = columnIdx;
                    while (tempColumnIdx < rowLength - 1 && newPlatform[rowIdx][tempColumnIdx + 1] == '.')
                    {
                        tempColumnIdx++;
                    }

                    if (tempColumnIdx != columnIdx)
                    {
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        newPlatform[rowIdx] = newPlatform[rowIdx].Remove(tempColumnIdx, 1).Insert(tempColumnIdx, "O");
                    }
                }
            }
        }

        return newPlatform;
    }

    private static int CalculateTotalLoad(IList<string> platform)
    {
        var rowCount = platform.Count;
        var res = 0;

        for (int i = 0; i < rowCount; i++)
        {
            res += platform[i].Count(x => x == 'O') * (rowCount - i);
        }

        return res;
    }
}
