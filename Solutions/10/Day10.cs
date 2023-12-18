using AdventOfCode2023.Common;
using AdventOfCode2023.Day10Solution;

namespace AdventOfCode2023;

public class Day10 : Solution
{
    private Loop? _loop;
    protected override string LogicPart1()
    {
        var startingPipe = FindStartingPipe();
        _loop = new Loop(startingPipe);

        Pipe? nextPipe;
        var previousPipe = startingPipe;
        var direction = Direction.None;
        while (!_loop.IsComplete)
        {
            nextPipe = FindNextPipe(previousPipe, direction);

            if (nextPipe == null || nextPipe == previousPipe)
            {
                throw new Exception("Could not find continuation");
            }

            _loop.Add(nextPipe);
            direction = GridHelpers.DirectionBetween(previousPipe, nextPipe);
            previousPipe = nextPipe;
        }

        return _loop.DistanceToFarthestPoint().ToString();
    }

    protected override string LogicPart2()
    {
        if (_loop is null || !_loop.IsComplete) throw new InvalidOperationException("Loop not created or incomplete");
        
        return _loop.TilesInside().ToString();
    }

    private Pipe FindStartingPipe()
    {
        for (int i = 0; i < inputLines.Length; i++)
        {
            var line = inputLines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == 'S')
                {
                    return new Pipe(
                        'S',
                        new Coordinates(j, i));
                }
            }
        }

        throw new Exception("Could not find S");
    }

    private Pipe? FindNextPipe(Pipe point, Direction lastDirection)
    {
        if (point.Coordinates.Y > 0 && lastDirection != Direction.Down)
        {
            if (AllowedContinuations.Up(point.Shape).Contains(inputLines[point.Coordinates.Y - 1][point.Coordinates.X]))
            {
                return new Pipe(
                    inputLines[point.Coordinates.Y - 1][point.Coordinates.X],
                    new Coordinates(
                        point.Coordinates.X,
                        point.Coordinates.Y - 1));
            }
        }

        if (point.Coordinates.X < inputLines[0].Length - 1 && lastDirection != Direction.Left)
        {
            if (AllowedContinuations.Right(point.Shape).Contains(inputLines[point.Coordinates.Y][point.Coordinates.X + 1]))
            {
                return new Pipe(
                    inputLines[point.Coordinates.Y][point.Coordinates.X + 1],
                    new Coordinates(
                        point.Coordinates.X + 1,
                        point.Coordinates.Y));
            }
        }

        if (point.Coordinates.X > 0 && lastDirection != Direction.Right)
        {
            if (AllowedContinuations.Left(point.Shape).Contains(inputLines[point.Coordinates.Y][point.Coordinates.X - 1]))
            {
                return new Pipe(
                    inputLines[point.Coordinates.Y][point.Coordinates.X - 1],
                    new Coordinates(
                        point.Coordinates.X - 1,
                        point.Coordinates.Y));
            }
        }

        if (point.Coordinates.Y < inputLines.Length - 1 && lastDirection != Direction.Up)
        {
            if (AllowedContinuations.Down(point.Shape).Contains(inputLines[point.Coordinates.Y + 1][point.Coordinates.X]))
            {
                return new Pipe(
                    inputLines[point.Coordinates.Y + 1][point.Coordinates.X],
                    new Coordinates(
                        point.Coordinates.X,
                        point.Coordinates.Y + 1));
            }
        }

        return null;
    }
}
