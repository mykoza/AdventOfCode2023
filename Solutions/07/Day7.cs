namespace AdventOfCode2023;

public class Day7 : Solution
{
    protected override int DayNumber { get; init; } = 7;

    protected override string LogicPart1()
    {
        var listOfHands = ListOfHands.Parse([.. _inputLines]);

        return listOfHands.Value().ToString();
    }

    protected override string LogicPart2()
    {
        throw new NotImplementedException();
    }
}

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

    public static ListOfHands Parse(List<string> input)
    {
        var hands = new List<Hand>();

        foreach (var line in input)
        {
            var split = line.Split(' ');
            var cardsString = split[0];
            var bid = int.Parse(split[1]);
            var cards = cardsString
                .Select(symbol => new Card(symbol.ToString()))
                .ToList();


            hands.Add(new Hand(cards, bid));
        }

        return new ListOfHands(hands);
    }
}

public class Hand : IComparable<Hand>
{
    private readonly int _bid;
    private readonly List<Card> _cards = [];
    private readonly HandType _type;

    public int Bid => _bid;

    public HandType Type => _type;

    public Hand(List<Card> cards, int bid)
    {
        _bid = bid;
        _cards = cards;
        _type = FindHandType(cards);
    }

    private static HandType FindHandType(List<Card> cards)
    {
        var counts = cards
            .GroupBy((card) => card.Value)
            .ToList();
            // .Select(grouping => new { Key = grouping.Key, Count = grouping.Count()})
            // .OrderByDescending(x => x.Count)
            // .ToList();

        if (counts.Exists(grouping => grouping.Count() == 5))
        {
            return HandType.FiveOfAKind;
        }
        else if (counts.Exists(grouping => grouping.Count() == 4))
        {
            return HandType.FourOfAKind;
        }
        else if (counts.Exists(grouping => grouping.Count() == 3) && counts.Exists(grouping => grouping.Count() == 2))
        {
            return HandType.FullHouse;
        }
        else if (counts.Exists(grouping => grouping.Count() == 3))
        {
            return HandType.ThreeOfAKind;
        }
        else if (counts.FindAll(grouping => grouping.Count() == 2).Count == 2)
        {
            return HandType.TwoPair;
        }
        else if (counts.Exists(grouping => grouping.Count() == 2))
        {
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }

    public int CompareTo(Hand? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (_type > other._type)
        {
            return 1;
        }
        else if (_type < other._type)
        {
            return -1;
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            if (_cards[i].Value > other._cards[i].Value)
            {
                return 1;
            }
            else if (_cards[i].Value < other._cards[i].Value)
            {
                return -1;
            }
        }

        return 0;
    }
}

public class Card
{
    private readonly int _value;

    public Card(string card)
    {
        _value = card switch
        {
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            "7" => 7,
            "8" => 8,
            "9" => 9,
            "T" => 10,
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => 14,
            _ => throw new ArgumentOutOfRangeException($"{card} is not a valid card"),
        };
    }

    public int Value => _value;
}

public enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}
