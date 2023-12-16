using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day9 : Solution
{
    private readonly List<List<int>> _histories = [];

    protected override void BeforeLogic()
    {
        foreach (var line in inputLines)
        {
            _histories.Add(line.Split(' ').Select(int.Parse).ToList());
        }
    }

    protected override string LogicPart1()
    {
        var predictions = new List<int>();
        foreach (var history in _histories)
        {
            var differences = new List<List<int>>
            {
                history
            };
            
            var lastDifferences = new List<int>(history);
            while (!lastDifferences.All(x => x == 0))
            {
                var currentDifferences = new List<int>();
                for (int i = 1; i < lastDifferences.Count; i++)
                {
                    currentDifferences.Add(lastDifferences[i] - lastDifferences[i-1]);
                }

                differences.Add(currentDifferences);
                lastDifferences = currentDifferences;
            }

            var prediction = 0;
            for (int i = differences.Count - 2; i >= 0; i--)
            {
                prediction += differences[i].Last();
            }

            predictions.Add(prediction);
        }

        return predictions.Sum().ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }
}
