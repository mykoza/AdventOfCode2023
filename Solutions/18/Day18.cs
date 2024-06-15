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
    }

    protected override string LogicPart1()
    {
        _instructions = ParseInputPart1();

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
        _area = 0;
        _cubes.Clear();
        _instructions.Clear();
        _instructions = ParseInputPart2();

        Coordinates currentPosition = (0, 0);

        foreach (var instruction in _instructions)
        {
            currentPosition = RunInstruction(instruction, currentPosition);
        }

        _area += ComputeInnerArea();

        return _area.ToString();
    }

    private List<Instruction> ParseInputPart1()
    {
        var instructions = new List<Instruction>();
        
        foreach (var line in inputLines)
        {
            instructions.Add(Instruction.ParsePart1(line));
        }

        return instructions;
    }

    private List<Instruction> ParseInputPart2()
    {
        var instructions = new List<Instruction>();

        foreach (var line in inputLines)
        {
            instructions.Add(Instruction.ParsePart2(line));
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
        long sum = 0;
        _cubes.Add(_cubes[0]);

        for (int i = 0; i < _cubes.Count - 1; i++)
        {
            long rowIndex = _cubes[i].Coordinates.RowIndex;
            long columnIndex = _cubes[i + 1].Coordinates.ColumnIndex;
            long columnIndex1 = _cubes[i].Coordinates.ColumnIndex;
            long rowIndex1 = _cubes[i + 1].Coordinates.RowIndex;
            sum += rowIndex * columnIndex - columnIndex1 * rowIndex1;
        }

        return ((-sum - _area) / 2.0) + 1;
    }

    private record Instruction
    {
        public Direction Direction { get; init; }
        public int Length { get; init; }
        public Color Color { get; init; }

        public static Instruction ParsePart1(string line)
        {
            var parts = line.Split(' ');

            return new Instruction
            {
                Direction = DirectionHelpers.FromString(parts[0]),
                Length = int.Parse(parts[1]),
                Color = ColorTranslator.FromHtml(parts[2].Trim(['(', ')'])),
            };
        }

        public static Instruction ParsePart2(string line)
        {
            var parts = line.Split(' ');

            var hex = parts[2].Trim('(', ')')[1..];
            var length = int.Parse(hex[0..5], System.Globalization.NumberStyles.HexNumber);
            var direction = hex[^1] switch {
                '0' => Direction.Right,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '3' => Direction.Up,
                _ => throw new Exception("Invalid direction")
            };

            return new Instruction
            {
                Direction = direction,
                Length = length,
                Color = Color.Black,
            };
        }
    }

    private record Cube
    {
        public Coordinates Coordinates { get; init; }
        public Color Color { get; init; }
    }
}
