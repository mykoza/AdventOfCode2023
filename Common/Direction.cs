namespace AdventOfCode2023.Common;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public static class DirectionExtensions
{
    public static bool IsOpposite(Direction direction, Direction other)
    {
        return direction switch
        {
            Direction.Up when other == Direction.Down => true,
            Direction.Down when other == Direction.Up => true,
            Direction.Left when other == Direction.Right => true,
            Direction.Right when other == Direction.Left => true,
            _ => false,
        };
    }

    public static Direction FromChangeInCoordinates(Coordinates coordinates)
    {
        if (coordinates.RowIndex < 0)
        {
            return Direction.Up;
        }
        else if (coordinates.RowIndex > 0)
        {
            return Direction.Down;
        }
        else if (coordinates.ColumnIndex < 0)
        {
            return Direction.Left;
        }
        else if (coordinates.ColumnIndex > 0)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.None;
        }
    }
}