﻿using AdventOfCode2023;

Solution[] days = [ 
    new Day1(),
    new Day2(),
];

foreach (var day in days)
{
    day.Run();
}

Console.WriteLine("Press any key to exit...");
Console.ReadLine();