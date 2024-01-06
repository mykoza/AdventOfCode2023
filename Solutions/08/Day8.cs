using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day8 : Solution
{
    private string _instructions = "";
    private readonly Dictionary<string, Node> _nodes = [];

    protected override void BeforeLogic()
    {
        _instructions = inputLines[0];

        var inputNodes = inputLines[2..];

        foreach (var line in inputNodes)
        {
            var split = line.Split(" = ");
            var name = split[0];
            var lr = split[1].Trim(['(', ')']).Split(", ");
            var left = lr[0];
            var right = lr[1];

            _nodes.TryAdd(name, new Node(name));
            _nodes.TryAdd(left, new Node(left));
            _nodes.TryAdd(right, new Node(right));

            _nodes[name].Left = _nodes[left];
            _nodes[name].Right = _nodes[right];
        }
    }

    protected override string LogicPart1()
    {
        var currentNode = _nodes["AAA"];

        var steps = FindNodeZ(currentNode, "ZZZ");

        return steps.ToString();
    }

    protected override string LogicPart2()
    {
        var currentNodes = _nodes.Where(node => node.Value.Name.EndsWith('A')).ToDictionary();
        var numOfSteps = new List<long>();

        foreach (var node in currentNodes)
        {
            numOfSteps.Add(FindNodeZ(node.Value, "Z"));
        }

        return Maths.LeastCommonMultiple(numOfSteps).ToString();
    }

    private int FindNodeZ(Node currentNode, string endsWith)
    {
        var foundZ = false;
        var steps = 0;

        while (!foundZ)
        {
            foreach (var instruction in _instructions)
            {
                if (instruction == 'L')
                {
                    currentNode = currentNode.Left!;
                }
                else
                {
                    currentNode = currentNode.Right!;
                }

                steps++;

                if (currentNode.Name.EndsWith(endsWith))
                {
                    foundZ = true;
                    break;
                }
            }
        }

        return steps;
    }

    public class Node(string name, Node? left = null, Node? right = null)
    {
        private readonly string _name = name;
        public Node? Left { get; set; } = left;
        public Node? Right { get; set; } = right;

        public string Name => _name;
    }
}
