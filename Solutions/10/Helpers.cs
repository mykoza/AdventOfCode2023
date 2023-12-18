namespace AdventOfCode2023.Day10Solution;

public static class GridHelpers
{
    public static Direction DirectionBetween(Pipe previous, Pipe next)
    {
        var (X, Y) = CoordinatesDifferenceBetween(previous, next);

        return (X, Y) switch
        {
            ( > 0, _) => Direction.Right,
            ( < 0, _) => Direction.Left,
            (_, > 0) => Direction.Down,
            _ => Direction.Up
        };
    }

    public static (int X, int Y) CoordinatesDifferenceBetween(Pipe previous, Pipe current)
    {
        return (
            current.Coordinates.X - previous.Coordinates.X,
            current.Coordinates.Y - previous.Coordinates.Y
        );
    }
}

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

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}
