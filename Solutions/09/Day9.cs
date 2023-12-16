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
        var sumOfPredictions = 0;
        foreach (var history in _histories)
        {
            var values = new List<List<int>>
            {
                history,
            };

            values.AddRange(CountDifferences(history));

            var prediction = 0;
            for (int i = values.Count - 2; i >= 0; i--)
            {
                prediction += values[i].Last();
            }

            sumOfPredictions += prediction;
        }

        return sumOfPredictions.ToString();
    }

    protected override string LogicPart2()
    {
        var sumOfPredictions = 0;
        foreach (var history in _histories)
        {
            var values = new List<List<int>>
            {
                history,
            };

            values.AddRange(CountDifferences(history));

            var prediction = 0;
            for (int i = values.Count - 2; i >= 0; i--)
            {
                prediction = values[i][0] - prediction;
            }

            sumOfPredictions += prediction;
        }

        return sumOfPredictions.ToString();
    }

    private static List<List<int>> CountDifferences(List<int> initialValues)
    {
        var differences = new List<List<int>>();

        var lastDifferences = initialValues;
        while (!lastDifferences.All(x => x == 0))
        {
            var currentDifferences = new List<int>();
            for (int i = 1; i < lastDifferences.Count; i++)
            {
                currentDifferences.Add(lastDifferences[i] - lastDifferences[i - 1]);
            }

            differences.Add(currentDifferences);
            lastDifferences = currentDifferences;
        }

        return differences;
    }
}
