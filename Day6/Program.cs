//var lines = File.ReadAllLines("Example.txt");

using System.Diagnostics;

var lines = File.ReadAllLines("Input.txt");

var times = lines[0].Split(' ').Skip(1).Where(c => !string.IsNullOrEmpty(c)).Select(long.Parse).ToList();
var distances = lines[1].Split(' ').Skip(1).Where(c => !string.IsNullOrEmpty(c)).Select(long.Parse).ToList();

long combination = 1;
for (var i = 0; i < times.Count; i++)
{
    combination *= PossibleWin2(times[i], distances[i]);
}
Console.WriteLine($"[1] Total combination: {combination}");

var singleTime = long.Parse(string.Join("", times));
var singleDistance = long.Parse(string.Join("", distances));
Console.WriteLine($"[2] Total combination: {PossibleWin2(singleTime, singleDistance)}");
return;


long PossibleWin(long timeLimit, long distanceLimit)
{
    long ways = 0;
    for (var holdTime = distanceLimit / timeLimit; holdTime < timeLimit; holdTime++)
    {
        if (holdTime * (timeLimit - holdTime) > distanceLimit)
        {
            ways++;
        }
        
    }
    return ways;
}

long PossibleWin2(long timeLimit, long distanceLimit)
{
    var discriminant = Math.Sqrt(timeLimit * timeLimit - 4 * distanceLimit);

    var root1 = (timeLimit - discriminant) / 2;
    var root2 = (timeLimit + discriminant) / 2;

    var ways = (long)Math.Floor(root2) - (long)Math.Ceiling(root1) + 1;
    return ways;
}

