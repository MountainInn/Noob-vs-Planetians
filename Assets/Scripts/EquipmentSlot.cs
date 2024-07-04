using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class EquipmentSlot<T> : MonoBehaviour
    where T : Component
{
    [SerializeField] protected List<bool> toggles = new();

    [SerializeField] T[] equipments;
    T current;

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

        return current;
    }

    public void MaybeSwitchEquipment(int i)
    {
        if (current == equipments[i])
            return;

        T previous = current;
        current = equipments[i];

        foreach (var eq in equipments)
        {
            bool thisToggle = (eq == current);

            if (thisToggle && previous != null)
                OnToggleOn(previous, eq);
            else
                OnToggleOff(eq);

            eq.gameObject.SetActive(thisToggle);
        }
    }

    protected abstract void OnToggleOff(T current);
    protected abstract void OnToggleOn(T previous, T current);

    public T GetFirstActive()
    {
        return equipments.First(eq => eq.gameObject.activeSelf);
    }
}
