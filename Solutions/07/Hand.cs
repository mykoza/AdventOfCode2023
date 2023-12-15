namespace AdventOfCode2023;

public class Hand : IComparable<Hand>
{
    private readonly int _bid;
    private readonly List<ICard> _cards = [];
    private readonly HandType _type;

    public int Bid => _bid;

    public HandType Type => _type;

    public Hand(List<ICard> cards, int bid, int partNum)
    {
        _bid = bid;
        _cards = cards;
        _type = FindHandType(cards, partNum);
    }

    private static HandType FindHandType(List<ICard> cards, int partNum)
    {
        if (partNum == 1)
        {
            return Part1TypeFinder(cards);
        }

        return Part2TypeFinder(cards);
    }

    private static HandType Part1TypeFinder(List<ICard> cards)
    {
        var counts = cards
            .GroupBy((card) => card.Value)
            .ToList();

        if (counts.Exists(grouping => grouping.Count() == 5))
        {
            return HandType.FiveOfAKind;
        }
        else if (counts.Exists(grouping => grouping.Count() == 4))
        {
            return HandType.FourOfAKind;
        }
        else if (counts.Exists(grouping => grouping.Count() == 3))
        {
            if (counts.Exists(grouping => grouping.Count() == 2))
            {
                return HandType.FullHouse;
            }

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

    private static HandType Part2TypeFinder(List<ICard> cards)
    {
        if (!cards.Exists(card => card.Value == 1) || cards.FindAll(card => card.Value == 1).Count == 5)
        {
            return Part1TypeFinder(cards);
        }

        var counts = cards
            .GroupBy((card) => card.Value)
            .OrderByDescending(grouping => grouping.Count());

        var highestCardCountOtherThanJoker = counts
            .First(grouping => grouping.Key != 1)
            .Count();

        var numberOfJokers = counts
            .First(card => card.Key == 1)
            .Count();

        if (highestCardCountOtherThanJoker == 4)
        {
            return HandType.FiveOfAKind;
        }
        else if (highestCardCountOtherThanJoker == 3)
        {
            if (numberOfJokers == 1)
            {
                return HandType.FourOfAKind;
            }

            return HandType.FiveOfAKind;
        }
        else if (highestCardCountOtherThanJoker == 2)
        {
            if (numberOfJokers == 3)
            {
                return HandType.FiveOfAKind;
            }
            else if (numberOfJokers == 2)
            {
                return HandType.FourOfAKind;
            }
            else if (counts.Where(grouping => grouping.Count() == 2).Count() == 2)
            {
                return HandType.FullHouse;
            }

            return HandType.ThreeOfAKind;
        }
        else if (highestCardCountOtherThanJoker == 1)
        {
            if (numberOfJokers == 4)
            {
                return HandType.FiveOfAKind;
            }
            else if (numberOfJokers == 3)
            {
                return HandType.FourOfAKind;
            }
            else if (numberOfJokers == 2)
            {
                return HandType.ThreeOfAKind;
            }
        }
        
        return HandType.OnePair;
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
