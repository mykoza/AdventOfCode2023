namespace AdventOfCode2023.Common;

public static class Maths
{
    public static long LeastCommonMultiple(params long[] numbers)
    {
        long lcm = 1;

        foreach (var number in numbers)
        {
            lcm = LeastCommonMultiple(lcm, number);
        }

        return lcm;
    }

    public static long LeastCommonMultiple(IEnumerable<long> numbers)
    {
        long lcm = 1;

        foreach (var number in numbers)
        {
            lcm = LeastCommonMultiple(lcm, number);
        }

        return lcm;
    }

    public static long LeastCommonMultiple(long value1, long value2)
    {
        long a = Math.Abs(value1);
        long b = Math.Abs(value2);

        return a / GreatestCommonDivisor(a, b) * b;
    }

    public static long GreatestCommonDivisor(params long[] numbers)
    {
        long gcd = 1;

        foreach (var number in numbers)
        {
            gcd = GreatestCommonDivisor(gcd, number);
        }

        return gcd;
    }

    public static long GreatestCommonDivisor(IEnumerable<long> numbers)
    {
        long gcd = 1;

        foreach (var number in numbers)
        {
            gcd = GreatestCommonDivisor(gcd, number);
        }

        return gcd;
    }

    public static long GreatestCommonDivisor(long value1, long value2)
    {
        long gcd = 1;

        if (value1 == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value1), "Value cannot be 0");
        }

        if (value2 == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value2), "Value cannot be 0");
        }

        long a = Math.Abs(value1);
        long b = Math.Abs(value2);

        if (a == b) return a;
        if (a > b && a % b == 0) return b;
        if (b > a && b % a == 0) return a;

        while (b != 0)
        {
            gcd = b;
            b = a % b;
            a = gcd;
        }

        return gcd;
    }
}
