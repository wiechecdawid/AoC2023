// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

const string pattern = @"[0-9]|(zero)|(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine)";
// const string pattern = @"[0-9]";
var regexLeft = new Regex(pattern);
var regexRight = new Regex(pattern, RegexOptions.RightToLeft);
var sum = 0;
var linesCount = 0;
Dictionary<string, int> dict = new()
{
    ["0"] = 0,
    ["1"] = 1,
    ["2"] = 2,
    ["3"] = 3,
    ["4"] = 4,
    ["5"] = 5,
    ["6"] = 6,
    ["7"] = 7,
    ["8"] = 8,
    ["9"] = 9,
    ["zero"] = 0,
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3,
    ["four"] = 4,
    ["five"] = 5,
    ["six"] = 6,
    ["seven"] = 7,
    ["eight"] = 8,
    ["nine"] = 9
};

using var fs = File.OpenRead("input.txt");
using var sr = new StreamReader(fs);
while (sr.Peek() >= 0)
{
    var line = sr.ReadLine();
    if (string.IsNullOrEmpty(line))
    {
        continue;
    }
    linesCount++;
    var left = dict[regexLeft.Match(line).Value];
    var right = dict[regexRight.Match(line).Value];
    sum = sum + (left * 10) + right;
}

Console.WriteLine(sum);
Console.WriteLine(linesCount);