using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day17 : Solution
{
    private Node[,] Map { get; set; }
    private Dictionary<int, Node> Graph { get; set; } = [];

    protected override bool UseExample => true;
    private int printedLines = 0;

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        ParseInputMap();
    }

    protected override string LogicPart1()
    {
        var path = AStarPart1();

        Console.WriteLine(path[1..].Aggregate($"({path[0].Coordinates.RowIndex}, {path[0].Coordinates.ColumnIndex})", (acc, node) => $"{acc} -> ({node.Coordinates.RowIndex}, {node.Coordinates.ColumnIndex})")); ;
        Console.WriteLine(path[1..].Sum(node => node.Cost).ToString());

        return path[1..].Sum(node => node.Cost).ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private List<Node> AStarPart1()
    {
        // var avgStepCost = (int)Graph.Average(x => x.Value.Cost);

        var start = Graph.First().Value;
        var goal = Graph.Last().Value;
        var openSet = new HashSet<Node>{
            start
        };

        var cameFrom = new Dictionary<int, Node>();
        var gScore = Graph.ToDictionary(x => x.Key, x => int.MaxValue);
        gScore[start.Key] = 0;
        start.GScore = 0;

        var fScore = Graph.ToDictionary(x => x.Key, x => int.MaxValue);
        fScore[start.Key] = AStarEstimator(start, goal);
        start.FScore = fScore[start.Key];

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(node => fScore[node.Key]).First();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);

            var lastNodes = ReconstructPath(cameFrom, current, 5);
            var lastDirections = ReconstructDirections(lastNodes);
            var (lastDirection, speed) = ComputeSpeed(lastDirections);
            var possibleNeighbors = current.Neighbors;

            if (lastNodes.Count > 1)
            {
                possibleNeighbors.Remove(lastNodes[^2]);
            }

            if (speed >= 3)
            {
                possibleNeighbors = possibleNeighbors.FindAll(node =>
                    DirectionExtensions.FromChangeInCoordinates(node.Coordinates - current.Coordinates) != lastDirection
                );
            }

            foreach (var neighbor in possibleNeighbors)
            {
                var tentativeScore = gScore[current.Key] + neighbor.Cost;
                Console.WriteLine($"{++printedLines}. Checking node: {neighbor.Key}. gScore: {gScore[neighbor.Key]}, tentativeScore: {tentativeScore}");

                if (tentativeScore < gScore[neighbor.Key])
                {
                    cameFrom[neighbor.Key] = current;
                    gScore[neighbor.Key] = tentativeScore;
                    fScore[neighbor.Key] = tentativeScore + AStarEstimator(neighbor, goal);

                    openSet.Add(neighbor);
                }
            }
        }

        return [];

    }

    private int AStarEstimator(Node current, Node goal)
    {
        var distance = Math.Abs(goal.Coordinates.RowIndex - current.Coordinates.RowIndex)
            + Math.Abs(goal.Coordinates.ColumnIndex - current.Coordinates.ColumnIndex);

        // var verticalDistance = Graph
        //     .Where(x => x.Value.Coordinates.RowIndex > current.Coordinates.RowIndex 
        //         && x.Value.Coordinates.ColumnIndex == current.Coordinates.ColumnIndex)
        //     .Sum(x => x.Value.Cost);

        // var horizontalDistance = Graph
        //     .Where(x => x.Value.Coordinates.RowIndex == Map.GetLength(0) - 1
        //         && x.Value.Coordinates.ColumnIndex > current.Coordinates.ColumnIndex)
        //     .Sum(x => x.Value.Cost);

        // var distance = verticalDistance + horizontalDistance;

        return distance;
    }

    private List<Node> ReconstructPath(Dictionary<int, Node> cameFrom, Node current, int maxSteps = int.MaxValue)
    {
        var path = new List<Node> { current };
        maxSteps--;

        while (cameFrom.TryGetValue(current.Key, out current) && maxSteps > 0)
        {
            path.Add(current);
            maxSteps--;
        }

        path.Reverse();

        return path;
    }

    private List<Direction> ReconstructDirections(List<Node> nodes)
    {
        var lastDirections = new List<Direction>();

        Node? prev = null;
        foreach (var node in nodes)
        {
            if (prev is not null)
            {
                lastDirections.Add(DirectionExtensions.FromChangeInCoordinates(node.Coordinates - prev.Coordinates));
            }
            prev = node;
        }

        return lastDirections;
    }

    private (Direction, int) ComputeSpeed(List<Direction> directions)
    {
        if (directions.Count == 0)
        {
            return (Direction.None, 0);
        }

        var speed = 1;
        var prev = directions[0];
        foreach (var direction in directions[1..])
        {
            if (prev == direction)
            {
                speed++;
            }
            else
            {
                speed = 1;
            }

            prev = direction;
        }

        return (prev, speed);
    }

    private void ParseInputMap()
    {
        var width = inputLines[0].Length;
        var height = inputLines.Length;

        Map = new Node[height, width];

        var nodeIdx = 0;
        for (int i = 0; i < height; i++)
        {
            string row = inputLines[i];
            for (int j = 0; j < width; j++)
            {
                char character = row[j];
                var node = new Node
                {
                    Key = nodeIdx,
                    Coordinates = (i, j),
                    Cost = int.Parse(character.ToString())
                };

                Map[i, j] = node;
                Graph.Add(nodeIdx, node);

                nodeIdx++;
            }
        }

        foreach (var (key, node) in Graph)
        {
            var rowIdx = node.Coordinates.RowIndex;
            var colIdx = node.Coordinates.ColumnIndex;

            if (rowIdx > 0)
            {
                node.Neighbors.Add(Graph[key - width]);
            }

            if (rowIdx < height - 1)
            {
                node.Neighbors.Add(Graph[key + width]);
            }

            if (colIdx > 0)
            {
                node.Neighbors.Add(Graph[key - 1]);
            }

            if (colIdx < width - 1)
            {
                node.Neighbors.Add(Graph[key + 1]);
            }
        }
    }

    private record Node
    {
        public int Key { get; init; }
        public Coordinates Coordinates { get; init; }
        public int Cost { get; init; }
        public int Speed { get; init; } = 0;
        public Direction Direction { get; init; } = Direction.None;
        public int GScore { get; set; } = int.MaxValue;
        public int FScore { get; set; } = int.MaxValue;
        public Node? Parent { get; init; }
        public List<Node> Neighbors { get; init; } = [];
    }

    private class Path(Path.StepValidator stepValidator)
    {
        public List<Step> Steps { get; set; } = [];

        public int Cost => Steps.Sum(x => x.Cost);

        public delegate bool StepValidator(List<Step> previousSteps, Step nextStep);

        private StepValidator CurrentStepValidator { get; init; } = stepValidator;

        public bool TryAddStep(Step step)
        {
            if (CurrentStepValidator(Steps, step))
            {
                return false;
            }

            Steps.Add(step);

            return true;
        }

        public static bool IsValidStepPart1(List<Step> previousSteps, Step nextStep)
        {
            return previousSteps[^3..].All(x => x.Direction == nextStep.Direction);
        }
    }

    private readonly struct Step(Direction direction, Node from, Node to)
    {
        public Direction Direction { get; init; } = direction;
        public int Cost { get; init; } = to.Cost;
        public Node From { get; init; } = from;
        public Node To { get; init; } = to;
    }
}

