using UnityEngine;
using System;

public abstract class WithCallbackInterface<I> : MonoBehaviour
{
    I[] callbackProviders;

    protected void Start()
    {
        callbackProviders = GetComponents<I>();
    }

    protected void DoCallbacks(Action<I> callback)
    {
        foreach (var item in callbackProviders)
            callback.Invoke(item);
    }
}
