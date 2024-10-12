using UnityEngine;
using System.Collections.Generic;
using YG;
using System;

public class YandexSaveSystem : MonoBehaviour
{
    List<Action<SavesYG>> savers = new();
    List<Action<SavesYG>> loaders = new();

    public void Register(Action<SavesYG> saver,
                         Action<SavesYG> loader)
    {
        savers.Add(saver);
        loaders.Add(loader);
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    void Load()
    {
        loaders.ForEach(l => l.Invoke(YandexGame.savesData));
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        savers.ForEach(s => s.Invoke(YandexGame.savesData));

        YandexGame.SaveProgress();
    }
}
