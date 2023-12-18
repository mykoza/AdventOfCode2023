namespace AdventOfCode2023.Day10Solution;

public record Pipe
{
    public char Shape { get; init; }
    public Coordinates Coordinates { get; init; }

    public Pipe(char character, Coordinates coordinates)
    {
        Shape = character;
        Coordinates = coordinates;
    }

    public Pipe(char character, int x, int y)
    {
        Shape = character;
        Coordinates = new Coordinates(x, y);
    }
};

public record Coordinates(int X, int Y);
