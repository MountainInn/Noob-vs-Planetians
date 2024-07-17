using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    static public MyAudioManager instance => _inst;
    static MyAudioManager _inst;
    MyAudioManager(){ _inst = this; }

    Dictionary<string, AudioSource> sources = new();
    Dictionary<AudioSource, float> lastPlaytime = new();

    public void Register(string id, float interval)
    {
        if (!sources.TryGetValue(id, out AudioSource source))
        {
            source = new GameObject($"Source: {id}").AddComponent<AudioSource>();

            source.transform.SetParent(transform);

            lastPlaytime.Add(source, -interval);

            sources.Add(id, source);
        }
    }

    public void Play(AudioClip clip, string id, float volume, float interval)
    {
        AudioSource source = sources[id];

        if (Time.time - lastPlaytime[source] >= interval)
        {
            source.PlayOneShot(clip, volume);

            lastPlaytime[source] = Time.time;
        }
    }
}
