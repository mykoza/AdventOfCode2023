﻿using AdventOfCode2023;

Solution[] days = [ 
    // new Day1(),
    // new Day2(),
    // new Day3(),
    // new Day4(),
    // new Day5(),
    new Day6(),
    new Day7(),
];

foreach (var day in days)
{
    day.Run();
}

Console.WriteLine("Press any key to exit...");
Console.ReadLine();