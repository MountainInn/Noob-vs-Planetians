using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

static public class ListExtension
{
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
