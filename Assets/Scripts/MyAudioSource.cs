using UnityEngine;
using HyperCasual.Runner;

public class MyAudioSource : MonoBehaviour
{
    [SerializeField] [Range(0, 1f)] float volume;
    [Space]
    [SerializeField] AudioClip[] clips;

    public void __PlayFirst() => PlayFirst();
    public void PlayFirst()
    {
        AudioManager.Instance.PlayEffect(clips[0], volume);
    }

    public void __PlayRandom( ) => PlayRandom();
    public void PlayRandom()
    {
        AudioManager.Instance.PlayEffect(clips.GetRandom(), volume);
    }
}
