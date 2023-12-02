namespace AdventOfCode2023;

public class Day2 : Solution
{
    protected override int DayNumber {get; init;} = 2;

    protected override string LogicPart1()
    {
        var possibleGames = new int[_inputLines.Length];
        var maxNumbers = new Dictionary<string, int> (){
            {"red", 12 },
            {"green", 13},
            {"blue", 14}
        };

        for (int i = 0; i < _inputLines.Length; i++)
        {
            string line = _inputLines[i];
            var split = line.Split(':');
            var game = split[0];
            var gameId = game.Split(' ')[1];
            string[] sets = split[1].Split(';').Select(x => x.Trim()).ToArray();
            var gamePossible = true;

            foreach (var set in sets)
            {
                string[] subsets = set.Split(',').Select(x => x.Trim()).ToArray();

                foreach (var subset in subsets)
                {
                    var splitSubset = subset.Split(' ');
                    var number = int.Parse(splitSubset[0].ToString());
                    var color = splitSubset[1];

                    if (number > maxNumbers[color])
                    {
                        gamePossible = false;
                        break;
                    }
                }
                
                if (!gamePossible)
                {
                    break;
                }
            }

            if (gamePossible)
            {
                possibleGames[i] = int.Parse(gameId);
            }
        }

        return possibleGames.Sum().ToString();
    }

    protected override string LoginPart2()
    {
        throw new NotImplementedException();
    }
}
