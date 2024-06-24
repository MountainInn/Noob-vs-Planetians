using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

static public class IEnumerableExt
{
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

    public static void DestroyAll<T>(this IEnumerable<T> source)
        where T : Component
    {
        foreach (var item in source) MonoBehaviour.Destroy(item.gameObject);
    }

    public static void DestroyAllImmediate<T>(this IEnumerable<T> source)
        where T : Component
    {
        foreach (var item in source) MonoBehaviour.DestroyImmediate(item.gameObject);
    }

    public static void DestroyAllImmediate(this IEnumerable<GameObject> source)
    {
        foreach (var item in source) GameObject.DestroyImmediate(item);
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
            .Map(source.Add);

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
        return source == null || !source.Any();
    }
    static public bool None<T>(this IEnumerable<T> source, Func<T, bool> pred)
    {
        return !source.Any(pred);
    }

    static public IEnumerable<T> Map<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source.None())
        {
            return source;
        }

        foreach (var item in source)
        {
            if (item == null) continue;
            action.Invoke(item);
        }

        return source;
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
            (!source.Any())
            ? "Empty"
            : source
            .Select(item => item?.ToString() ?? "NULL")
            .Aggregate((a, b) => a + ", " + b);

        Debug.Log(prefixMessage + $" {source.Count()}: " + str);

        return source;
    }

}
