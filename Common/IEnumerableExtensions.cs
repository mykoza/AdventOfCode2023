namespace AdventOfCode2023;

public static class IEnumerableExtensions
{
    public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        List<T> buffer = [];

        foreach (var item in source)
        {
            if (predicate(item))
            {
                if (buffer.Count > 0)
                {
                    yield return buffer;
                    buffer = [];
                }
            }
            else
            {
                buffer.Add(item);
            }
            
        }

        if (buffer.Count > 0)
        {
            yield return buffer;
        }
    }

    public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> source, T value)
    {
        List<T> buffer = [];

        foreach (var item in source)
        {
            if (item is not null && item.Equals(value))
            {
                if (buffer.Count > 0)
                {
                    yield return buffer;
                    buffer = [];
                }
            }
            else
            {
                buffer.Add(item);
            }
            
        }

        if (buffer.Count > 0)
        {
            yield return buffer;
        }
    }
}
