using UnityEngine;
using HyperCasual.Core;

[RequireComponent(typeof(Fade))]
public class MySplashScreen : View
{
    [SerializeField] public Fade fade;

    void OnValidate()
    {
        fade = GetComponent<Fade>();
    }
}
