using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day14 : Solution
{
    protected override string LogicPart1()
    {
        var tilted = TiltPlatform(inputLines);
        var load = CalculateTotalLoad(tilted);
        return load.ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private static IList<string> TiltPlatform(IList<string> platform)
    {
        for (int rowIdx = 1; rowIdx < platform.Count; rowIdx++)
        {
            for (int columnIdx = 0; columnIdx < platform[0].Length; columnIdx++)
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

        return platform;
    }

    private int CalculateTotalLoad(IList<string> platform)
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
