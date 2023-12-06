//var lines = File.ReadAllLines("Example.txt");
var lines = File.ReadAllLines("Input.txt");

var times = lines[0].Split(' ').Skip(1).Where(c => !string.IsNullOrEmpty(c)).Select(long.Parse).ToList();
var distances = lines[1].Split(' ').Skip(1).Where(c => !string.IsNullOrEmpty(c)).Select(long.Parse).ToList();

long combination = 1;
for (var i = 0; i < times.Count; i++)
{
    combination *= PossibleWin(times[i], distances[i]);
}
Console.WriteLine($"[1] Total combination: {combination}");

var singleTime = long.Parse(string.Join("", times));
var singleDistance = long.Parse(string.Join("", distances));
Console.WriteLine($"[2] Total combination: {PossibleWin(singleTime, singleDistance)}");
return;


long PossibleWin(long timeLimit, long distanceLimit)
{
    long ways = 0;
    for (long holdTime = 1; holdTime < timeLimit; holdTime++)
    {
        if (holdTime * (timeLimit - holdTime) > distanceLimit)
        {
            ways++;
        }
    }
    return ways;
}

