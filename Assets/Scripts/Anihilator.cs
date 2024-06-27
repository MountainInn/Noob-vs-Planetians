using UnityEngine;
using UnityEngine.Events;

public class Anihilator : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    [Space]
    [SerializeField] bool alsoDestroySelf;
    [Space]
    [SerializeField] UnityEvent onAnihilate;

    public void __Anihilate() => Anihilate();
    public void Anihilate()
    {
        GameObject.Destroy(objectToDestroy);

        if (alsoDestroySelf)
            GameObject.Destroy(gameObject);

        onAnihilate?.Invoke();
    }
}
