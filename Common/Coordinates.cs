namespace AdventOfCode2023.Common;

public readonly struct Coordinates
{
    public readonly int rowIndex;
    public readonly int columnIndex;

    public Coordinates()
    {
        rowIndex = 0;
        columnIndex = 0;
    }

    public Coordinates(int rowIndex, int columnIndex)
    {
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coordinates other &&
               rowIndex == other.rowIndex &&
               columnIndex == other.columnIndex;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(rowIndex, columnIndex);
    }

    public void Deconstruct(out int rowIndex, out int columnIndex)
    {
        rowIndex = this.rowIndex;
        columnIndex = this.columnIndex;
    }

    public static implicit operator (int rowIndex, int columnIndex)(Coordinates value)
    {
        return (value.rowIndex, value.columnIndex);
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
}