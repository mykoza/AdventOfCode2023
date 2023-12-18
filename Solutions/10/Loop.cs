namespace AdventOfCode2023.Day10Solution;

public class Loop
{
    private readonly List<Pipe> _pipes = [];
    private bool _isComplete = false;

    public Loop(Pipe startingPipe)
    {
        _pipes.Add(startingPipe);
    }

    public bool IsComplete => _isComplete;

    public void Add(Pipe point)
    {
        if (point.Shape == 'S')
        {
            _isComplete = true;
            return;
        }

        _pipes.Add(point);
    }

    public int DistanceToFarthestPoint()
    {
        return (int)Math.Ceiling((decimal)_pipes.Count / 2);
    }

    public int TilesInside()
    {
        ChangeStartingPointToCorrectPipe();

        var pointsByLine = _pipes
            .GroupBy(character => character.Coordinates.Y)
            .OrderBy(grouping => grouping.Key)
            .Select(grouping => grouping
                .OrderBy(point => point.Coordinates.X)
                .Where(point => point.Shape != '-')
                .ToList())
            .ToList();

        var pointsInside = 0;

        for (int i = 0; i < pointsByLine.Count; i++)
        {
            List<Pipe>? points = pointsByLine[i];

            var inside = false;
            var insideSince = -1;
            for (int j = 0; j < points.Count; j++)
            {
                if (points[j].Shape == '|')
                {
                    inside = !inside;

                    if (inside)
                    {
                        insideSince = points[j].Coordinates.X;
                    }
                    else
                    {
                        pointsInside += points[j].Coordinates.X - insideSince - 1;
                    }
                }
                else if (
                    (points[j].Shape == 'F' && points[j + 1].Shape == 'J') ||
                    (points[j].Shape == 'L' && points[j + 1].Shape == '7')
                )
                {
                    inside = !inside;

                    if (inside)
                    {
                        insideSince = points[j + 1].Coordinates.X;
                    }                    
                    else
                    {
                        pointsInside += points[j].Coordinates.X - insideSince - 1;
                    }

                    j++;
                }
                else if (
                    (points[j].Shape == 'F' && points[j + 1].Shape == '7') ||
                    (points[j].Shape == 'L' && points[j + 1].Shape == 'J')
                )
                {
                    if (inside)
                    {
                        pointsInside -= points[j + 1].Coordinates.X - points[j].Coordinates.X + 1;
                    }

                    j++;
                }
                else
                {
                    throw new Exception("Invalid character");
                }
            }
        }

        return pointsInside;
    }

    private void ChangeStartingPointToCorrectPipe()
    {
        var directionStartToFirst = GridHelpers.DirectionBetween(_pipes[0], _pipes[1]);
        var directionStartToLast = GridHelpers.DirectionBetween(_pipes[0], _pipes[^1]);

        var correctPipe = (directionStartToFirst, directionStartToLast) switch
        {
            (Direction.Up, Direction.Right) => 'L',
            (Direction.Up, Direction.Left) => 'J',
            (Direction.Up, Direction.Down) => '|',
            (Direction.Right, Direction.Left) => '-',
            (Direction.Right, Direction.Down) => 'F',
            (Direction.Left, Direction.Down) => '7',
            _ => throw new Exception("Could not find correct pipe"),
        };

        _pipes[0] = _pipes[0] with { Shape = correctPipe };
    }
}
