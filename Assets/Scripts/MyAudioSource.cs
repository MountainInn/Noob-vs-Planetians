using UnityEngine;

public class MyAudioSource : MonoBehaviour
{
    [SerializeField] [Range(0, 1f)] float volume = .75f;
    [Space]
    [SerializeField] [Range(0, 1f)] float minSoundInterval = .1f;
    [Space]
    [SerializeField] AudioClip[] clips;

    public void __PlayFirst() => PlayFirst();
    public void PlayFirst()
    {
        MyAudioManager.instance.Play(clips[0], volume, minSoundInterval);
    }

    public void __PlayRandom( ) => PlayRandom();
    public void PlayRandom()
    {
        MyAudioManager.instance.Play(clips.GetRandom(), volume, minSoundInterval);
    }
}
