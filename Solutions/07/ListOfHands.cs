namespace AdventOfCode2023;

public class ListOfHands
{
    private readonly List<Hand> _hands = [];
    private readonly int _count;

    public ListOfHands(List<Hand> hands)
    {
        _hands = hands;
        _hands.Sort();
        _hands.Reverse();
        _count = _hands.Count;
    }

    public int Value()
    {
        return _hands
            .Select((hand, i) => hand.Bid * (_count - i))
            .Sum();
    }

    public static ListOfHands Parse(List<string> input, int partNum)
    {
        var hands = new List<Hand>();

        foreach (var line in input)
        {
            var split = line.Split(' ');
            var cardsString = split[0];
            var bid = int.Parse(split[1]);
            var cards = cardsString
                .Select(symbol => partNum == 1 ? (ICard)new CardPart1(symbol.ToString()) : (ICard)new CardPart2(symbol.ToString()))
                .ToList();


            hands.Add(new Hand(cards, bid, partNum));
        }

        return new ListOfHands(hands);
    }
}
