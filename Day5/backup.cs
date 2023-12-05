// using System.Collections.Concurrent;
//
// var lines = File.ReadAllLines("Input.txt");
// //var lines = File.ReadAllLines("Example.txt");
//
// var mode = 0;
//
// var seeds = lines[0].Replace("seeds: ", "").Trim().Split(' ').Select(long.Parse).ToList();
//
// var SeedToSoil = new List<Mapping>();
// var SoilToFertilizer = new List<Mapping>();
// var FertToWater = new List<Mapping>();
// var WaterToLight = new List<Mapping>();
// var LightToTemp = new List<Mapping>();
// var TempToHumidity = new List<Mapping>();
// var HumidityToLocation = new List<Mapping>();
//
// foreach (var line in lines.Skip(1))
// {
//     if (string.IsNullOrWhiteSpace(line))
//         continue;
//
//     if (line.Contains("map:"))
//     {
//         mode++;
//         continue;
//     }
//
//     var parts = line.Split(' ').Select(x => long.Parse(x.Trim())).ToArray();
//     var mapping = new Mapping(parts[0], parts[1], parts[2]);
//
//     switch (mode)
//     {
//         case 1:
//             SeedToSoil.Add(mapping);
//             break;
//         case 2:
//             SoilToFertilizer.Add(mapping);
//             break;
//         case 3:
//             FertToWater.Add(mapping);
//             break;
//         case 4:
//             WaterToLight.Add(mapping);
//             break;
//         case 5:
//             LightToTemp.Add(mapping);
//             break;
//         case 6:
//             TempToHumidity.Add(mapping);
//             break;
//         case 7:
//             HumidityToLocation.Add(mapping);
//             break;
//     }
// }
//
// var locationNumbers = new List<long>();
// foreach (var seed in seeds)
// {
//     long currentNumber = seed;
//
//     currentNumber = Mapping.GetMappingNumber(currentNumber, SeedToSoil);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, SoilToFertilizer);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, FertToWater);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, WaterToLight);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, LightToTemp);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, TempToHumidity);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, HumidityToLocation);
//
//     locationNumbers.Add(currentNumber);
// }
//
//
// object lockObj = new object();
// long smallestLocationNumber = long.MaxValue;
// long totalC = 0;
//
// var rangePartitions = Partitioner.Create(0, seeds.Count / 2);
// Parallel.ForEach(rangePartitions, (range, state) =>
// {
//     long localSmallest = long.MaxValue;
//
//     for (int i = range.Item1; i < range.Item2; i++)
//     {
//         long start = seeds[i * 2];
//         long end = seeds[i * 2 + 1];
//         if (end < start)
//         {
//             (start, end) = (end, start);
//         }
//
//         Console.WriteLine($"Pair {(end-start).ToString("C")}");
//         for (long j = start; j <= end; j++)
//         {
//             long locationNumber = GetFinalLocation(j);
//             if (locationNumber < localSmallest)
//             {
//                 localSmallest = locationNumber;
//             }
//
//             // Update progress
//             Interlocked.Increment(ref totalC);
//             if (totalC % 10000000 == 0)
//             {
//                 Console.WriteLine($"Parts {totalC}");
//             }
//         }
//     }
//
//     lock (lockObj)
//     {
//         if (localSmallest < smallestLocationNumber)
//         {
//             smallestLocationNumber = localSmallest;
//         }
//     }
// });
//
//
//
// // Find the lowest location number
// long lowestLocationNumber = locationNumbers.Min();
//
// Console.WriteLine("[1] Lowest Location Number: " + lowestLocationNumber);
// Console.WriteLine("[2] Lowest Location Number: "+ smallestLocationNumber);
//
//
// long GetFinalLocation(long currentNumber)
// {
//     currentNumber = Mapping.GetMappingNumber(currentNumber, SeedToSoil);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, SoilToFertilizer);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, FertToWater);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, WaterToLight);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, LightToTemp);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, TempToHumidity);
//     currentNumber = Mapping.GetMappingNumber(currentNumber, HumidityToLocation);
//
//     return currentNumber;
// }
// long ConvertThroughMappings(long number, List<Mapping> mappings)
// {
//     foreach (var mapping in mappings)
//     {
//         if (number >= mapping.Source && number < mapping.Source + mapping.Range)
//         {
//             return mapping.Destination + (number - mapping.Source);
//         }
//     }
//
// // If the number does not match any range, return it as is
//     return number;
// }
//
//
// class Mapping
// {
//     public long Destination { get; set; }
//     public long Source { get; set; }
//     public long Range { get; set; }
//
//     public Mapping(long destination, long source, long range)
//     {
//         Destination = destination;
//         Source = source;
//         Range = range;
//     }
//
//     public static long GetMappingNumber(long number, List<Mapping> mappings)
//     {
//
//
//         foreach (var mapping in mappings)
//         {
//             if (number >= mapping.Source && number < mapping.Source + mapping.Range)
//             {
//                 long convertedNumber = mapping.Destination + (number - mapping.Source);
//                 return convertedNumber;
//             }
//         }
//
//         // If the number does not match any range, return it as is
//         return number;
//     }
// }