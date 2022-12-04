using solutions;

ReportDaySolution(new Day4(), "The number of fully overlapping pairs {0}", "The numbe of any overlapping pairs {0}");

ReportDaySolution(new Day3(), "The sum of item priorities is {0}", "The sum of group badges is {0}");

ReportDaySolution(new Day2(), "Strategy Score {0}", "Goal Score {0}");

ReportDaySolution(new Day1(), "Highest calorie carried is {0}", "Sum of the top 3 highest calories carried is {0}");



static void ReportDaySolution<T>(IAdventDay<T> day, string? part1Message = null, string? part2Message = null)
{
    Console.WriteLine($"Solution for {day.GetType().Name}:");

    if (part1Message is not null)
        Console.WriteLine(part1Message, day.Part1());

    if (part2Message is not null)
        Console.WriteLine(part2Message, day.Part2());

    Console.WriteLine();
}