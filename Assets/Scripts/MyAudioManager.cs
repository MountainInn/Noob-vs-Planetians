using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    static public MyAudioManager instance => _inst;
    static MyAudioManager _inst;
    MyAudioManager(){ _inst = this; }

    Dictionary<string, AudioSource> sources = new();
    Dictionary<AudioSource, float> lastPlaytime = new();

    public void Play(AudioClip clip, float volume, float interval)
    {
        if (!sources.TryGetValue(clip.name, out AudioSource source))
        {
            source = new GameObject($"Source: {clip.name}").AddComponent<AudioSource>();

            sources.Add(clip.name, source);

            source.transform.SetParent(transform);

            lastPlaytime.Add(source, -interval);
        }

        if (Time.time - lastPlaytime[source] >= interval)
        {
            source.PlayOneShot(clip, volume);

            lastPlaytime[source] = Time.time;
        }
    }
}
