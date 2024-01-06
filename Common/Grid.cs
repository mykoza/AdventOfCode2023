using System.Collections.Immutable;

namespace AdventOfCode2023;

public class Grid<T>
{
    private readonly List<List<T>> _grid = [];

    public int Width { get; init; }
    public int Height { get; init; }
    public ImmutableList<ImmutableList<T>> Rows {
        get {
            return _grid.Select(row => ImmutableList.CreateRange(row)).ToImmutableList();
        }
    }

    public Grid(ICollection<ICollection<T>> input)
    {
        Height = input.Count;
        Width = input.Count > 0 ? input.First().Count : 0;
        _grid = input.Select(row => new List<T>(row)).ToList();
    }

    public T Value(int rowIndex, int columnIndex)
    {
        return _grid[rowIndex][columnIndex];
    }

    public List<T> Row(int index)
    {
        return _grid[index];
    }

    public List<T> Column(int index)
    {
        List<T> res = [];

        foreach (var row in _grid)
        {
            res.Add(row[index]);
        }

        return res;
    }
}
