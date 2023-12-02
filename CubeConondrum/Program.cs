
using var fs = File.OpenRead("input.txt");
using var sr = new StreamReader(fs);
var lines = sr.ReadAllLines();
var result1 = new GamesResolver(lines).PartOne();
var result2 = new GamesResolver(lines).PartTwo();
Console.WriteLine($"PartOne: {result1}\nPartTwo: {result2}");

class MaxValues
{
    public static int MaxBlue = 14;
    public static int MaxGreen = 13;
    public static int MaxRed = 12;
}

record Set(int Blue, int Green, int Red);
class Game
{
    public int Id { get; init; }
    public IEnumerable<Set> Sets { get; init; }

    public int IsKosher()
    {
        foreach (var set in Sets)
        {
            if(set.Blue > MaxValues.MaxBlue 
                || set.Green > MaxValues.MaxGreen
                || set.Red > MaxValues.MaxRed
            ) return 0;
        }
        return Id;
    }

    public int SmallestPossibleCubes()
    {
        var blue = 0;
        var green = 0;
        var red = 0;

        foreach(var set in Sets)
        {
            if (set.Blue > blue) blue = set.Blue;
            if (set.Green > green) green = set.Green;
            if (set.Red > red) red = set.Red;
        }

        return blue * green * red;
    }
}

class GamesResolver
{
    private readonly IEnumerable<Game> Games;

    public GamesResolver(IEnumerable<string> lines)
    {
        Games = lines.Select(x => ToGame(x));
    }

    public int PartOne() => Games.Select(x => x.IsKosher()).Sum();
    public int PartTwo() => Games.Select(x => x.SmallestPossibleCubes()).Sum();

    private Game ToGame(string line)
    {
        var gameParts = line.Split(": ");
        return new Game
        {
            Id = int.Parse(gameParts[0].Split(" ")[1]),
            Sets = gameParts[1].Split("; ").Select(x => 
            {
                return x.ReadSet();
            })
        };
    }
}

static class Extensions
{
    public static Set ReadSet(this string setStr)
    {
        var attributes = setStr.Split(", ").Select(x => 
        {
            var parts = x.Split(" ");
            return new {
                Key = parts[1],
                Value = int.Parse(parts[0])
            };
        }).ToDictionary(attr => attr.Key, attr => attr.Value);

        attributes.TryGetValue("blue", out var blue);
        attributes.TryGetValue("green", out var green);
        attributes.TryGetValue("red", out var red);

        return new Set(blue, green, red);
    }

    public static IEnumerable<string> ReadAllLines(this StreamReader sr)
    {
        List<string> lines = new();
        while (sr.Peek() >= 0)
        {
            lines.Add(sr.ReadLine());
        }

        return lines;
    }
}
