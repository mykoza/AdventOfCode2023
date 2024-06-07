using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day17 : Solution
{
    protected override bool UseExample => false;
    private int _numOfNodes = 0;
    private Node[,] Map { get; set; } = new Node[1, 1];
    private Dictionary<int, Node> Graph { get; set; } = [];
    private delegate bool CanContinueDelegate(NodeIdentifier prev, NodeIdentifier next);

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        ParseInputMap();

        _numOfNodes = Map.Length;
    }

    protected override string LogicPart1()
    {
        var cost = AStar(CanContinuePart1);
        return cost.ToString();
    }

    protected override string LogicPart2()
    {
        var cost = AStar(CanContinuePart2);
        return cost.ToString();
    }

    private int AStar(CanContinueDelegate canContinueDelegate)
    {
        var start = Graph.First().Value;
        var goal = Graph.Last().Value;
        var openSet = new HashSet<Node>{
            start
        };

        var cameFrom = new Dictionary<NodeIdentifier, Node>();

        var gScore = new Dictionary<NodeIdentifier, int>
        {
            [start.ToIdentifier()] = 0
        };

        var fScore = new Dictionary<NodeIdentifier, int>
        {
            [start.ToIdentifier()] = AStarEstimator(start, goal)
        };

        while (openSet.Count > 0)
        {
            var current = openSet
                .OrderBy(node => fScore.GetValueOrDefault(node.ToIdentifier(), int.MaxValue))
                .First();

            var currentIdentifier = current.ToIdentifier();

            if (current.Coordinates == goal.Coordinates)
            {
                PrintPath(cameFrom, current);
                return CalculatePathCost(cameFrom, current);
            }

            openSet.Remove(current);

            var currentCameWithDirection = current.Direction;
            var currentSpeed = current.Speed;

            foreach (var neighbor in current.Neighbors)
            {
                var neighborDirection = current.Coordinates.DirectionTo(neighbor.Coordinates);

                var newNeighbor = neighbor with
                {
                    Direction = neighborDirection,
                    Speed = neighborDirection == current.Direction ? currentSpeed + 1 : 1,
                };

                var newNeighborIdentifier = newNeighbor.ToIdentifier();

                if (!canContinueDelegate(currentIdentifier, newNeighborIdentifier))
                {
                    continue;
                }

                var tentativeScore = gScore.GetValueOrDefault(currentIdentifier, int.MaxValue) + newNeighbor.Cost;
                var neighborGScore = gScore.GetValueOrDefault(newNeighborIdentifier, int.MaxValue);

                if (tentativeScore < neighborGScore)
                {
                    cameFrom[newNeighborIdentifier] = current;
                    gScore[newNeighborIdentifier] = tentativeScore;
                    fScore[newNeighborIdentifier] = tentativeScore + AStarEstimator(newNeighbor, goal);

                    openSet.Add(newNeighbor);
                }
            }
        }

        return -1;
    }

    private bool CanContinuePart1(NodeIdentifier prev, NodeIdentifier next)
    {
        return CanContinue(prev, next, 0, 3);
    }

    private bool CanContinuePart2(NodeIdentifier prev, NodeIdentifier next)
    {
        return CanContinue(prev, next, 4, 10);
    }

    private bool CanContinue(NodeIdentifier prev, NodeIdentifier next, int minSpeed, int maxSpeed)
    {
        if (next.Key == _numOfNodes - 1 && (next.Speed < minSpeed || next.Speed > maxSpeed))
        {
            return false;
        }

        if (prev.Direction == Direction.None)
        {
            return true;
        }

        if (prev.Direction.IsOppositeTo(next.Direction))
        {
            return false;
        }

        if (prev.Speed >= minSpeed && next.Speed <= maxSpeed)
        {
            return true;
        }

        if (prev.Speed < minSpeed && prev.Direction == next.Direction)
        {
            return true;
        }

        return false;
    }

    private static int AStarEstimator(Node current, Node goal)
    {
        var distance = Math.Abs(goal.Coordinates.RowIndex - current.Coordinates.RowIndex)
            + Math.Abs(goal.Coordinates.ColumnIndex - current.Coordinates.ColumnIndex);

        return distance;
    }

    private static void PrintPath(Dictionary<NodeIdentifier, Node> cameFrom, Node lastNode)
    {
        var path = ReconstructPath(cameFrom, lastNode);

        Console.WriteLine(path.Aggregate("", (acc, node) => acc + $"({node.Coordinates.RowIndex}, {node.Coordinates.ColumnIndex}) -> "));
    }


    private static List<Node> ReconstructPath(Dictionary<NodeIdentifier, Node> cameFrom, Node current, int maxSteps = int.MaxValue)
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

    private static int CalculatePathCost(Dictionary<NodeIdentifier, Node> cameFrom, Node lastNode)
    {
        var cost = lastNode.Cost;

        Node? current = lastNode;
        while (cameFrom.TryGetValue(current.ToIdentifier(), out current) && current.Key != 0)
        {
            cost += current.Cost;
        }

        return cost;
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

    private readonly struct NodeIdentifier : IEquatable<NodeIdentifier>
    {
        public int Key { get; init; }
        public Direction Direction { get; init; }
        public int Speed { get; init; }

        public bool Equals(NodeIdentifier other)
        {
            return Key == other.Key
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
            return new NodeIdentifier
            {
                Key = Key,
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
}

