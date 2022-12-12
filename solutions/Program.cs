using solutions;
using System.Diagnostics;

//await new Day1().Part1Async();

ReportDaySolution(new Day10(), "{0}", "DONE {0}");

//ReportDaySolution(new Day9(), "{0} unique tail positions");

//ReportDaySolution(new Day8(), "Trees visible from outside {0}");

//ReportDaySolution(new Day7(), "Sum of directories under 100,000 {0}", "Size of directory to remove {0}");

//ReportDaySolution(new Day6(), "Processed elements before SoP {0}", "Processed elements before SoM {0}");

//ReportDaySolution(new Day5(), "Top containers consist of {0}", "Top containers consist of {0}");

//ReportDaySolution(new Day4(), "The number of fully overlapping pairs {0}", "The numbe of any overlapping pairs {0}");

//ReportDaySolution(new Day3(), "The sum of item priorities is {0}", "The sum of group badges is {0}");

//ReportDaySolution(new Day2(), "Strategy Score {0}", "Goal Score {0}");

//ReportDaySolution(new Day1(), "Highest calorie carried is {0}", "Sum of the top 3 highest calories carried is {0}");


static void ReportDaySolution<T>(IAdventDay<T> day, string? part1Message = null, string? part2Message = null)
{
    Console.WriteLine($"Solution for {day.GetType().Name}:");
    T answer;
    if (part1Message is not null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        answer = day.Part1();
        sw.Stop();
        Console.WriteLine(part1Message, $"({sw.Elapsed}): {string.Format(part1Message, answer)}");
    }
    else
        Console.WriteLine("There is no solution for Part 1");

    if (part2Message is not null)
    {
        Stopwatch sw = Stopwatch.StartNew();
        answer = day.Part2();
        sw.Stop();
        Console.WriteLine(part2Message, $"({sw.Elapsed}): {string.Format(part2Message, answer)}");
    }
    else
        Console.WriteLine("There is no solution for Part 2");

    Console.WriteLine();
}