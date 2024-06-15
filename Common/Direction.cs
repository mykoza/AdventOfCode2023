namespace AdventOfCode2023.Common;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public static class DirectionHelpers
{
    public static bool IsOppositeTo(this Direction direction, Direction other)
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

    public static Direction FromChangeInCoordinates(this Direction direction, Coordinates coordinates)
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
    
    public static Direction FromString(string str)
    {
        return str switch
        {
            "U" => Direction.Up,
            "D" => Direction.Down,
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => Direction.None
        };
    }
}