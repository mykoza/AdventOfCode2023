﻿using AdventOfCode2023;
using AdventOfCode2023.Common;

Solution[] days = [ 
    // new Day1(),
    // new Day2(),
    // new Day3(),
    // new Day4(),
    // new Day5(),
    // new Day6(),
    // new Day7(),
    // new Day8(),
    // new Day9(),
    // new Day10(),
    new Day11(),
];

foreach (var day in days)
{
    day.Run();
}

Console.WriteLine("Press any key to exit...");
Console.ReadLine();