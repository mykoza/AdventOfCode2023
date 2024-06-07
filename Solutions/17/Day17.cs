using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day17 : Solution
{
    private Node[,] Map { get; set; }
    private Dictionary<int, Node> Graph { get; set; } = [];
    private int _numOfOperations = 0;

    protected override bool UseExample => false;

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        ParseInputMap();
    }

    protected override string LogicPart1()
    {
        var cost = AStarPart1();

        Console.WriteLine($"Number of operations {_numOfOperations}");


        // Console.WriteLine(path[1..].Aggregate($"({path[0].Coordinates.RowIndex}, {path[0].Coordinates.ColumnIndex})", (acc, node) => $"{acc} -> ({node.Coordinates.RowIndex}, {node.Coordinates.ColumnIndex})")); ;
        // Console.WriteLine(path[1..].Sum(node => node.Cost).ToString());

        return cost.ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private int AStarPart1()
    {
        var start = Graph.First().Value;
        var goal = Graph.Last().Value;
        var openSet = new HashSet<Node>{
            start
        };

        // rowIndex, columnIndex, fromDirection, speed
        var cameFrom = new Dictionary<NodeIdentifier, Node>();

        var gScore = new Dictionary<NodeIdentifier, int>();
        gScore[start.ToIdentifier()] = 0;

        var fScore = new Dictionary<NodeIdentifier, int>();
        fScore[start.ToIdentifier()] = AStarEstimator(start, goal);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(node => fScore.GetValueOrDefault(node.ToIdentifier(), int.MaxValue))
                .First();

            var currentIdentifier = current.ToIdentifier();

            if (current.Coordinates == goal.Coordinates)
            {
                // return ReconstructPath(cameFrom, current);
                return CalculatePathCost(cameFrom, current);
            }

            openSet.Remove(current);

            var lastNodes = ReconstructPath(cameFrom, current, 5);
            // var lastDirections = ReconstructDirections(lastNodes);
            // var (lastDirection, speed) = ComputeSpeed(lastDirections);
            var currentCameWithDirection = current.Direction;
            var currentSpeed = current.Speed;

            // if (lastNodes.Count > 1)
            // {
            //     possibleNeighbors = possibleNeighbors.FindAll(node =>
            //         node.Key != lastNodes[^2].Key);
            // }

            // if (speed >= 3)
            // {
            //     possibleNeighbors = possibleNeighbors.FindAll(node =>
            //         DirectionExtensions.FromChangeInCoordinates(node.Coordinates - current.Coordinates) != lastDirection
            //     );
            // }

            foreach (var neighbor in current.Neighbors)
            {
                ++_numOfOperations;
                var neighborCameWithDirection = DirectionExtensions.FromChangeInCoordinates(neighbor.Coordinates - current.Coordinates);

                if (DirectionExtensions.IsOpposite(current.Direction, neighborCameWithDirection))
                {
                    continue;
                }

                if (current.Direction == neighborCameWithDirection && currentSpeed >= 3)
                {
                    continue;
                }

                
                var newNeighbor = neighbor with {
                    Direction = neighborCameWithDirection,
                    Speed = neighborCameWithDirection == current.Direction ? currentSpeed + 1 : 1,
                };

                var tentativeScore = gScore.GetValueOrDefault(currentIdentifier, int.MaxValue) + newNeighbor.Cost;

                var neighborGScore = gScore.GetValueOrDefault(newNeighbor.ToIdentifier(), int.MaxValue);

                if (tentativeScore < neighborGScore)
                {
                    var identifier = newNeighbor.ToIdentifier();

                    cameFrom[identifier] = current;

                    gScore[identifier] = tentativeScore;

                    fScore[identifier] = tentativeScore + AStarEstimator(newNeighbor, goal);

                    openSet.Add(newNeighbor);
                }
            }
        }

        return -1;
    }

    private int AStarEstimator(Node current, Node goal)
    {
        var distance = Math.Abs(goal.Coordinates.RowIndex - current.Coordinates.RowIndex)
            + Math.Abs(goal.Coordinates.ColumnIndex - current.Coordinates.ColumnIndex);

        return distance;
    }

    private List<Node> ReconstructPath(Dictionary<NodeIdentifier, Node> cameFrom, Node current, int maxSteps = int.MaxValue)
    {
        var path = new List<Node> { current };
        maxSteps--;

        while (cameFrom.TryGetValue(current.ToIdentifier(), out current) && maxSteps > 0)
        {
            path.Add(current);
            maxSteps--;
        }

        path.Reverse();

        return path;
    }

    private int CalculatePathCost(Dictionary<NodeIdentifier, Node> cameFrom, Node lastNode)
    {
        var cost = lastNode.Cost;


        var first = cameFrom[new NodeIdentifier {Key = 1, Direction = Direction.Right, Speed = 1}];

        Node? current = lastNode;
        while (cameFrom.TryGetValue(current.ToIdentifier(), out current) && current.Key != 0)
        {
            cost += current.Cost;
        }

        return cost;
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

    private readonly struct NodeIdentifier : IEquatable<NodeIdentifier>{
        public int Key { get; init; }
        // public Coordinates Coordinates { get; init; }
        public Direction Direction { get; init; }
        public int Speed { get; init; }

        public bool Equals(NodeIdentifier other)
        {
            return Key == other.Key
                // && Coordinates == other.Coordinates
                && Direction == other.Direction
                && Speed == other.Speed;
        }

        public override bool Equals(object? obj)
        {
            return obj is NodeIdentifier identifier && Equals(identifier);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Direction, Speed);
        }
    }

    private record Node
    {
        public int Key { get; init; }
        public Coordinates Coordinates { get; init; }
        public int Cost { get; init; }
        public int Speed { get; init; } = 0;
        public Direction Direction { get; init; } = Direction.None;
        public Node? Parent { get; init; }
        public List<Node> Neighbors { get; init; } = [];

        public NodeIdentifier ToIdentifier()
        {
            return new NodeIdentifier {
                Key = Key,
                // Coordinates = Coordinates,
                Direction = Direction,
                Speed = Speed
            };
        }

        public virtual bool Equals(Node? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (
                Key == other.Key
                && Speed == other.Speed
                && Direction == other.Direction
                && Parent?.Key == other.Parent?.Key
            )
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Speed, Direction, Parent?.Key);
        }
    }

    // private class Path(Path.StepValidator stepValidator)
    // {
    //     public List<Step> Steps { get; set; } = [];

    //     public int Cost => Steps.Sum(x => x.Cost);

    //     public delegate bool StepValidator(List<Step> previousSteps, Step nextStep);

    //     private StepValidator CurrentStepValidator { get; init; } = stepValidator;

    //     public bool TryAddStep(Step step)
    //     {
    //         if (CurrentStepValidator(Steps, step))
    //         {
    //             return false;
    //         }

    //         Steps.Add(step);

    //         return true;
    //     }

    //     public static bool IsValidStepPart1(List<Step> previousSteps, Step nextStep)
    //     {
    //         return previousSteps[^3..].All(x => x.Direction == nextStep.Direction);
    //     }
    // }

    // private readonly struct Step(Direction direction, Node from, Node to)
    // {
    //     public Direction Direction { get; init; } = direction;
    //     public int Cost { get; init; } = to.Cost;
    //     public Node From { get; init; } = from;
    //     public Node To { get; init; } = to;
    // }
}

