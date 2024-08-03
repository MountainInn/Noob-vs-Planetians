using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

static public class ListExtension
{
        public static void Deconstruct<T1, T2>(this List<(T1, T2)> source, out List<T1> list1, out List<T2> list2)
    {
        list1 = new List<T1>();
        list2 = new List<T2>();

        foreach (var (item1, item2) in source)
        {
            list1.Add(item1);
            list2.Add(item2);
        }
    }

    public static void DestroyAll<T>(this List<T> list)
        where T : Component
    {
        foreach (var item in list)
        {
            if (item == null)
                continue;

            if (Application.isPlaying)
                MonoBehaviour.Destroy(item.gameObject);
            else
                MonoBehaviour.DestroyImmediate(item.gameObject);
        }

        list.Clear();
    }

    public static void ResizeDestructive<T>(this List<T> list, int targetSize)
    {
        ResizeDestructive(list, targetSize, () => default, null);
    }

    public static void ResizeDestructive<T>(this List<T> list,
                                            int targetSize,
                                            Func<T> createNew,
                                            Action<T> onRemove)
    {
        int diff = (targetSize - list.Count);

        if (diff == 0)
            return;

        for (int i = 0; i < Mathf.Abs(diff); i++)
        {
            int lastIndex = Mathf.Max(0, list.Count - 1);

            if (diff > 0)
            {
                list.Add(createNew.Invoke());
            }
            else if (diff < 0)
            {
                onRemove?.Invoke(list[lastIndex]);

                list.RemoveAt(lastIndex);
            }
        }
    }
}
