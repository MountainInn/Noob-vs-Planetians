using UnityEngine;
using System.Collections.Generic;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] protected List<bool> variants = new();

    public void OnValidate()
    {
        variants.ResizeDestructive(transform.childCount);

        for (int i = 0; i < variants.Count; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(variants[i]);
        }
    }

    public GameObject GetFirstActive()
    {
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
                return item.gameObject;
        }

        return null;
    }
}
