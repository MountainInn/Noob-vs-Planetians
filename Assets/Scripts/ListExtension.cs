using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

static public class ListExtension
{
    public static void DestroyAll<T>(this List<T> source)
        where T : Component
    {
        foreach (var item in source) MonoBehaviour.Destroy(item.gameObject);
        source.Clear();
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
