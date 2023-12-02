using System.Text.RegularExpressions;

var lines = File.ReadAllLines("Input.txt");
Console.WriteLine(lines.Sum(x => GetSimpleNumberEx(x, false)));
Console.WriteLine(lines.Sum(x => GetSimpleNumberEx(x, true)));
return;

static int GetSimpleNumberEx(string input, bool useEnglish)
{
    var first = GetFirstDigit(input, useEnglish);
    var last = GetLastDigit(input, useEnglish);
    return int.Parse("" + first + last);
}

static string GetFirstDigit(string input, bool useEnglish)
{
    for (var i = 1; i <= input.Length; i++)
    {
        var segment = input[..i];

        var digits = Regex.Replace(segment, "[^0-9]", "");
        if (!string.IsNullOrEmpty(digits)) return digits;

        //check word
        if (!useEnglish) continue;
        if (segment.Contains("one")) return "1";
        if (segment.Contains("two")) return "2";
        if (segment.Contains("three")) return "3";
        if (segment.Contains("four")) return "4";
        if (segment.Contains("five")) return "5";
        if (segment.Contains("six")) return "6";
        if (segment.Contains("seven")) return "7";
        if (segment.Contains("eight")) return "8";
        if (segment.Contains("nine")) return "9";
    }

    return "0";
}

static string GetLastDigit(string input, bool useEnglish)
{
    for (var i = input.Length; i > 0; i--)
    {
        var segment = input[(i - 1)..];
        var digits = Regex.Replace(segment, "[^0-9]", "");
        if (!string.IsNullOrEmpty(digits)) return digits;

        //check word
        if (!useEnglish) continue;
        if (segment.Contains("one")) return "1";
        if (segment.Contains("two")) return "2";
        if (segment.Contains("three")) return "3";
        if (segment.Contains("four")) return "4";
        if (segment.Contains("five")) return "5";
        if (segment.Contains("six")) return "6";
        if (segment.Contains("seven")) return "7";
        if (segment.Contains("eight")) return "8";
        if (segment.Contains("nine")) return "9";
    }

    return "0";
}