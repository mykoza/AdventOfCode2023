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