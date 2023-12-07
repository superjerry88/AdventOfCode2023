var lines = File.ReadAllLines("Input.txt");
var inputs = lines.Select(line => new Hand(line)).ToList();

Console.WriteLine("[Part 1] Total Winnings: " + CalculateTotalWInning(inputs,false));
Console.WriteLine("[Part 2] Total Winnings: " + CalculateTotalWInning(inputs,true));
return;

int CalculateTotalWInning(List<Hand> hands, bool useJokerRule)
{
    foreach (var hand in hands)
    {
        hand.JokerRule = useJokerRule;
        hand.CalculateHandType();
    }

    //Weakest type first
    //Then by card value
    var sortedHands = hands.OrderBy(hand => hand.Type)
        .ThenBy(hand => hand.SortedIndex())
        .ToList();

    var multiplier = 1;
    var totalWinnings = sortedHands.Sum(hand => hand.GetWinning(multiplier++));
    return totalWinnings;
}

public enum HandType
{
    HighCard = 0,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

public class Hand
{
    private static readonly string NormalOrder = "23456789TJQKA";
    private static readonly string JokerOrder = "J23456789TQKA";

    public string CardValue { get; private set; }
    public int Bid { get; private set; }
    public HandType Type { get; private set; }
    public bool JokerRule { get; set; } = false;

    public Hand(string line)
    {
        var parts = line.Split(' ');
        CardValue = parts[0];
        Bid = int.Parse(parts[1]);
    }

    public int GetWinning(int multiplier)
    {
        return Bid * multiplier;
    }

    private int GetCardValue(char card)
    {
        if (JokerRule && card == 'J') return -1;
        return NormalOrder.IndexOf(card);
    }

    public void CalculateHandType()
    {
        var bestHandType = DetermineHandType(CardValue);

        if (JokerRule && CardValue.Contains('J'))
        {
            foreach (var card in JokerOrder.Skip(1))
            {
                var replacedHand = DetermineHandType(CardValue.Replace('J', card));
                if (replacedHand > bestHandType)
                {
                    bestHandType = replacedHand;
                }
            }
        }

        Type = bestHandType;
    }

    private HandType DetermineHandType(string handValue)
    {
        var grouping = handValue.GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .ThenByDescending(g => GetCardValue(g.Key))
            .ToList();

        switch (grouping[0].Count())
        {
            case 5:
                return HandType.FiveOfAKind;
            case 4:
                return HandType.FourOfAKind;
            case 3:
                return grouping[1].Count() == 2 ? HandType.FullHouse : HandType.ThreeOfAKind;
            case 2:
                return grouping[1].Count() == 2 ? HandType.TwoPair : HandType.OnePair;
            default:
                return HandType.HighCard;
        }
    }

    public string SortedIndex()
    {
        var order = JokerRule ? JokerOrder : NormalOrder;
        return string.Concat(CardValue.Select(c => (char)('A' + order.IndexOf(c))));
    }

}