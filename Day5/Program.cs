using System.Collections.Concurrent;

var lines = File.ReadAllLines("Input.txt");
//var lines = File.ReadAllLines("Example.txt");

var mode = 0;

List<long> seeds = lines[0].Replace("seeds: ", "").Trim().Split(' ').Select(long.Parse).ToList();

var group = new Group();
foreach (var line in lines.Skip(1))
{
    if (string.IsNullOrWhiteSpace(line))
        continue;

    if (line.Contains("map:"))
    {
        mode++;
        continue;
    }
    var parts = line.Split(' ').Select(x => long.Parse(x.Trim())).ToArray();
    var mapping = new Mapping(parts[0], parts[1], parts[2]);
    switch (mode)
    {
        case 1:
            group.SeedToSoil.Add(mapping);
            break;
        case 2:
            group.SoilToFertilizer.Add(mapping);
            break;
        case 3:
            group.FertilizerToWater.Add(mapping);
            break;
        case 4:
            group.WaterToLight.Add(mapping);
            break;
        case 5:
            group.LightToTemp.Add(mapping);
            break;
        case 6:
            group.TempToHumidity.Add(mapping);
            break;
        case 7:
            group.HumidityToLocation.Add(mapping);
            break;
    }
}


var low = long.MaxValue;
foreach (var seed in seeds)
{
    var currentNumber = group.GetMapping(seed);

    if (currentNumber < low)
    {
        low = currentNumber;
    }
}
Console.WriteLine("[1] Lowest Location Number: " + low);



int maxBatchSize = 500000;
var batches = new List<(long start, long end)>();
for (int i = 0; i < seeds.Count; i += 2)
{
    long start = seeds[i];
    long rangeLength = seeds[i + 1];
    long end = start + rangeLength;

    // Split large ranges into smaller batches
    while (start < end)
    {
        long batchEnd = Math.Min(start + maxBatchSize, end);
        batches.Add((start, batchEnd));
        start = batchEnd;
    }
}

Console.WriteLine($"Batch size: {batches.Count}");

int totalBatches = batches.Count;
ConcurrentBag<long> lowestValues = new ConcurrentBag<long>();
int completedBatches = 0;
Parallel.ForEach(batches, (batch) =>
{
    long batchLowest = long.MaxValue;
    for (long j = batch.start; j < batch.end; j++)
    {
        long locationNumber = group.GetMapping(j);
        if (locationNumber < batchLowest)
        {
            batchLowest = locationNumber;
        }
    }

    if (batchLowest < long.MaxValue)
    {
        lowestValues.Add(batchLowest);
    }

    int currentBatch = Interlocked.Increment(ref completedBatches);
    Console.WriteLine($"Progress: {currentBatch}/{totalBatches} batches completed.");
});
long overallLowest = lowestValues.Any() ? lowestValues.Min() : long.MaxValue;


Console.WriteLine("[2] Lowest Location Number: "+ overallLowest);

class Group
{
    public Maps SeedToSoil { get; set; } = new Maps();
    public Maps SoilToFertilizer { get; set; } = new Maps();
    public Maps FertilizerToWater { get; set; } = new Maps();
    public Maps WaterToLight { get; set; } = new Maps();
    public Maps LightToTemp { get; set; } = new Maps();
    public Maps TempToHumidity { get; set; } = new Maps();
    public Maps HumidityToLocation { get; set; } = new Maps();

    public long GetMapping(long currentNumber)
    {
        currentNumber = SeedToSoil.GetMappingNumber(currentNumber);
        currentNumber = SoilToFertilizer.GetMappingNumber(currentNumber);
        currentNumber = FertilizerToWater.GetMappingNumber(currentNumber);
        currentNumber = WaterToLight.GetMappingNumber(currentNumber);
        currentNumber = LightToTemp.GetMappingNumber(currentNumber);
        currentNumber = TempToHumidity.GetMappingNumber(currentNumber);
        currentNumber = HumidityToLocation.GetMappingNumber(currentNumber);
        return currentNumber;
    }
}

class Maps
{
    public List<Mapping> Mappings { get; set; } = new List<Mapping>();

    public void Add(Mapping mapping)
    {
        Mappings.Add(mapping);
        Mappings = Mappings.OrderBy(m => m.Source).ToList();
    }

    public long GetMappingNumber(long number)
    {
        var mappingInRange = Mappings.FirstOrDefault(c => c.IsInRange(number));
        return mappingInRange?.GetLocation(number) ?? number;
    }
}

class Mapping(long offset, long source, long range)
{
    public long Offset { get; set; } = offset;
    public long Source { get; set; } = source;
    public long EndSource => Source + Range;
    public long Range { get; set; } = range;
    
    public long GetLocation(long number)
    {
        if (number < Source || number >= Source + Range) return number;
        return Offset + (number - Source);
    }

    public bool IsInRange(long number)
    {
        return number >= Source && number < EndSource;
    }
}