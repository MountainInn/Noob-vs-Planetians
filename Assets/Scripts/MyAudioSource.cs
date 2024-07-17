using UnityEngine;

public class MyAudioSource : MonoBehaviour
{
    [SerializeField] string id;
    [Space]
    [SerializeField] [Range(0, 1f)] float volume = .75f;
    [Space]
    [SerializeField] [Range(0, 1f)] float minSoundInterval = .1f;
    [Space]
    [SerializeField] AudioClip[] clips;

    void Awake()
    {
        if (id == "")
        {
            id = clips[0].name;
        }

        MyAudioManager.instance.Register(id, minSoundInterval);
    }

    public void __PlayFirst() => PlayFirst();
    public void PlayFirst()
    {
        MyAudioManager.instance
            .Play(clips[0], id, volume, minSoundInterval);
    }

    public void __PlayRandom( ) => PlayRandom();
    public void PlayRandom()
    {
        MyAudioManager.instance
            .Play(clips.GetRandom(), id, volume, minSoundInterval);
    }
}
