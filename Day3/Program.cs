var lines = File.ReadAllLines("Input.txt");
//var lines = File.ReadAllLines("Example.txt");

var symbols = CreateSymbolCache(lines);
var numbers = CreateNumberCache(lines);

Console.WriteLine(GetValidNumbers());
Console.WriteLine(GetGearRatio());
return;

int GetValidNumbers()
{
    return numbers.Where(CloseToSymbol).Sum(numberPos => numberPos.Number);
}

int GetGearRatio()
{
    var total = 0;
    foreach (var symbolPos in symbols.Where(c=> c.IsGear()))
    {
        var adjacentNumbers = numbers.Where(n => CloseToGear(n, symbolPos)).ToList();
        if (adjacentNumbers.Count == 2)
        {
            total += adjacentNumbers[0].Number * adjacentNumbers[1].Number;
        }
    }
    return total;
}

bool CloseToSymbol(NumberPos numberPos)
{
    var numberLength = numberPos.Number.ToString().Length;
    for (var colOffset = 0; colOffset < numberLength; colOffset++)
    {
        var col = numberPos.Col + colOffset;

        foreach (var symbolPos in symbols)
        {
            if (Math.Abs(symbolPos.Row - numberPos.Row) <= 1 && Math.Abs(symbolPos.Col - col) <= 1)
            {
                return true;
            }
        }
    }
    return false;
}


bool CloseToGear(NumberPos numberPos, SymbolPos symbolPos)
{
    var numberLength = numberPos.Number.ToString().Length;
    for (var offset = 0; offset < numberLength; offset++)
    {
        var col = numberPos.Col + offset;
        if (Math.Abs(numberPos.Row - symbolPos.Row) <= 1 && Math.Abs(col - symbolPos.Col) <= 1)
        {
            return true;
        }
    }
    return false;
}

List<NumberPos> CreateNumberCache(string[] input)
{
    var numberPositions = new List<NumberPos>();

    for (var row = 0; row < input.Length; row++)
    {
        for (var col = 0; col < input[row].Length; col++)
        {
            if (char.IsDigit(input[row][col]))
            {
                var tail = col;
                while (tail < input[row].Length && char.IsDigit(input[row][tail]))
                {
                    tail++;
                }
                var number = int.Parse(input[row].Substring(col, tail - col));
                numberPositions.Add(new NumberPos(row, col, number));
                col = tail - 1;
            }
        }
    }

    return numberPositions;
}

List<SymbolPos> CreateSymbolCache(string[] input)
{
    var symbolPositions = new HashSet<SymbolPos>();

    for (var row = 0; row < input.Length; row++)
    {
        for (var col = 0; col < input[row].Length; col++)
        {
            var letter = input[row][col];
            if (!char.IsDigit(letter) && !char.IsLetter(letter) && letter != '.')
            {
                symbolPositions.Add(new SymbolPos(row, col, letter));
            }
        }
    }
    return symbolPositions.ToList();
}

struct SymbolPos(int row, int col, char symbol)
{
    public int Row = row;
    public int Col = col;
    public char Symbol = symbol;

    public bool IsGear()
    {
        return Symbol == '*';
    }
}

struct NumberPos(int row, int col, int number)
{
    public int Row = row;
    public int Col = col;
    public int Number = number;
}