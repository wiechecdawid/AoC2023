using System.Text.RegularExpressions;

using var fs = File.OpenRead("input.txt");
using var sr = new StreamReader(fs);
var lines = sr.ReadAllLines().ToArray();
var numbers = lines.SelectMany((line, index) => Helpers.FindNumbers(line, index));

var sum = numbers.Where(x => x.IsPartNumber(lines)).Select(x => x.ToInt()).Sum();
Console.WriteLine(sum);

static class Helpers
{
    public static IEnumerable<string> ReadAllLines(this StreamReader sr)
    {
        List<string> lines = new();
        while (sr.Peek() >= 0)
        {
            lines.Add(sr.ReadLine());
        }

        return lines;
    }

    public static IEnumerable<NumberCandidate> FindNumbers(string line, int lineIndex)
    {
        var candidates = new List<NumberCandidate>();
        List<char> chars = new();
        int? index = null;
        for (int i = 0; i < line.Length; i++)
        {
            if (IsCypher(line[i]))
            {
                chars.Add(line[i]);
                index ??= i;
            }
            else if (index is not null)
            {
                candidates.Add(new(chars, lineIndex, (int)index));
                chars.Clear();
                index = null;
            }
        }

        if (index is not null)
            candidates.Add(new(chars, lineIndex, (int)index));

        return candidates;
    }

    private static bool IsCypher(char ch) => ch - '0' >= 0 && ch - '0' < 10;
}

class NumberCandidate
{
    private const char SpaceSymbol = '.';
    private readonly string _value;
    private readonly int _line;
    private readonly int _index;

    public NumberCandidate(IEnumerable<char> chars, int linesIndex, int lineIndex)
    {
        _value = new string(chars.ToArray());
        _line = linesIndex;
        _index = lineIndex;
    }

    public bool IsPartNumber(string[] lines)
    {
        var previousLine = _line - 1 < 0 ? null : lines[_line - 1];
        var nextLine = _line + 1 == lines.Length ? null : lines[_line + 1]; 
        return IsHorizontallyAdjacent(lines[_index]) || IsDiagonallyAdjacent(previousLine, nextLine);
    }

    public int ToInt() => int.Parse(_value);
    private bool IsHorizontallyAdjacent(string line)
    {
        try
        {
            return line[_index - 1] - SpaceSymbol != 0
            || line[_index + _value.Length] - SpaceSymbol != 0;
        }
        catch (IndexOutOfRangeException)
        {
            if (_index - 1 < 0)
                return line[_index + _value.Length] - SpaceSymbol != 0;
            return line[_index - 1] - SpaceSymbol != 0;
        }
    }

    private bool IsDiagonallyAdjacent(string? previousLine, string? nextLine)
    {
        for (int i = _index - 1; i <= _index + _value.Length; i++)
        {
            try
            {
                if (previousLine?[i] - SpaceSymbol != 0
                    || nextLine?[i] - SpaceSymbol != 0)
                        return true;
            }
            catch(IndexOutOfRangeException)
            {
                continue;
            }
        }
        return false;
    }
}

// ﻿using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text.RegularExpressions;

// class Program
// {
//     public class Schematic
//     {
//         public int? Val { get; set; }
//         public int Start { get; set; }
//         public int End { get; set; }
//         public int Line { get; set; }
//         public string Type { get; set; }
//     }

//     static void Main()
//     {
//         var pattern = new Regex(@"(\d+|[^\.\d\n])");
//         var lines = File.ReadLines("input.txt").Select((line, index) => new { Line = line, Index = index });

//         var schematic = lines.SelectMany(x => pattern.Matches(x.Line).Cast<Match>().Select(m => ParseMatch(m, x.Index))).ToList();

//         // Part 1
//         var part1 = schematic.Where(s => s.Type == "num" && FindAdjacent(schematic, s, new[] { "gear", "part" }).Any()).Sum(s => s.Val);
//         Console.WriteLine("Day 3 (Part 1): " + part1);

//         // Part 2
//         var part2 = schematic.Where(s => s.Type == "gear").Sum(s => GearVal(FindAdjacent(schematic, s, new[] { "num" })));
//         Console.WriteLine("Day 3 (Part 2): " + part2);
//     }

//     static Schematic ParseMatch(Match match, int line)
//     {
//         var val = match.Groups[1].Value.All(char.IsDigit) ? int.Parse(match.Groups[1].Value) : (int?)null;
//         var type = match.Groups[1].Value.All(char.IsDigit) ? "num" : match.Groups[1].Value == "*" ? "gear" : "part";
//         return new Schematic { Val = val, Start = match.Index, End = match.Index + match.Length - 1, Line = line, Type = type };
//     }

//     static List<Schematic> FindAdjacent(List<Schematic> schematic, Schematic row, string[] types)
//     {
//         return schematic.Where(s => types.Contains(s.Type) && Math.Abs(s.Line - row.Line) <= 1 && s.Start <= row.End + 1 && s.End >= row.Start - 1).ToList();
//     }

//     static int GearVal(List<Schematic> adjacent)
//     {
//         return adjacent.Count == 2 ? adjacent[0].Val.Value * adjacent[1].Val.Value : 0;
//     }
// }