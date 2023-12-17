using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day10 : Solution
{
    protected override string LogicPart1()
    {
        var pointS = FindCoordinatesOfS(inputLines);
        var loop = new Loop(pointS);

        var loopCompleted = false;
        Point? continuation;
        var lastPoint = pointS;
        var direction = Direction.None;
        while (!loopCompleted)
        {
            continuation = FindContinuation(lastPoint, direction);

            if (continuation == null || continuation == lastPoint)
            {
                throw new Exception("Could not find continuation");
            }

            if (continuation.Character == 'S')
            {
                loopCompleted = true;
            }

            loop.Add(continuation);
            direction = ComputeDirection(lastPoint, continuation);
            lastPoint = continuation;
        }

        return loop.DistanceToFarthestPoint().ToString();
    }

    private Point FindCoordinatesOfS(string[] inputLines)
    {
        for (int i = 0; i < inputLines.Length; i++)
        {
            var line = inputLines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == 'S')
                {
                    return new Point(
                        'S',
                        new Coordinates(j, i));
                }
            }
        }

        throw new Exception("Could not find S");
    }

    private Point? FindContinuation(Point point, Direction lastDirection)
    {
        if (point.Coordinates.Y > 0 && lastDirection != Direction.Down)
        {
            if (AllowedContinuations.Up(point.Character).Contains(inputLines[point.Coordinates.Y - 1][point.Coordinates.X]))
            {
                return new Point(
                    inputLines[point.Coordinates.Y - 1][point.Coordinates.X],
                    new Coordinates(
                        point.Coordinates.X,
                        point.Coordinates.Y - 1));
            }
        }

        if (point.Coordinates.X < inputLines[0].Length - 1 && lastDirection != Direction.Left)
        {
            if (AllowedContinuations.Right(point.Character).Contains(inputLines[point.Coordinates.Y][point.Coordinates.X + 1]))
            {
                return new Point(
                    inputLines[point.Coordinates.Y][point.Coordinates.X + 1],
                    new Coordinates(
                        point.Coordinates.X + 1,
                        point.Coordinates.Y));
            }
        }

        if (point.Coordinates.X > 0 && lastDirection != Direction.Right)
        {
            if (AllowedContinuations.Left(point.Character).Contains(inputLines[point.Coordinates.Y][point.Coordinates.X - 1]))
            {
                return new Point(
                    inputLines[point.Coordinates.Y][point.Coordinates.X - 1],
                    new Coordinates(
                        point.Coordinates.X - 1,
                        point.Coordinates.Y));
            }
        }

        if (point.Coordinates.Y < inputLines.Length - 1 && lastDirection != Direction.Up)
        {
            if (AllowedContinuations.Down(point.Character).Contains(inputLines[point.Coordinates.Y + 1][point.Coordinates.X]))
            {
                return new Point(
                    inputLines[point.Coordinates.Y + 1][point.Coordinates.X],
                    new Coordinates(
                        point.Coordinates.X,
                        point.Coordinates.Y + 1));
            }
        }

        return null;
    }

    private Direction ComputeDirection(Point previous, Point current)
    {
        var (X, Y) = current.Distance(previous);

        if (X > 0)
        {
            return Direction.Right;
        }
        else if (X < 0)
        {
            return Direction.Left;
        }
        else if (Y > 0)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Up;
        }
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }
}

public class Loop
{
    private readonly List<Point> _characters = [];
    private bool _finished = false;

    public Loop(Point startingPoint)
    {
        _characters.Add(startingPoint);
    }

    public void Add(Point point)
    {
        if (point.Character == 'S')
        {
            _finished = true;
            return;
        }

        _characters.Add(point);
    }

    public int DistanceToFarthestPoint()
    {
        return (int)Math.Ceiling((decimal)_characters.Count / 2);
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public record Point(char Character, Coordinates Coordinates)
{
    public (int X, int Y) Distance(Point other)
    {
        return (
            Coordinates.X - other.Coordinates.X,
            Coordinates.Y - other.Coordinates.Y
        );
    }
};

public record Coordinates(int X, int Y);

public static class AllowedContinuations
{
    public static char[] Up(char character)
    {
        return character switch
        {
            'S' => ['|', '7', 'F'],
            '-' => [],
            '|' => ['S', '|', '7', 'F'],
            'L' => ['S', '|', '7', 'F'],
            'J' => ['S', '|', '7', 'F'],
            '7' => [],
            'F' => [],
            _ => throw new Exception("Invalid character"),
        };
    }

    public static char[] Down(char character)
    {
        return character switch
        {
            'S' => ['|', 'L', 'J'],
            '-' => [],
            '|' => ['S', '|', 'L', 'J'],
            'L' => [],
            'J' => [],
            '7' => ['S', '|', 'L', 'J'],
            'F' => ['S', '|', 'L', 'J'],
            _ => throw new Exception("Invalid character"),
        };
    }

    public static char[] Left(char character)
    {
        return character switch
        {
            'S' => ['-', 'L', 'F'],
            '-' => ['S', '-', 'L', 'F'],
            '|' => [],
            'L' => [],
            'J' => ['S', '-', 'L', 'F'],
            '7' => ['S', '-', 'L', 'F'],
            'F' => [],
            _ => throw new Exception("Invalid character"),
        };
    }

    public static char[] Right(char character)
    {
        return character switch
        {
            'S' => ['-', 'J', '7'],
            '-' => ['S', '-', 'J', '7'],
            '|' => [],
            'L' => ['S', '-', 'J', '7'],
            'J' => [],
            '7' => [],
            'F' => ['S', '-', 'J', '7'],
            _ => throw new Exception("Invalid character"),
        };
    }
}