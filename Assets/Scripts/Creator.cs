using UnityEngine;
using UnityEngine.Events;

public class Creator : MonoBehaviour
{
    [SerializeField] GameObject objectToCreate;
    [Space]
    [SerializeField] UnityEvent onCreation;

    public void Create()
    {
        Instantiate(objectToCreate,
                    transform.position,
                    transform.rotation,
                    null);

        onCreation?.Invoke();
    }
}
