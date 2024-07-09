using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public abstract class EquipmentSlot<T> : MonoBehaviour
    where T : Component
{
    [SerializeField] public UnityEvent<T> onToggledOn;
    [SerializeField] public UnityEvent<T> onToggledOff;
    [Space]
    [SerializeField] protected List<bool> toggles = new();

    [SerializeField] T[] equipments;
    T current;

    protected virtual int maxAvailableIndex => equipments.Length;

    public void OnValidate()
    {
        equipments = GetComponentsInChildren<T>(true);

        toggles.ResizeDestructive(equipments.Length);

        for (int i = 0; i < toggles.Count; i ++)
        {
            equipments[i].gameObject.SetActive(toggles[i]);
        }
    }

    void Start()
    {
        current = GetFirstActive();

        OnToggleOn(current, current);
        onToggledOn?.Invoke(current);
    }

    public void AddListeners(UnityAction<T> toggledOn, UnityAction<T> toggledOff)
    {
        onToggledOn.AddListener(toggledOn);
        onToggledOff.AddListener(toggledOff);
    }

    public T RandomEquip()
    {
        MaybeSwitchEquipment(UnityEngine.Random.Range(0, maxAvailableIndex));

        return current;
    }

    public void MaybeSwitchEquipment(int i)
    {
        if (current == equipments[i]
            || maxAvailableIndex < i)
            return;

        T previous = current;
        current = equipments[i];

        foreach (var eq in equipments)
        {
            bool thisToggle = (eq == current);

            if (thisToggle && previous != null)
            {
                OnToggleOn(previous, eq);
                onToggledOn?.Invoke(previous);
            }
            else
            {
                OnToggleOff(eq);
                onToggledOff?.Invoke(eq);
            }

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
