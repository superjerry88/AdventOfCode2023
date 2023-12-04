var lines = File.ReadAllLines("Input.txt");
var games = lines.Select(line => new Game(line)).ToList();


Console.WriteLine(games.Sum(game => game.Score));
Console.WriteLine(CalculateTotalCards(games));

return;

int CalculateTotalCards(List<Game> originalGame)
{
    var totalCards = originalGame.Count;
    var todoCards = new Queue<Game>(originalGame);
    while (todoCards.Count > 0)
    {
        var game = todoCards.Dequeue();
        var wonResult = game.Wins;
        for (var nextCard = 1; nextCard <= wonResult; nextCard++)
        {
            var nextCardIndex = game.CardNumber -1 + nextCard;
            if (nextCardIndex < originalGame.Count)
            {
                todoCards.Enqueue(originalGame[nextCardIndex]);
                totalCards++;
            }
        }
    }
    return totalCards;
}


public class Game
{
    public List<int> WinningNumbers { get; set; }
    public List<int> PlayerNumbers { get; set; }
    public int CardNumber { get; set; }
    public int Score { get; set; }
    public int Wins { get; set; }

    public Game(string input)
    {
        var parts = input.Split(':');
        var sets = parts[1].Split('|');
        CardNumber = int.Parse(parts[0].Replace("Card ","")) ;
        PlayerNumbers = sets[0].Trim().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList();
        WinningNumbers = sets[1].Trim().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList();
        Score = CalculateScore();
        Wins = CalculateWin();
    }

    public int CalculateScore()
    {
        return PlayerNumbers.Where(playerNumber => WinningNumbers.Contains(playerNumber)).Aggregate(0, (current, playerNumber) => current == 0 ? 1 : current * 2);
    }

    public int CalculateWin()
    {
        return PlayerNumbers.Count(playerNumber => WinningNumbers.Contains(playerNumber));
    }
}