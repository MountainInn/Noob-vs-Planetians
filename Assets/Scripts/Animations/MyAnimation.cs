using UnityEngine;

[RequireComponent(typeof(Animation))]
public class MyAnimation : MonoBehaviour
{
    Animation anim;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void __Play( ) => Play();
    public void Play()
    {
        anim.Play();
    }
}
