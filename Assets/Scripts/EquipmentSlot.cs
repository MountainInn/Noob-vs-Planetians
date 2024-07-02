using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class EquipmentSlot<T> : MonoBehaviour
    where T : Component
{
    [SerializeField] protected List<bool> toggles = new();

    protected int activeId = -1;

    [SerializeField] T[] equipments;

    public void OnValidate()
    {
        equipments = GetComponentsInChildren<T>(true);

        toggles.ResizeDestructive(equipments.Length);

        for (int i = 0; i < toggles.Count; i ++)
        {
            equipments[i].gameObject.SetActive(toggles[i]);
        }
    }

    public T RandomEquip()
    {
        MaybeSwitchEquipment(UnityEngine.Random.Range(0, equipments.Length));

        return equipments[activeId];
    }

    public void MaybeSwitchEquipment(int i)
    {
        if (activeId == i)
            return;
        
        activeId = i;

        for (i = 0; i < toggles.Count; i ++)
        {
            bool toggle = (i == activeId);

            OnToggle(equipments[i], toggle);

            equipments[i].gameObject.SetActive(toggle);
        }
    }

    protected abstract void OnToggle(T equipment, bool toggle);

    public T GetFirstActive()
    {
        return equipments.First(eq => eq.gameObject.activeSelf);
    }
}
