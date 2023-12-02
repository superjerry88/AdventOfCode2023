var lines = File.ReadAllLines("Input.txt");
var games = lines.Select(line => new Game(line)).ToList();

//Part 1
Console.WriteLine(games.Where(c => c.IsLegal()).Sum(c=>c.Id));

//Part 2 
Console.WriteLine(games.Sum(c=>c.GetPower()));

return;

class Game
{
    public int Id;
    public List<Set> Sets = new();

    public Game(string input)
    {
        Id = int.Parse(input.Split(' ')[1].Replace(":", ""));
        var groups = input.Split(':')[1].Trim().Split(';');

        foreach (var text in groups)
        {
            var set = new Set();
            var cubeCounts = text.Trim().Split(',');

            foreach (var cubeCount in cubeCounts)
            {
                var parts = cubeCount.Trim().Split(' ');
                var count = int.Parse(parts[0]);
                var color = parts[1];

                switch (color)
                {
                    case "red":
                        set.Red = count;
                        break;
                    case "green":
                        set.Green = count;
                        break;
                    case "blue":
                        set.Blue = count;
                        break;
                }
            }

            Sets.Add(set);
        }
    }

    public bool IsLegal()
    {
        return Sets.All(bag => bag is { Red: <= 12, Green: <= 13, Blue: <= 14 });
    }

    public int GetPower()
    {
        return Sets.Max(c => c.Red) * Sets.Max(c => c.Green) * Sets.Max(c => c.Blue);
    }
}

class Set
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
}
