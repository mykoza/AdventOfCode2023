namespace AdventOfCode2023;

public interface ICard
{
    int Value { get; }
}

public class CardPart1 : ICard
{
    private readonly int _value;

    public CardPart1(string card)
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

public class CardPart2 : ICard
{
    private readonly int _value;

    public CardPart2(string card)
    {
        _value = card switch
        {
            "J" => 1,
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            "7" => 7,
            "8" => 8,
            "9" => 9,
            "T" => 10,
            "Q" => 12,
            "K" => 13,
            "A" => 14,
            _ => throw new ArgumentOutOfRangeException($"{card} is not a valid card"),
        };
    }

    public int Value => _value;
}
