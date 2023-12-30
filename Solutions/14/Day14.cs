using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day14 : Solution
{
    protected override string LogicPart1()
    {
        var tilted = new List<string>(inputLines);
        TiltPlatformNorth(ref tilted);
        var load = CalculateTotalLoad(tilted);
        return load.ToString();
    }

    protected override string LogicPart2()
    {
        var states = new Dictionary<long, long>();
        var tilted = new List<string>(inputLines);

        for (long i = 1; i <= 1_000_000_000; i++)
        {
            var hash = GetHashCode(tilted);

            if (states.TryGetValue(hash, out long value))
            {
                var diff = i - value;
                var count = (long)Math.Floor((decimal)(1_000_000_000 - i) / diff);
                i += count * diff - 1;
                states = [];
                continue;
            }

            states[hash] = i;

            TiltPlatformNorth(ref tilted);
            TiltPlatformWest(ref tilted);
            TiltPlatformSouth(ref tilted);
            TiltPlatformEast(ref tilted);
        }

        var load = CalculateTotalLoad(tilted);
        return load.ToString();
    }

    private static long GetHashCode(List<string> platform)
    {
        return platform.Aggregate(0L, (acc, x) => x.GetHashCode() ^ acc);
    }

    private static void TiltPlatformNorth(ref List<string> platform)
    {
        int rowLength = platform[0].Length;
        int rowCount = platform.Count;

        for (int rowIdx = 1; rowIdx < rowCount; rowIdx++)
        {
            for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
            {
                if (platform[rowIdx][columnIdx] == 'O')
                {
                    var tempRowIdx = rowIdx;
                    while (tempRowIdx > 0 && platform[tempRowIdx - 1][columnIdx] == '.')
                    {
                        tempRowIdx--;
                    }

                    if (tempRowIdx != rowIdx)
                    {
                        platform[rowIdx] = platform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        platform[tempRowIdx] = platform[tempRowIdx].Remove(columnIdx, 1).Insert(columnIdx, "O");
                    }
                }
            }
        }
    }

    private static void TiltPlatformWest(ref List<string> platform)
    {
        int rowCount = platform.Count;
        int rowLength = platform[0].Length;

        for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
        {
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (platform[rowIdx][columnIdx] == 'O')
                {
                    var tempColumnIdx = columnIdx;
                    while (tempColumnIdx > 0 && platform[rowIdx][tempColumnIdx - 1] == '.')
                    {
                        tempColumnIdx--;
                    }

                    if (tempColumnIdx != columnIdx)
                    {
                        platform[rowIdx] = platform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        platform[rowIdx] = platform[rowIdx].Remove(tempColumnIdx, 1).Insert(tempColumnIdx, "O");
                    }
                }
            }
        }
    }

    private static void TiltPlatformSouth(ref List<string> platform)
    {
        int rowLength = platform[0].Length;
        int rowCount = platform.Count;

        for (int rowIdx = rowCount - 1; rowIdx >= 0; rowIdx--)
        {
            for (int columnIdx = 0; columnIdx < rowLength; columnIdx++)
            {
                if (platform[rowIdx][columnIdx] == 'O')
                {
                    var tempRowIdx = rowIdx;
                    while (tempRowIdx < rowCount - 1 && platform[tempRowIdx + 1][columnIdx] == '.')
                    {
                        tempRowIdx++;
                    }

                    if (tempRowIdx != rowIdx)
                    {
                        platform[rowIdx] = platform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        platform[tempRowIdx] = platform[tempRowIdx].Remove(columnIdx, 1).Insert(columnIdx, "O");
                    }
                }
            }
        }
    }

    private static void TiltPlatformEast(ref List<string> platform)
    {
        int rowCount = platform.Count;
        int rowLength = platform[0].Length;

        for (int columnIdx = rowLength - 1; columnIdx >= 0; columnIdx--)
        {
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (platform[rowIdx][columnIdx] == 'O')
                {
                    var tempColumnIdx = columnIdx;
                    while (tempColumnIdx < rowLength - 1 && platform[rowIdx][tempColumnIdx + 1] == '.')
                    {
                        tempColumnIdx++;
                    }

                    if (tempColumnIdx != columnIdx)
                    {
                        platform[rowIdx] = platform[rowIdx].Remove(columnIdx, 1).Insert(columnIdx, ".");
                        platform[rowIdx] = platform[rowIdx].Remove(tempColumnIdx, 1).Insert(tempColumnIdx, "O");
                    }
                }
            }
        }
    }

    private static int CalculateTotalLoad(IList<string> platform)
    {
        var res = 0;

        for (int i = 0; i < platform.Count; i++)
        {
            res += platform[i].Count(x => x == 'O') * (platform.Count - i);
        }

        return res;
    }
}
