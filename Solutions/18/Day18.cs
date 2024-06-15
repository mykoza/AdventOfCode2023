using System.Drawing;
using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day18 : Solution
{
    protected override bool UseExample => false;
    private readonly List<Cube> _cubes = [];
    private double _area = 0;
    private List<Instruction> _instructions = [];

    protected override void BeforeLogic()
    {
        base.BeforeLogic();

        _instructions = ParseInput();
    }

    protected override string LogicPart1()
    {
        Coordinates currentPosition = (0, 0);

        foreach (var instruction in _instructions)
        {
            currentPosition = RunInstruction(instruction, currentPosition);
        }

        _area += ComputeInnerArea();

        return _area.ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }

    private List<Instruction> ParseInput()
    {
        var instructions = new List<Instruction>();
        
        foreach (var line in inputLines)
        {
            instructions.Add(Instruction.Parse(line));
        }

        return instructions;
    }

    private Coordinates RunInstruction(Instruction instruction, Coordinates fromPosition)
    {
        var currentPosition = fromPosition;

        currentPosition = currentPosition.ChangeBy(instruction.Direction, instruction.Length);

        Cube cube = new Cube
        {
            Coordinates = currentPosition,
            Color = instruction.Color,
        };

        _area += instruction.Length;

        _cubes.Add(cube);

        return currentPosition;
    }

    private double ComputeInnerArea()
    {
        var sum = 0;
        _cubes.Add(_cubes[0]);

        for (int i = 0; i < _cubes.Count - 1; i++)
        {
            sum += (_cubes[i].Coordinates.RowIndex * _cubes[i+1].Coordinates.ColumnIndex) - (_cubes[i].Coordinates.ColumnIndex * _cubes[i+1].Coordinates.RowIndex);
        }

        return ((-sum - _area) / 2.0) + 1;
    }

    private record Instruction
    {
        public Direction Direction { get; init; }
        public int Length { get; init; }
        public Color Color { get; init; }

        public static Instruction Parse(string line)
        {
            var parts = line.Split(' ');

            return new Instruction
            {
                Direction = DirectionHelpers.FromString(parts[0]),
                Length = int.Parse(parts[1]),
                Color = ColorTranslator.FromHtml(parts[2].Trim(['(', ')'])),
            };
        }
    }

    private record Cube
    {
        public Coordinates Coordinates { get; init; }
        public Color Color { get; init; }
    }
}
