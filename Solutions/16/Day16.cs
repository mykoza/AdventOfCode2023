using AdventOfCode2023.Common;

namespace AdventOfCode2023;

public class Day16 : Solution
{
    private readonly List<Beam> _beams = [];
    private readonly Dictionary<Coordinates, int> _energizedTiles = [];
    private readonly Dictionary<(Coordinates, Direction), int> _beamHistory = [];

    protected override string LogicPart1()
    {
        RunBeam(Direction.Right, (0, 0));

        return _energizedTiles.Count.ToString();
    }

    protected override string LogicPart2()
    {
        int width = inputLines[0].Length;
        int height = inputLines.Length;

        int maxEnergizedTiles = 0;

        // row 0
        for (int i = 0; i < width; i++)
        {
            RunBeam(Direction.Down, (0, i));

            maxEnergizedTiles = Math.Max(maxEnergizedTiles, _energizedTiles.Count);
        }

        // last row
        for (int i = 0; i < width; i++)
        {
            RunBeam(Direction.Up, (height - 1, i));

            maxEnergizedTiles = Math.Max(maxEnergizedTiles, _energizedTiles.Count);
        }

        // column 0
        for (int i = 0; i < height; i++)
        {
            RunBeam(Direction.Right, (i, 0));

            maxEnergizedTiles = Math.Max(maxEnergizedTiles, _energizedTiles.Count);
        }

        // last column
        for (int i = 0; i < height; i++)
        {
            RunBeam(Direction.Left, (i, width - 1));

            maxEnergizedTiles = Math.Max(maxEnergizedTiles, _energizedTiles.Count);
        }

        return maxEnergizedTiles.ToString();
    }

    private void RunBeam(Direction direction, Coordinates coordinates)
    {
        _beams.Clear();
        _beamHistory.Clear();
        _energizedTiles.Clear();

        _beams.Add(new Beam(direction, coordinates));

        _energizedTiles.Add(_beams[0].Coordinates, 1);

        while (MoveBeams())
        {
            _ = _beams.RemoveAll(x => CoordinatesOutOfBound(x.Coordinates));
        }
    }

    private bool MoveBeams()
    {
        var res = false;

        var cnt = _beams.Count;
        for (int i = 0; i < cnt; i++)
        {
            Beam beam = _beams[i];
            if (!_energizedTiles.TryAdd(beam.Coordinates, 1))
            {
                _energizedTiles[beam.Coordinates] += 1;
            }

            var beamMoved = MoveBeam(beam);

            if (beamMoved)
            {
                if (!_beamHistory.TryAdd((beam.Coordinates, beam.Direction), 1))
                {
                    beam.Coordinates = (-1, -1);
                }

                res = true;
            }
        }

        return res;
    }

    private bool MoveBeam(Beam beam)
    {
        if (CoordinatesOutOfBound(beam.Coordinates))
        {
            return false;
        }

        char c = inputLines[beam.Coordinates.rowIndex][beam.Coordinates.columnIndex];
        if (c == '.')
        {
            ChangePosition(beam);
            return true;
        }
        else if (c == '|')
        {
            if (beam.Direction == Direction.Up || beam.Direction == Direction.Down)
            {
                ChangePosition(beam);
                return true;
            }
            else
            {
                Beam newBeam = new(Direction.Up, beam.Coordinates);
                ChangePosition(newBeam);
                _beams.Add(newBeam);

                beam.Direction = Direction.Down;
                ChangePosition(beam);
                return true;
            }
        }
        else if (c == '-')
        {
            if (beam.Direction == Direction.Left || beam.Direction == Direction.Right)
            {
                ChangePosition(beam);
                return true;
            }
            else
            {
                Beam newBeam = new Beam(Direction.Left, beam.Coordinates);
                ChangePosition(newBeam);
                _beams.Add(newBeam);

                beam.Direction = Direction.Right;
                ChangePosition(beam);
                return true;
            }
        }
        else if (c == '/' || c == '\\')
        {
            ChangeDirection(beam, c);
            ChangePosition(beam);
            return true;
        }
        else
        {
            throw new Exception("Invalid character");
        }
    }

    private static void ChangePosition(Beam beam)
    {
        switch (beam.Direction)
        {
            case Direction.Up:
                beam.Coordinates = (beam.Coordinates.rowIndex - 1, beam.Coordinates.columnIndex);
                break;
            case Direction.Down:
                beam.Coordinates = (beam.Coordinates.rowIndex + 1, beam.Coordinates.columnIndex);
                break;
            case Direction.Left:
                beam.Coordinates = (beam.Coordinates.rowIndex, beam.Coordinates.columnIndex - 1);
                break;
            case Direction.Right:
                beam.Coordinates = (beam.Coordinates.rowIndex, beam.Coordinates.columnIndex + 1);
                break;
            case Direction.None:
                throw new Exception("Cannot move");
        };
    }

    private static void ChangeDirection(Beam beam, char mirror)
    {
        if (mirror == '/')
        {
            beam.Direction = beam.Direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                _ => throw new Exception("Cannot move"),
            };
        }
        else if (mirror == '\\')
        {
            beam.Direction = beam.Direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new Exception("Cannot move"),
            };
        }
        else
        {
            return;
        }
    }

    private bool CoordinatesOutOfBound(Coordinates coordinates)
    {
        return coordinates.rowIndex < 0
            || coordinates.columnIndex < 0
            || coordinates.rowIndex >= inputLines.Length
            || coordinates.columnIndex >= inputLines[0].Length;
    }

    private record Beam
    {
        public Direction Direction { get; set; }
        public Coordinates Coordinates { get; set; }

        public Beam(Direction direction, Coordinates coordinates)
        {
            Direction = direction;
            Coordinates = coordinates;
        }
    }
}
