namespace AdventOfCode2023.Common;

public readonly struct Coordinates : IEquatable<Coordinates>
{
    public int RowIndex { get; init; }
    public int ColumnIndex { get; init; }

    public Coordinates()
    {
        RowIndex = 0;
        ColumnIndex = 0;
    }

    public Coordinates(int rowIndex, int columnIndex)
    {
        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    public bool Equals(Coordinates other)
    {
        return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coordinates coordinates && Equals(coordinates);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RowIndex, ColumnIndex);
    }

    public void Deconstruct(out int rowIndex, out int columnIndex)
    {
        rowIndex = RowIndex;
        columnIndex = ColumnIndex;
    }

    public static implicit operator (int rowIndex, int columnIndex)(Coordinates value)
    {
        return (value.RowIndex, value.ColumnIndex);
    }

    public static implicit operator Coordinates((int rowIndex, int columnIndex) value)
    {
        return new Coordinates(value.rowIndex, value.columnIndex);
    }

    public static bool operator ==(Coordinates left, Coordinates right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinates left, Coordinates right)
    {
        return !(left == right);
    }

    public static Coordinates operator +(Coordinates left, Coordinates right)
    {
        return new Coordinates(left.RowIndex + right.RowIndex, left.ColumnIndex + right.ColumnIndex);
    }

    public static Coordinates operator -(Coordinates left, Coordinates right)
    {
        return new Coordinates(left.RowIndex - right.RowIndex, left.ColumnIndex - right.ColumnIndex);
    }
}