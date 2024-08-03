using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

static public class IEnumerableExt
{
    static public IEnumerable<(T1, T2)> Zip<T1, T2>(this (IEnumerable<T1>, IEnumerable<T2>) tuple)
    {
        return tuple.Zip((a, b) => (a, b));
    }
    static public IEnumerable<TResult> Zip<T1, T2, TResult>(this (IEnumerable<T1>, IEnumerable<T2>) tuple, Func<T1, T2, TResult> func)
    {
        using (var e1 = tuple.Item1.GetEnumerator())
        using (var e2 = tuple.Item2.GetEnumerator())
        {
            bool next1 = false;
            bool next2 = false;

            while (true)
            {
                next1 = e1.MoveNext();
                next2 = e2.MoveNext();

                if (next1 || next2)
                    yield return
                        func.Invoke(
                            (next1) ? e1.Current : default,
                            (next2) ? e2.Current : default
                        );
                else
                    yield break;
            }
        }
    }
    static public IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>(this (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) tuple)
    {
        return tuple.Zip((a, b, c) => (a, b, c));
    }
    static public IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) tuple, Func<T1, T2, T3, TResult> func)
    {
        using (var e1 = tuple.Item1.GetEnumerator())
        using (var e2 = tuple.Item2.GetEnumerator())
        using (var e3 = tuple.Item3.GetEnumerator())
        {
            bool next1 = false;
            bool next2 = false;
            bool next3 = false;

            while (true)
            {
                next1 = e1.MoveNext();
                next2 = e2.MoveNext();
                next3 = e3.MoveNext();

                if (next1 || next2 || next3)
                    yield return
                        func.Invoke(
                            (next1) ? e1.Current : default,
                            (next2) ? e2.Current : default,
                            (next3) ? e3.Current : default
                        );
                else
                    yield break;
            }
        }
    }


    static public string JoinAsString(this IEnumerable<string> strs, string delimeter)
    {
        return string.Join(delimeter, strs);
    }
    static public string JoinAsString(this IEnumerable<char> chars)
    {
        return string.Join("", chars);
    }

    public static IEnumerable<float> FloatRange(Func<float> step, float max)
    {
        for (float f = step.Invoke(); f < max; f += step.Invoke())
        {
            if (f < max)
                yield return f;
        }
    }
    public static IEnumerable<T> Scan<T>(this IEnumerable<T> source,
                                         Func<T, T, T> scanner)
    {
        return
            source
            .Skip(1)
            .Aggregate(new [] { source.First() }.AsEnumerable(),
                       (acum, border) =>
                       acum.Append( scanner.Invoke(acum.Last(), border) ));
    }

    public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> source, int size)
    {
        return
            source
            .Select((obj, i) => (obj, chunkIndex: i / size))
            .GroupBy(tuple => tuple.chunkIndex)
            .Select(gr =>
                    gr.Select(tuple => tuple.obj));
    }

    static public T GetRandom<T>(this IEnumerable<T> source)
    {
        int id = UnityEngine.Random.Range(0, source.Count());

        return source.ElementAt(id);
    }

    static public IEnumerable<T> GetRandoms<T>(this IEnumerable<T> source, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int id = UnityEngine.Random.Range(0, source.Count());

            yield return source.ElementAt(id);
        }
    }

    static public IEnumerable<T> GetUniqueRandoms<T>(this IEnumerable<T> source, int count)
    {
        HashSet<int> usedIndices = new();

        for (int i = 0; i < count; )
        {
            int id = UnityEngine.Random.Range(0, source.Count());

            if (usedIndices.Add(id))
            {
                i++;

                yield return source.ElementAt(id);
            }
        }
    }


    static public IEnumerable<T> Takeout<T>(this IEnumerable<T> source, out IEnumerable<T> takeout, Func<T, bool> predicate)
    {
        List<T>
            takeoutList = new List<T>(),
            stayList = new List<T>();

        foreach (var item in source)
        {
            bool v = predicate.Invoke(item);
            if (v)
                takeoutList.Add(item);
            else
                stayList.Add(item);
        }

        takeout = takeoutList;

        return stayList;
    }
    static public IEnumerable<T> Takeout<T>(this IEnumerable<T> source, out IEnumerable<T> takeout)
    {
        takeout = source;
        return source;
    }


    static public IEnumerable<T> Separate<T>(this IEnumerable<T> source, Func<T, bool> predicate, out IEnumerable<T> fals)
    {
        fals = source.Where(item => predicate.Invoke(item) == false);
        return source.Where(item => predicate.Invoke(item) == true);
    }

    static public IEnumerable<T> MultiplyByField<T>(this IEnumerable<T> source, Func<T, int> fieldSelector)
    {
        return source
            .SelectMany(obj =>
                        Enumerable
                        .Range(0, fieldSelector.Invoke(obj))
                        .Select(_ => obj));
    }
    static public Dictionary<TKey, TValue> ToDict<TKey, TValue>(this IEnumerable<(TKey, TValue )> source)
    {
        return source.ToDictionary(kv => kv.Item1,
                                   kv => kv.Item2);
    }
    static public Dictionary<TKey, TValue> ToDict<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> source)
    {
        return source.ToDictionary(kv => kv.Item1,
                                   kv => kv.Item2);
    }
    static public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        return source.ToDictionary(kv => kv.Key,
                                   kv => kv.Value);
    }
    static public IEnumerable<( int index, T value )> Enumerate<T>(this IEnumerable<T> source)
    {
        int i = 0;
        foreach (var item in source)
        {
            yield return (i, item);
            i++;
        }
    }
    static public IEnumerable<T> UnionBy<T>(this ICollection<T> source,
                                            IEnumerable<T> other,
                                            Func<T, object> fieldSelector)
    {
        other.Where(o =>
                    source.None(s =>
                                fieldSelector.Invoke(o) == fieldSelector.Invoke(s)))
            .ForEach(source.Add);

        return source;
    }

    static public IEnumerable<TResult> WhereCast<TResult>(this IEnumerable source)
    {
        foreach (var item in source)
        {
            if (item is TResult)
                yield return (TResult)item;
            else
                continue;
        }
    }
    static public bool None<T>(this IEnumerable<T> source)
    {
        return source == null || !source.Any() || source.Count() == 0;
    }
    static public bool None<T>(this IEnumerable<T> source, Func<T, bool> pred)
    {
        return !source.Any(pred);
    }

    static public IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        return source.Map(action).ToList();
    }


    static public IEnumerable<T> Map<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source.None())
        {
            yield break;
        }

        foreach (var item in source)
        {
            if (item == null) continue;
            action.Invoke(item);

            yield return item;
        }
    }
    static public void Split<T>(this IEnumerable<T> source, Func<T, bool> predicate, out IEnumerable<T> a, out IEnumerable<T> b)
    {
        a = source.Where(predicate);
        b = source.Where(item => predicate.Invoke(item) == false);
    }
    static public IEnumerable<(T, O)> Zip<T, O>(this IEnumerable<T> source, IEnumerable<O> other)
    {
        return source.Zip(other, (a , b) => (a, b));
    }
    static public IEnumerable<(T, O, E)> Zip<T, O, E>(this IEnumerable<T> source, IEnumerable<O> other, IEnumerable<E> elseOther)
    {
        return source.Zip(other, (a , b) => (a, b)).Zip(elseOther, (a, b) => (a.a, a.b, b));
    }

    static public IEnumerable<IEnumerable<T>> Windows<T>(this IEnumerable<T> source, int length)
    {
        for (int i = 0; i < source.Count() - length; i+=length)
        {
            yield return source.Take(length);
        }
    }

    static public IEnumerable<T> InfiniteStream<T>(this IEnumerable<T> source)
    {
        if (source == null || source.Count() == 0)
        {
            Debug.Log($"NONE");
            yield break;
        }

        int n = 0;

        while (true)
        {
            foreach (var item in source)
            {
                yield return item;
            }

            n++;

            if (n > 1000)
            {
                Debug.LogWarning($"----> Stream is too infinite!");
                yield break;
            }
        };
    }

    static public IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(_ => UnityEngine.Random.value);
    }
    static public T GetRandomOrThrow<T>(this IEnumerable<T> source)
    {
        int count = source.Count();

        if (count == 0)
            throw new System.Exception("No items in collection!");

        int id = UnityEngine.Random.Range(0, count);

        return source.ElementAt(id);
    }
    static public T GetRandomOrDefault<T>(this IEnumerable<T> source)
    {
        int count = source.Count();

        if (count == 0)
            return default;

        int id = UnityEngine.Random.Range(0, count);

        return source.ElementAt(id);
    }
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        where T : class
                  {
                      return source.Where(item => item != null);
        }

    static public IEnumerable<T> NotEqual<T>(this IEnumerable<T> source, T other)
    {
        return source.Where(item => !item.Equals(other));
    }
    static public IEnumerable<T> Log<T>(this IEnumerable<T> source, string prefixMessage="")
    {
        string str =
            (!source.Any()) ? "Empty"
            : source
            .Select(item => item?.ToString() ?? "NULL")
            .Aggregate((a, b) => a + ", " + b);

        Debug.Log(prefixMessage + $" {source.Count()}: " + str);

        return source;
    }

    static public IEnumerable<int> ToRange(this int i, int startIndex = 0)
    {
        return Enumerable.Range(startIndex, i);
    }

}
