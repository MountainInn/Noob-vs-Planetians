using UnityEngine;
using System.Collections.Generic;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] protected List<bool> variants = new();

    int activeId;

    public void OnValidate()
    {
        variants.ResizeDestructive(transform.childCount);

        for (int i = 0; i < variants.Count; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(variants[i]);
        }
    }

    public void MaybeSwitchEquipment(int i)
    {
        if (activeId == i)
            return;
        
        activeId = i;

        for (i = 0; i < variants.Count; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(i == activeId);
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
